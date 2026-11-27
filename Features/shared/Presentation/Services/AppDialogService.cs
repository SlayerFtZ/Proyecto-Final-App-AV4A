
using FastPopups;
using Microsoft.Maui.ApplicationModel;
using AppFinalAv4A.Features.Shared.Presentation.Dialogs.Interfaces;
using AppFinalAv4A.Features.Shared.Presentation.Popups;
using AppoMobi.Maui.FastPopups;

namespace AppFinalAv4A.Features.Shared.Presentation.Dialogs.Services;

public sealed class AppDialogService : IAppDialogService
{
    private const double PopupWidth = 340;

    private static Page? GetCurrentPage()
    {
        return Shell.Current
            ?? Application.Current?.Windows.FirstOrDefault()?.Page;
    }

    public Task ShowSuccessAsync(
        string message,
        string title = "¡Operación exitosa!")
    {
        return ShowAlertAsync(
            title,
            message,
            "✓",
            PopupAnimationType.BounceIn);
    }

    public Task ShowErrorAsync(
        string message,
        string title = "Ocurrió un error")
    {
        return ShowAlertAsync(
            title,
            message,
            "✕",
            PopupAnimationType.ZoomIn);
    }

    public Task ShowLanguageUpdatedAsync(string languageName)
    {
        return ShowAlertAsync(
            "Idioma actualizado",
            $"El idioma se cambió a {languageName}.",
            "🌐",
            PopupAnimationType.SprintBottom);
    }

    private async Task ShowAlertAsync(
        string title,
        string message,
        string icon,
        PopupAnimationType animation)
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            var page = GetCurrentPage();

            if (page is null)
                return;

            Popup? popup = null;

            var content = new AlertPopup(
                title,
                message,
                icon,
                () => popup?.Close());

            popup = new Popup
            {
                Content = content,
                WidthRequest = PopupWidth,
                HeightRequest = 280,

                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,

                BackgroundColor = Color.FromArgb("#66000000"),

                CloseWhenBackgroundIsClicked = true,

                AnimationType = animation,
                AnimationDuration = 350,
                AnimationEasing = PopupAnimationEasing.Decelerate
            };

            await page.ShowPopupAsync(popup);
        });
    }

    public async Task<bool> ConfirmLogoutAsync()
    {
        var confirmed = false;

        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            var page = GetCurrentPage();

            if (page is null)
                return;

            Popup? popup = null;

            var content = new ConfirmPopup(
                "¿Cerrar sesión?",
                "¿Seguro que quieres cerrar sesión?",
                () => popup?.Close(true),
                () => popup?.Close(false));

            popup = new Popup
            {
                Content = content,
                WidthRequest = PopupWidth,
                HeightRequest = 300,

                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,

                BackgroundColor = Color.FromArgb("#66000000"),

                CloseWhenBackgroundIsClicked = false,

                AnimationType = PopupAnimationType.BounceIn,
                AnimationDuration = 350,
                AnimationEasing = PopupAnimationEasing.Decelerate
            };

            var result = await page.ShowPopupAsync(popup);

            confirmed = result is bool value && value;
        });

        return confirmed;
    }
}
