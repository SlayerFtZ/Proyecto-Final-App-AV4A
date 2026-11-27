
namespace AppFinalAv4A.Features.Shared.Presentation.Dialogs.Interfaces;

public interface IAppDialogService
{
    Task ShowSuccessAsync(
        string message,
        string title = "¡Operación exitosa!");

    Task ShowErrorAsync(
        string message,
        string title = "Ocurrió un error");

    Task<bool> ConfirmLogoutAsync();

    Task ShowLanguageUpdatedAsync(string languageName);
}
