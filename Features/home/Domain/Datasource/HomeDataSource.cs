using AppFinalAv4A.Features.home.Domain.Entities;

namespace AppFinalAv4A.Features.home.Domain.Datasource;

public interface IHomeDataSource
{
  
    Task<DaySummary> GetDaySummaryAsync(CancellationToken ct = default);
}
