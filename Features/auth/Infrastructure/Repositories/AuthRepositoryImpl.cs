using AppFinalAv4A.Features.auth.Domain.Datasource;
using AppFinalAv4A.Features.auth.Domain.Entities;
using AppFinalAv4A.Features.auth.Domain.Repositories;

namespace AppFinalAv4A.Features.auth.Infrastructure.Repositories;

public class AuthRepositoryImpl : IAuthRepository

{
    readonly IAuthDataSource _dataSourse;

    public AuthRepositoryImpl(IAuthDataSource dataSource) => _dataSourse = dataSource;

    public Task<AuthGoogle> SingInWithGoogleAsync(CancellationToken ct = default) => _dataSourse.SignInAsync(ct);
    public Task SingOutAsync() => _dataSourse.SignOutAsync();

    public Task<AuthGoogle?> GetCurrentSessionAsync() => _dataSourse.GetCurrentSessionAsync();

   
}