using System.Globalization;
using AppFinalAv4A.Features.home.Domain.Entities;

namespace AppFinalAv4A.Features.home.Presentation.Converters;


public class SeverityToColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var isBackground = string.Equals(parameter as string, "bg", StringComparison.OrdinalIgnoreCase);

        var key = value switch
        {
            AlertSeverity.Critical => isBackground ? "CriticalBackground" : "Critical",
            AlertSeverity.Warning => isBackground ? "WarningBackground" : "WarningText",
            _ => isBackground ? "Surface" : "Primary"
        };

        return Application.Current?.Resources.TryGetValue(key, out var color) == true
            ? color
            : Colors.Gray;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
