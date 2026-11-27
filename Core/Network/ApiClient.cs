using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace AppFinalAv4A.Core.Network;

public sealed class ApiClient : IApiClient
{
    static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    static readonly Uri BaseUri = new(ApiConfig.BaseUrl);

    readonly HttpClient _http;
    readonly IAccessTokenProvider _tokens;

    public ApiClient(HttpClient http, IAccessTokenProvider tokens)
    {
        _http = http;
        _tokens = tokens;
    }

    public Task<TResponse> GetAsync<TResponse>(string path, CancellationToken ct = default)
        where TResponse : class
        => SendAsync<TResponse>(HttpMethod.Get, path, content: null, authorize: true, ct);

    public Task<TResponse> PostAsync<TRequest, TResponse>(
        string path, TRequest body, bool authorize = true, CancellationToken ct = default)
        where TResponse : class
        => SendAsync<TResponse>(HttpMethod.Post, path, JsonContent.Create(body, options: JsonOptions), authorize, ct);

    public Task<TResponse> PutAsync<TRequest, TResponse>(
        string path, TRequest body, CancellationToken ct = default)
        where TResponse : class
        => SendAsync<TResponse>(HttpMethod.Put, path, JsonContent.Create(body, options: JsonOptions), authorize: true, ct);

    public async Task DeleteAsync(string path, CancellationToken ct = default)
    {
        using var response = await SendRawAsync(HttpMethod.Delete, path, content: null, authorize: true, ct)
            .ConfigureAwait(false);
    }

    async Task<TResponse> SendAsync<TResponse>(
        HttpMethod method, string path, HttpContent? content, bool authorize, CancellationToken ct)
        where TResponse : class
    {
        using var response = await SendRawAsync(method, path, content, authorize, ct).ConfigureAwait(false);

        return await response.Content.ReadFromJsonAsync<TResponse>(JsonOptions, ct).ConfigureAwait(false)
               ?? throw new ApiException(response.StatusCode, "La respuesta del servidor llegó vacía.");
    }

    async Task<HttpResponseMessage> SendRawAsync(
        HttpMethod method, string path, HttpContent? content, bool authorize, CancellationToken ct)
    {
        using var request = new HttpRequestMessage(method, new Uri(BaseUri, path)) { Content = content };

        if (authorize)
        {
            var token = await _tokens.GetAccessTokenAsync().ConfigureAwait(false);
            if (string.IsNullOrEmpty(token))
                throw new ApiException(HttpStatusCode.Unauthorized, "Tu sesión expiró. Inicia sesión de nuevo.");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(ct);
        timeout.CancelAfter(ApiConfig.RequestTimeout);

        HttpResponseMessage response;
        try
        {
            response = await _http.SendAsync(request, timeout.Token).ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (!ct.IsCancellationRequested)
        {
            throw new ApiException(HttpStatusCode.RequestTimeout,
                "El servidor tardó demasiado en responder. Inténtalo de nuevo.");
        }
               catch (HttpRequestException ex)
        {
            System.Diagnostics.Debug.WriteLine($"[API] {method} {request.RequestUri} → {ex.GetType().Name}: {ex.Message} | {ex.InnerException?.Message}");
            throw new ApiException(null,
                "No se pudo conectar con el servidor. Revisa tu conexión.", inner: ex);
        }

        if (response.IsSuccessStatusCode)
            return response;

        var status = response.StatusCode;
        var error = await TryReadErrorAsync(response, ct).ConfigureAwait(false);
        response.Dispose();

        throw new ApiException(status,
            error?.Message ?? $"El servidor respondió con un error ({(int)status}).",
            error?.Code);
    }

    static async Task<ApiErrorResponse?> TryReadErrorAsync(HttpResponseMessage response, CancellationToken ct)
    {
        try
        {
            return await response.Content.ReadFromJsonAsync<ApiErrorResponse>(JsonOptions, ct).ConfigureAwait(false);
        }
        catch
        {
            return null;
        }
    }
}
