using AppFinalAv4A.Features.home.Domain.Entities;
using AppFinalAv4A.Features.home.Infrastructure.Models;

namespace AppFinalAv4A.Features.home.Infrastructure.Mappers;

public static class HomeMapper
{
    public static DaySummary ToEntity(DaySummaryDataModel model, DateOnly today)
    {
        
        var alerts = new List<AlertItem>();

        foreach (var p in model.LowStockProducts)
        {
            alerts.Add(p.Stock <= 0
                ? new OutOfStockAlert(p.ProductId, p.Name, p.MinimumStock)
                : new LowStockAlert(p.ProductId, p.Name, p.Stock, p.MinimumStock));
        }

        foreach (var p in model.ExpiringProducts)
            alerts.Add(new ExpiryAlert(p.ProductId, p.Name, p.ExpirationDate, today));

        var ordered = alerts
            .OrderBy(a => a.Priority)
            .ThenBy(a => a.ProductName, StringComparer.CurrentCultureIgnoreCase)
            .ToList();

        var topSellers = model.TopSellers
            .Select((t, index) => new TopSeller(t.ProductId, t.Name, t.UnitsSold, index + 1))
            .ToList();

        return new DaySummary
        {
            SalesCount = model.TotalSalesToday,
            Revenue = model.TotalRevenue,
            Alerts = ordered,
            TopSellers = topSellers
        };
    }
}
