using System.Text.Json;
using AppFinalAv4A.Features.auth.Infrastructure.Models;

public class TokenStorageService
{
    const string key = "google_auth_session";

    public async Task SaveAsync(AuthModel model)
    {
        var json = JsonSerializer.Serialize(model);
        await SecureStorage.Default.SetAsync(key, json);   
    }

    public async Task<AuthModel?> LoadAsync()
    {
        try
        {
            var json = await SecureStorage.Default.GetAsync(key);
            return string.IsNullOrEmpty(json)? null : JsonSerializer.Deserialize<AuthModel>(json);
        }catch
        {
            Clear();
            return null;
        }
    }

     public void Clear()
    {
        SecureStorage.Default.Remove(key);
    }
}