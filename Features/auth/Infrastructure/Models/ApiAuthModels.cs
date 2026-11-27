using System.Text.Json.Serialization;

namespace AppFinalAv4A.Features.auth.Infrastructure.Models;

/// <summary>Cuerpo de POST /api/auth/google.</summary>
public class GoogleLoginRequestModel
{
    [JsonPropertyName("idToken")]
    public string IdToken { get; set; } = string.Empty;
}

/// <summary>Respuesta de POST /api/auth/google (AuthResponse del backend).</summary>
public class ApiAuthResponseModel
{
    [JsonPropertyName("accessToken")]
    public string AccessToken { get; set; } = string.Empty;

    [JsonPropertyName("tokenType")]
    public string? TokenType { get; set; }

    [JsonPropertyName("expiresIn")]
    public long ExpiresIn { get; set; }

    [JsonPropertyName("expiresAt")]
    public DateTimeOffset ExpiresAt { get; set; }

    [JsonPropertyName("user")]
    public ApiUserModel? User { get; set; }
}

/// <summary>Usuario registrado en la tabla Usuarios (UserResponse del backend).</summary>
public class ApiUserModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("photoUrl")]
    public string? PhotoUrl { get; set; }
}
