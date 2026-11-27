namespace AppFinalAv4A.Features.auth.Domain.Entities;    
public class AuthGoogle
{
    public required string Id {get; set;}
    public required string Email {get; set;}
    public required string? Name {get; set;}
    public required string? PictureProfileUrl {get; init;}
    public required string? AccessToken {get; init;}
    public  string? RefreshToken {get; init;}
    public string? IdToken {get; init;}
    public DateTimeOffset ExpiresAt {get; init;}

    public bool IsExpired => DateTimeOffset.UtcNow >= ExpiresAt;

    // ── Sesión con la API (JWT propio del backend) ──
    public string? ApiAccessToken {get; init;}
    public DateTimeOffset? ApiTokenExpiresAt {get; init;}
    public int? ApiUserId {get; init;}

    public bool HasValidApiSession =>
        !string.IsNullOrEmpty(ApiAccessToken)
        && ApiTokenExpiresAt is { } exp
        && DateTimeOffset.UtcNow < exp;

}