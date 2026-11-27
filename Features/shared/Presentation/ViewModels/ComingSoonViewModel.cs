using AppFinalAv4A.Core.Localization;
using AppFinalAv4A.Core.Ui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AppFinalAv4A.Features.Shared.Presentation.ViewModels;

/// <summary>
/// Pantalla temporal para secciones en construcción.
/// Recibe el parámetro de navegación "feature" (Shell: ComingSoonScreen?feature=scanner).
/// </summary>
public partial class ComingSoonViewModel : ObservableObject, IQueryAttributable
{
    readonly ILocalizationService _loc;

    public ComingSoonViewModel(ILocalizationService loc) => _loc = loc;

    [ObservableProperty] private string title = string.Empty;
    [ObservableProperty] private string icon = Icons.Construction;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var feature = query.TryGetValue("feature", out var value) ? value?.ToString() : null;

        (Title, Icon) = feature switch
        {
            "scanner" => (_loc.GetString("Scan"), Icons.Scan),
            "reports" => (_loc.GetString("Reports"), Icons.Reports),
            "products" => (_loc.GetString("Products"), Icons.Products),
            "sales" => (_loc.GetString("Sales"), Icons.Sales),
            _ => (_loc.GetString("Coming soon"), Icons.Construction)
        };
    }

    [RelayCommand]
    private Task GoBackAsync() => Shell.Current.GoToAsync("..");
}
