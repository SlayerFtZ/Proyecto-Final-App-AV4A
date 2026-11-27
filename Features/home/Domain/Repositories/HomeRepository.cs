using AppFinalAv4A.Features.home.Domain.Entities;

namespace AppFinalAv4A.Features.home.Domain.Repositories;

public interface IHomeRepository
{
    Task<DaySummary> GetDaySummaryAsync(CancellationToken ct = default);
}
