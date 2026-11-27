using AppFinalAv4A.Core.Localization;
using AppFinalAv4A.Core.Ui;

namespace AppFinalAv4A.Features.home.Domain.Entities;


public sealed class OutOfStockAlert : AlertItem
{
    private int _minimumStock;

    public OutOfStockAlert(int productId, string productName, int minimumStock)
        : base(productId, productName)
    {
        MinimumStock = minimumStock;
    }

    public int MinimumStock
    {
        get => _minimumStock;
        private set => _minimumStock = value >= 0
            ? value
            : throw new ArgumentOutOfRangeException(nameof(MinimumStock));
    }

    public override AlertSeverity Severity => AlertSeverity.Critical;

    public override string Icon => Icons.OutOfStock;


    public override int Priority => -1;

    protected override string BuildDetail(ILocalizationService loc)
        => string.Format(loc.GetString("Out of stock · minimum {0}"), MinimumStock);
}

public sealed class LowStockAlert : AlertItem
{
    private int _stock;
    private int _minimumStock;

    public LowStockAlert(int productId, string productName, int stock, int minimumStock)
        : base(productId, productName)
    {
        Stock = stock;
        MinimumStock = minimumStock;
    }

    public int Stock
    {
        get => _stock;
        private set => _stock = value > 0
            ? value
            : throw new ArgumentOutOfRangeException(nameof(Stock), "Para stock 0 usa OutOfStockAlert.");
    }

    public int MinimumStock
    {
        get => _minimumStock;
        private set => _minimumStock = value >= 0
            ? value
            : throw new ArgumentOutOfRangeException(nameof(MinimumStock));
    }

    public int Missing => Math.Max(0, MinimumStock - Stock);

    public override AlertSeverity Severity => AlertSeverity.Warning;

    public override string Icon => Icons.LowStock;

    protected override string BuildDetail(ILocalizationService loc)
        => string.Format(loc.GetString("Stock: {0} · minimum {1}"), Stock, MinimumStock);
}

public sealed class ExpiryAlert : AlertItem
{
    private readonly DateOnly _today;

    public ExpiryAlert(int productId, string productName, DateOnly expirationDate, DateOnly today)
        : base(productId, productName)
    {
        ExpirationDate = expirationDate;
        _today = today;
    }

    public DateOnly ExpirationDate { get; }


    public int DaysLeft => ExpirationDate.DayNumber - _today.DayNumber;

    public bool IsExpired => DaysLeft < 0;

    public override AlertSeverity Severity => DaysLeft <= 2 ? AlertSeverity.Critical : AlertSeverity.Warning;

    public override string Icon => IsExpired ? Icons.Expired : Icons.Expiring;


    public override int Priority => base.Priority + Math.Clamp(DaysLeft, -5, 7) + 5;

    protected override string BuildDetail(ILocalizationService loc)
    {
        if (IsExpired)
        {
            var days = -DaysLeft;
            return string.Format(loc.GetPluralString("Expired {0} day ago", "Expired {0} days ago", days), days);
        }

        if (DaysLeft == 0)
            return loc.GetString("Expires today");

        return string.Format(loc.GetPluralString("Expires in {0} day", "Expires in {0} days", DaysLeft), DaysLeft);
    }
}
