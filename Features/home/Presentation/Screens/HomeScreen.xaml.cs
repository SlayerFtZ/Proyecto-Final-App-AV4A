using AppFinalAv4A.Features.home.Presentation.ViewModels;

namespace AppFinalAv4A.Features.home.Presentation.Screens;

public partial class HomeScreen : ContentPage
{
    readonly HomeViewModel _viewModel;

    public HomeScreen(HomeViewModel viewModel)
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
