using AppFinalAv4A.Features.auth.Domain.Entities;

namespace AppFinalAv4A.Features.auth.Domain.Datasource
{
   public interface IAuthDataSource
    {
        Task<AuthGoogle>SignInAsync(CancellationToken ct = default);
        Task SignOutAsync();
        Task<AuthGoogle>GetCurrentSessionAsync();
    }
}