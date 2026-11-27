using AppFinalAv4A.Features.Language.Screens;
using AppFinalAv4A.Features.Shared.Presentation.Screens;

namespace AppFinalAv4A;

public partial class AppShell : Shell
{
	public AppShell()
{
    InitializeComponent();
    Routing.RegisterRoute(nameof(LanguageScreen), typeof(LanguageScreen));
    // Pantalla con parámetro de navegación: ComingSoonScreen?feature=...
    Routing.RegisterRoute(nameof(ComingSoonScreen), typeof(ComingSoonScreen));
}
}
