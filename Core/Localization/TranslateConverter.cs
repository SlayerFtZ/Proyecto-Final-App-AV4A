using System.Globalization;

namespace AppFinalAv4A.Core.Localization;

internal sealed class TranslateConverter : IValueConverter
{
    public static TranslateConverter Instance { get; } = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => LocalizationService.Instance.GetString(parameter as string ?? string.Empty);

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}