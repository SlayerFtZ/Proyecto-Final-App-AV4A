using System.Text.Json.Serialization;

namespace AppFinalAv4A.Features.home.Infrastructure.Models;

public class SummaryResponseModel
{
    [JsonPropertyName("data")]
    public DaySummaryDataModel? Data { get; set; }
}

public class DaySummaryDataModel
{
    [JsonPropertyName("totalSalesToday")]
    public long TotalSalesToday { get; set; }

    [JsonPropertyName("totalRevenue")]
    public decimal TotalRevenue { get; set; }

    [JsonPropertyName("lowStockProducts")]
    public List<LowStockItemModel> LowStockProducts { get; set; } = new();

    [JsonPropertyName("expiringProducts")]
    public List<ExpiryItemModel> ExpiringProducts { get; set; } = new();

    [JsonPropertyName("topSellers")]
    public List<TopSellerItemModel> TopSellers { get; set; } = new();
}

public class LowStockItemModel
{
    [JsonPropertyName("productId")]
    public int ProductId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("stock")]
    public int Stock { get; set; }

    [JsonPropertyName("minimumStock")]
    public int MinimumStock { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }
}

public class ExpiryItemModel
{
    [JsonPropertyName("productId")]
    public int ProductId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("expirationDate")]
    public DateOnly ExpirationDate { get; set; }

    [JsonPropertyName("stock")]
    public int Stock { get; set; }
}

public class TopSellerItemModel
{
    [JsonPropertyName("productId")]
    public int ProductId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("unitsSold")]
    public long UnitsSold { get; set; }
}
