using AppFinalAv4A.Core.Network;

namespace AppFinalAv4A.Features.auth.Service;

/// <summary>
/// Lee el JWT de la API guardado en SecureStorage. Devuelve null si no existe o ya venció.
/// </summary>
public sealed class AccessTokenProvider : IAccessTokenProvider
{
    readonly TokenStorageService _storage;

    public AccessTokenProvider(TokenStorageService storage) => _storage = storage;

    public async Task<string?> GetAccessTokenAsync()
    {
        var model = await _storage.LoadAsync();

        if (string.IsNullOrEmpty(model?.ApiAccessToken))
            return null;

        if (model.ApiTokenExpiresAt is not { } expiresAt || DateTimeOffset.UtcNow >= expiresAt)
            return null;

        return model.ApiAccessToken;
    }
}
