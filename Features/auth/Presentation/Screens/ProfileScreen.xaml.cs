namespace AppFinalAv4A.Features.auth.Presentation.screens;

public partial class ProfileScreen : ContentPage
{
    readonly ProfileViewModel _viewModel;

    public ProfileScreen(ProfileViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadCommand.ExecuteAsync(null);
    }
}