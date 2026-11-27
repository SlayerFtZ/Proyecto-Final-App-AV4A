using CommunityToolkit.Mvvm.ComponentModel;
namespace AppFinalAv4A.Features.Language.Screens;
public partial class LanguageOption : ObservableObject
{
    public string Code { get; init; } = "";    
    public string Name { get; init; } = "";
    public string Flag { get; init; } = "";     
    [ObservableProperty] private bool isSelected;
}