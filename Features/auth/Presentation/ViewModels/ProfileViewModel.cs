
using AppFinalAv4A.Core.Localization;
using AppFinalAv4A.Features.auth.Domain.Repositories;
using AppFinalAv4A.Features.Language.Screens;
using AppFinalAv4A.Features.Shared.Presentation.Dialogs.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AppFinalAv4A.Features.auth.Presentation.Screens;

public partial class ProfileViewModel : ObservableObject
{
    private readonly IAuthRepository _repository;
    private readonly ILocalizationService _loc;
    private readonly IAppDialogService _dialogService;

    public ProfileViewModel(
        IAuthRepository repository,
        ILocalizationService loc,
        IAppDialogService dialogService)
    {
        _repository = repository;
        _loc = loc;
        _dialogService = dialogService;

        RefreshLanguage();
    }

    [ObservableProperty] private string? name;
    [ObservableProperty] private string? email;
    [ObservableProperty] private string? pictureUrl;
    [ObservableProperty] private string currentLanguageName = "";

    [RelayCommand]
    async Task LoadAsync()
    {
        var user = await _repository.GetCurrentSessionAsync();

        if (user is null)
        {
            await Shell.Current.GoToAsync("//LoginScreen");
            return;
        }

        Name = user.Name;
        Email = user.Email;
        PictureUrl = user.PictureProfileUrl;
    }

    [RelayCommand]
    async Task SignOutAsync()
    {
    
        var confirmed = await _dialogService.ConfirmLogoutAsync();

        if (!confirmed)
            return;

        try
        {
          
            await _repository.SingOutAsync();

          
            await _dialogService.ShowSuccessAsync(
                "Tu sesión se ha cerrado correctamente.",
                "Hasta pronto"
            );

            
            await Shell.Current.GoToAsync("//LoginScreen");
        }
        catch (Exception)
        {
            await _dialogService.ShowErrorAsync(
                "No fue posible cerrar tu sesión. Inténtalo nuevamente."
            );
        }
    }

    [RelayCommand]
    async Task OpenLanguage()
    {
        await Shell.Current.GoToAsync(nameof(LanguageScreen));
    }

    public async Task NotifyLanguageUpdatedAsync()
    {
        RefreshLanguage();

        await _dialogService.ShowLanguageUpdatedAsync(
            CurrentLanguageName
        );
    }

    public void RefreshLanguage()
    {
        CurrentLanguageName =
            _loc.CurrentCulture.TwoLetterISOLanguageName == "en"
                ? "English"
                : "Español";
    }
}
