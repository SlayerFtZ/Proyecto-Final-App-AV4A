using AppFinalAv4A.Features.auth.Domain.Entities;
using AppFinalAv4A.Features.auth.Infrastructure.Models;

namespace AppFinalAv4A.Features.auth.Infrastructure.Mappers;

public static class AuthMapper
{
    public static AuthGoogle ToEntity(AuthModel model) => new()
    {
        Id = model.UserProfile.Sub,
        Email = model.UserProfile.Email,
        Name = model.UserProfile.Name,
        PictureProfileUrl = model.UserProfile.PictureProfileUrl,
        AccessToken = model.Token.AccessToken,
        RefreshToken = model.Token.RefreshToken,
        IdToken = model.Token.IdToken,
        ExpiresAt = model.ObtainedAt.AddSeconds(model.Token.ExpiresIn),
    };
}