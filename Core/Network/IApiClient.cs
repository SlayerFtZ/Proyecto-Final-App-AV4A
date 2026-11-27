namespace AppFinalAv4A.Core.Network;

public interface IApiClient
{
    Task<TResponse> GetAsync<TResponse>(string path, CancellationToken ct = default)
        where TResponse : class;

    Task<TResponse> PostAsync<TRequest, TResponse>(
        string path, TRequest body, bool authorize = true, CancellationToken ct = default)
        where TResponse : class;

    Task<TResponse> PutAsync<TRequest, TResponse>(
        string path, TRequest body, CancellationToken ct = default)
        where TResponse : class;

    Task DeleteAsync(string path, CancellationToken ct = default);
}
