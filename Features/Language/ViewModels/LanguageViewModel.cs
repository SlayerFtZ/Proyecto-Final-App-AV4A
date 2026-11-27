using System.Collections.ObjectModel;
using AppFinalAv4A.Core.Localization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
namespace AppFinalAv4A.Features.Language.Screens;
public partial class LanguageViewModel : ObservableObject
{
    private readonly ILocalizationService _loc;

    public ObservableCollection<LanguageOption> Languages { get; } = new()
{
    new() { Code = "es", Name = "Español", Flag = "es.svg" },
    new() { Code = "en", Name = "English", Flag = "en.svg" },
};

    public LanguageViewModel(ILocalizationService loc)
    {
        _loc = loc;
        var current = Preferences.Get("lang", _loc.CurrentCulture.TwoLetterISOLanguageName);
        foreach (var l in Languages) l.IsSelected = l.Code == current;
    }

    [RelayCommand]
    private async Task Select(LanguageOption option)
    {
        foreach (var l in Languages) l.IsSelected = l == option;
        await _loc.SetCultureAsync(option.Code);  
    }

   [RelayCommand]
private async Task Continue()
{
    var selected = Languages.First(l => l.IsSelected);
    Preferences.Set("lang", selected.Code);
    await _loc.SetCultureAsync(selected.Code);
    await Shell.Current.GoToAsync("..");
}
}