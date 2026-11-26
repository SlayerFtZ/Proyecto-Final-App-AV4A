using AppFinalAv4A.Features.auth.Domain.Entities;

namespace AppFinalAv4A.Features.auth.Domain.Repositories;

public interface IAuthRepository
{
    Task<AuthGoogle>SingInWithGoogleAsync(CancellationToken ct = default);
    Task SingOutAsync();
    Task<AuthGoogle?> GetCurrentSessionAsync();
}