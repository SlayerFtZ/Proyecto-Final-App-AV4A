
namespace AppFinalAv4A.Features.Shared.Presentation.Popups;

public partial class AlertPopup : ContentView
{
    private readonly Action _onClose;

    public AlertPopup(
        string title,
        string message,
        string icon,
        Action onClose)
    {
        InitializeComponent();

        _onClose = onClose;

        TitleLabel.Text = title;
        MessageLabel.Text = message;
        IconLabel.Text = icon;
    }

    private void OnCloseClicked(object sender, EventArgs e)
    {
        _onClose();
    }
}
