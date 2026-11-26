using AppFinalAv4A.Features.auth.Domain.Repositories;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AppFinalAv4A.Features.auth.Presentation.screens;

public partial class ProfileViewModel : ObservableObject
{
    readonly IAuthRepository _repository;

    public ProfileViewModel(IAuthRepository repository) => _repository = repository;

    [ObservableProperty] string? name;
    [ObservableProperty] string? email;
    [ObservableProperty] string? pictureUrl;

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
        await _repository.SingOutAsync();
        await Shell.Current.GoToAsync("//LoginScreen");
    }
}