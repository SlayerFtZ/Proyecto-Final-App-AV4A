using AppFinalAv4A.Core.Localization;
using AppFinalAv4A.Core.Network;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AppFinalAv4A.Core.Presentation;

public abstract partial class BaseViewModel : ObservableObject
{
    protected BaseViewModel(ILocalizationService localization)
    {
        Localization = localization;
    }

    protected ILocalizationService Localization { get; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool isBusy;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    private string? errorMessage;

    public bool IsNotBusy => !IsBusy;

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    protected abstract string DefaultErrorMessage { get; }

    protected virtual string ResolveErrorMessage(Exception ex) => ex switch
    {
        ApiException api => api.Message,
        _ => DefaultErrorMessage
    };

    protected string T(string text) => Localization.GetString(text);
}
