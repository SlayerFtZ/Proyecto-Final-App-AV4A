namespace AppFinalAv4A.Features.Language.Screens;

public partial class LanguageScreen : ContentPage
{
    public LanguageScreen(LanguageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}