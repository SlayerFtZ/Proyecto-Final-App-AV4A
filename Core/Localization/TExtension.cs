using Microsoft.Maui.Controls.Xaml;

namespace AppFinalAv4A.Core.Localization;

[ContentProperty(nameof(Text))]
public class TExtension : IMarkupExtension<BindingBase>
{
    public string Text { get; set; } = string.Empty;

    public BindingBase ProvideValue(IServiceProvider serviceProvider)
        => new Binding
        {
            Source = LocalizationService.Instance,
            Path = nameof(ILocalizationService.CurrentCulture),
            Mode = BindingMode.OneWay,
            Converter = TranslateConverter.Instance,
            ConverterParameter = Text
        };

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        => ProvideValue(serviceProvider);
}