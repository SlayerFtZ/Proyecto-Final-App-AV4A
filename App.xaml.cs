using AppFinalAv4A.Core.Localization;
using Microsoft.Extensions.DependencyInjection;

namespace AppFinalAv4A;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

   protected override Window CreateWindow(IActivationState? activationState)
{
    Task.Run(() => LocalizationService.Instance.InitializeAsync()).GetAwaiter().GetResult();
    return new Window(new AppShell());
}
}