using AppFinalAv4A.Core.Network;
using AppFinalAv4A.Features.home.Domain.Datasource;
using AppFinalAv4A.Features.home.Domain.Entities;
using AppFinalAv4A.Features.home.Infrastructure.Mappers;
using AppFinalAv4A.Features.home.Infrastructure.Models;

namespace AppFinalAv4A.Features.home.Infrastructure.Datasource;

public class HomeDataSourceImpl : IHomeDataSource
{
    readonly IApiClient _api;

    public HomeDataSourceImpl(IApiClient api) => _api = api;

    public async Task<DaySummary> GetDaySummaryAsync(CancellationToken ct = default)
    {
        var response = await _api
            .GetAsync<SummaryResponseModel>("api/resumen/datos", ct)
            .ConfigureAwait(false);

        var data = response.Data ?? new DaySummaryDataModel();
        return HomeMapper.ToEntity(data, DateOnly.FromDateTime(DateTime.Now));
    }
}
