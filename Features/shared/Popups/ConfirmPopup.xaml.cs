
namespace AppFinalAv4A.Features.Shared.Presentation.Popups;

public partial class ConfirmPopup : ContentView
{
    private readonly Action _onConfirm;
    private readonly Action _onCancel;

    public ConfirmPopup(
        string title,
        string message,
        Action onConfirm,
        Action onCancel)
    {
        InitializeComponent();

        _onConfirm = onConfirm;
        _onCancel = onCancel;

        TitleLabel.Text = title;
        MessageLabel.Text = message;
    }

    private void OnConfirmClicked(object sender, EventArgs e)
    {
        _onConfirm();
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        _onCancel();
    }
}
