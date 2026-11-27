namespace AppFinalAv4A.Features.auth.Presentation.Screens;

public partial class LoginScreen : ContentPage
{
    readonly LoginViewModel _viewModel;

    public LoginScreen(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeCommand.ExecuteAsync(null);
    }
}