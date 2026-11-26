using AppFinalAv4A.Features.auth.Domain.Entities;
using AppFinalAv4A.Features.auth.Domain.Repositories;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AppFinalAv4A.Features.auth.Presentation.screens;

public partial class LoginViewModel : ObservableObject
{
    readonly IAuthRepository _repository;

    public LoginViewModel(IAuthRepository repository) => _repository = repository;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsLoggedIn))]
    [NotifyPropertyChangedFor(nameof(IsLoggedOut))]
    AuthGoogle? user;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    bool isBusy;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    string? errorMessage;

    public bool IsNotBusy => !IsBusy;
    public bool IsLoggedIn => User is not null;
    public bool IsLoggedOut => User is null;
    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    [RelayCommand]
    async Task InitializeAsync()
    {
        User = await _repository.GetCurrentSessionAsync();
    }

    [RelayCommand]
    async Task SignInAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;
            User = await _repository.SingInWithGoogleAsync();
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

    [RelayCommand]
    async Task SignOutAsync()
    {
        await _repository.SingOutAsync();
        User = null;
    }
}