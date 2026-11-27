namespace AppFinalAv4A.Features.home.Domain.Entities;


public sealed class DaySummary
{
    public required long SalesCount { get; init; }

    public required decimal Revenue { get; init; }

  
    public required IReadOnlyList<AlertItem> Alerts { get; init; }

    public required IReadOnlyList<TopSeller> TopSellers { get; init; }
}


public sealed record TopSeller(int ProductId, string Name, long UnitsSold, int Rank);
