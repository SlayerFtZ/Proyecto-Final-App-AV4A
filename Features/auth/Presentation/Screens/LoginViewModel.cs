using AppFinalAv4A.Features.auth.Domain.Repositories;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AppFinalAv4A.Features.auth.Presentation.screens;

public partial class LoginViewModel : ObservableObject
{
    readonly IAuthRepository _repository;

    public LoginViewModel(IAuthRepository repository) => _repository = repository;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    bool isBusy;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    string? errorMessage;

    public bool IsNotBusy => !IsBusy;
    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    [RelayCommand]
    async Task InitializeAsync()
    {
        var session = await _repository.GetCurrentSessionAsync();
        if (session is not null)
            await Shell.Current.GoToAsync("//ProfileScreen");
    }

    [RelayCommand]
    async Task SignInAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;
            await _repository.SingInWithGoogleAsync();
            await Shell.Current.GoToAsync("//ProfileScreen");
        }
        catch (TaskCanceledException)
        {
    
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }
}