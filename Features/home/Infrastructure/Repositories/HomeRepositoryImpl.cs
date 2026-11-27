using AppFinalAv4A.Features.home.Domain.Datasource;
using AppFinalAv4A.Features.home.Domain.Entities;
using AppFinalAv4A.Features.home.Domain.Repositories;

namespace AppFinalAv4A.Features.home.Infrastructure.Repositories;

public class HomeRepositoryImpl : IHomeRepository
{
    readonly IHomeDataSource _dataSource;

    public HomeRepositoryImpl(IHomeDataSource dataSource) => _dataSource = dataSource;

    public Task<DaySummary> GetDaySummaryAsync(CancellationToken ct = default)
        => _dataSource.GetDaySummaryAsync(ct);
}
