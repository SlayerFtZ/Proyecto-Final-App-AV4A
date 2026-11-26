using System.Text.Json.Serialization;

namespace AppFinalAv4A.Features.auth.Infrastructure.Models;

public class AuthModel
{
    public required TokenModelAuth Token {get; set;}
    public required UserInfoModel UserProfile{get; set;} 
    public DateTimeOffset ObtainedAt {get; set;} = DateTimeOffset.UtcNow;
}