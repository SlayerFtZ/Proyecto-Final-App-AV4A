using System.Text.Json.Serialization;

namespace AppFinalAv4A.Features.auth.Infrastructure.Models;

public class UserInfoModel
{
    [JsonPropertyName("sub")]
    public string Sub {get; set;} = string.Empty;

    [JsonPropertyName("email")]
    public string Email {get; set;} = string.Empty;

    [JsonPropertyName("name")]
    public string? Name{get; set;}

    [JsonPropertyName("picture")]
    public string? PictureProfileUrl{get; set;}
}