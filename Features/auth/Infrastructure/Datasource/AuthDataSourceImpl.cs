using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using AppFinalAv4A.Features.auth.Domain.Datasource;
using AppFinalAv4A.Features.auth.Domain.Entities;
using AppFinalAv4A.Features.auth.Infrastructure.Mappers;
using AppFinalAv4A.Features.auth.Infrastructure.Models;
using AppFinalAv4A.Features.auth.Service;

namespace AppFinalAv4A.Features.auth.Infrastructure.Datasource;

public class AuthDataSourceImpl : IAuthDataSource
{
    readonly HttpClient _http;
    readonly TokenStorageService _storage;

    public AuthDataSourceImpl(HttpClient http, TokenStorageService storage)
    {
        _http = http;
        _storage = storage;
    }

    public async Task<AuthGoogle> SignInAsync(CancellationToken ct = default)
    {
        var codeVerifier = GenerateCodeVerifier();
        var codeChallenge = ComputeCodeChallenge(codeVerifier);
        var state = Guid.NewGuid().ToString("N");

        var authUrl = new Uri(
            $"{GoogleAuthConfig.AuthEndpoint}" +
            $"?client_id={Uri.EscapeDataString(GoogleAuthConfig.ClientId)}" +
            $"&redirect_uri={Uri.EscapeDataString(GoogleAuthConfig.RedirectUri)}" +
            $"&response_type=code" +
            $"&scope={Uri.EscapeDataString(GoogleAuthConfig.Scopes)}" +
            $"&code_challenge={codeChallenge}" +
            $"&code_challenge_method=S256" +
            $"&state={state}");
        var result = await WebAuthenticator.Default.AuthenticateAsync(
            new WebAuthenticatorOptions
            {
                Url = authUrl,
                CallbackUrl = new Uri(GoogleAuthConfig.RedirectUri),
                PrefersEphemeralWebBrowserSession = false
            });

    
        if (!result.Properties.TryGetValue("state", out var returnedState) || returnedState != state)
            throw new InvalidOperationException("El parámetro 'state' no coincide.");

        if (result.Properties.TryGetValue("error", out var error))
            throw new InvalidOperationException($"Google devolvió un error: {error}");

        if (!result.Properties.TryGetValue("code", out var code))
            throw new InvalidOperationException("No se recibió el código de autorización.");

    
        var token = await ExchangeCodeAsync(code, codeVerifier, ct);

    
        var user = await GetUserInfoAsync(token.AccessToken, ct);

        var model = new AuthModel { Token = token, UserProfile = user };
        await _storage.SaveAsync(model);

        return AuthMapper.ToEntity(model);
    }

    public async Task<AuthGoogle?> GetCurrentSessionAsync()
    {
        var model = await _storage.LoadAsync();
        return model is null ? null : AuthMapper.ToEntity(model);
    }

    public Task SignOutAsync()
    {
        _storage.Clear();
        return Task.CompletedTask;
    }


    async Task<TokenModelAuth> ExchangeCodeAsync(string code, string codeVerifier, CancellationToken ct)
    {
        var body = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["client_id"] = GoogleAuthConfig.ClientId,
            ["code"] = code,
            ["code_verifier"] = codeVerifier,
            ["grant_type"] = "authorization_code",
            ["redirect_uri"] = GoogleAuthConfig.RedirectUri
        });

        using var response = await _http.PostAsync(GoogleAuthConfig.TokenEndpoint, body, ct);
        if (!response.IsSuccessStatusCode)
        {
            var detail = await response.Content.ReadAsStringAsync(ct);
            throw new HttpRequestException($"Error al obtener tokens ({(int)response.StatusCode}): {detail}");
        }

        return await response.Content.ReadFromJsonAsync<TokenModelAuth>(cancellationToken: ct)
               ?? throw new InvalidOperationException("Respuesta de token vacía.");
    }

    async Task<UserInfoModel> GetUserInfoAsync(string accessToken, CancellationToken ct)
{
    if (string.IsNullOrWhiteSpace(accessToken))
        throw new InvalidOperationException("El access_token llegó vacío. Revisa los [JsonPropertyName] de TokenResponseModel.");

    using var request = new HttpRequestMessage(HttpMethod.Get, GoogleAuthConfig.UserInfoEndpoint);
    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

    using var response = await _http.SendAsync(request, ct);
    if (!response.IsSuccessStatusCode)
    {
        var detail = await response.Content.ReadAsStringAsync(ct);
        var www = string.Join(" | ", response.Headers.WwwAuthenticate.Select(h => h.ToString()));
        throw new HttpRequestException($"userinfo {(int)response.StatusCode}: {detail} {www}");
    }

    return await response.Content.ReadFromJsonAsync<UserInfoModel>(cancellationToken: ct)
           ?? throw new InvalidOperationException("Respuesta de userinfo vacía.");
}

    static string GenerateCodeVerifier()
    {
        var bytes = RandomNumberGenerator.GetBytes(32);
        return Base64UrlEncode(bytes);
    }

    static string ComputeCodeChallenge(string verifier)
    {
        var hash = SHA256.HashData(Encoding.ASCII.GetBytes(verifier));
        return Base64UrlEncode(hash);
    }

    static string Base64UrlEncode(byte[] data) =>
        Convert.ToBase64String(data)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
}