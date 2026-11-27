namespace AppFinalAv4A.Features.auth.Presentation.Screens;

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

    if (BindingContext is ProfileViewModel vm)
    {
        vm.RefreshLanguage();
        await vm.LoadCommand.ExecuteAsync(null);
    }
}
}