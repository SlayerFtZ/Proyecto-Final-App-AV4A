using AppFinalAv4A.Core.Localization;

namespace AppFinalAv4A.Features.home.Domain.Entities;

public enum AlertSeverity
{
    Critical,
    Warning
}

public abstract class AlertItem
{
    private int _productId;
    private string _productName = string.Empty;

    protected AlertItem(int productId, string productName)
    {
        ProductId = productId;
        ProductName = productName;
    }

    public int ProductId
    {
        get => _productId;
        private set
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(ProductId), "El id del producto debe ser mayor a 0.");
            _productId = value;
        }
    }

    public string ProductName
    {
        get => _productName;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("El nombre del producto es obligatorio.", nameof(ProductName));
            _productName = value.Trim();
        }
    }


    public abstract AlertSeverity Severity { get; }

 
    public abstract string Icon { get; }


    public virtual int Priority => Severity == AlertSeverity.Critical ? 0 : 10;


    public virtual string SeverityLabel => Severity == AlertSeverity.Critical
        ? LocalizationService.Instance.GetString("Critical")
        : LocalizationService.Instance.GetString("Warning");


    public string Detail => BuildDetail(LocalizationService.Instance);

    protected abstract string BuildDetail(ILocalizationService loc);
}
