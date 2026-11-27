using System.Text.Json.Serialization;

namespace AppFinalAv4A.Features.auth.Infrastructure.Models;

public class AuthModel
{
    public required TokenModelAuth Token {get; set;}
    public required UserInfoModel UserProfile{get; set;} 
    public DateTimeOffset ObtainedAt {get; set;} = DateTimeOffset.UtcNow;

    // JWT de la API (POST /api/auth/google). Vence según el backend (9 h).
    public string? ApiAccessToken {get; set;}
    public DateTimeOffset? ApiTokenExpiresAt {get; set;}
    public int? ApiUserId {get; set;}
}