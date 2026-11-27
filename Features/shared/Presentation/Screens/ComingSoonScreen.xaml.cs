using AppFinalAv4A.Features.Shared.Presentation.ViewModels;

namespace AppFinalAv4A.Features.Shared.Presentation.Screens;

public partial class ComingSoonScreen : ContentPage
{
    public ComingSoonScreen(ComingSoonViewModel viewModel)
    {
        InitializeComponent();
        // El ViewModel implementa IQueryAttributable: Shell le entrega los parámetros de la ruta.
        BindingContext = viewModel;
    }
}
