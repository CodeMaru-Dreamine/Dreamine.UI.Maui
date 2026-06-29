namespace Dreamine.UI.Maui.Popup;

/// <summary>
/// DreamineBlinkPopup이 내부적으로 띄우는 모달 페이지.
/// WPF DreamineBlinkPopupWindow / WinForms BlinkPopupForm과 같은 역할이다.
/// </summary>
public partial class BlinkPopupPage : ContentPage
{
    private readonly TaskCompletionSource<DreamineDialogResult> _tcs = new();
    private readonly BlinkPopupOptions _options;
    private IDispatcherTimer? _blinkTimer;
    private bool _blinkPhase;

    public Task<DreamineDialogResult> ResultTask => _tcs.Task;

    public BlinkPopupPage(BlinkPopupOptions options)
    {
        InitializeComponent();
        _options = options;

        TitleLabel.Text = options.Title ?? string.Empty;
        TitleLabel.TextColor = options.ForegroundColor;
        MessageLabel.Text = options.Message ?? string.Empty;
        MessageLabel.TextColor = options.ForegroundColor;
        Card.BackgroundColor = options.Color1;

        if (!string.IsNullOrEmpty(options.CancelText))
            ButtonRow.Children.Add(MakeButton(options.CancelText, DreamineDialogResult.Cancel));
        if (!string.IsNullOrEmpty(options.OkText))
            ButtonRow.Children.Add(MakeButton(options.OkText, DreamineDialogResult.OK));

        if (options.UseBlink)
        {
            _blinkTimer = Dispatcher.CreateTimer();
            _blinkTimer.Interval = TimeSpan.FromMilliseconds(options.BlinkIntervalMs);
            _blinkTimer.Tick += (_, _) =>
            {
                _blinkPhase = !_blinkPhase;
                Card.BackgroundColor = _blinkPhase ? options.Color2 : options.Color1;
            };
            _blinkTimer.Start();
        }
    }

    private Button MakeButton(string text, DreamineDialogResult result)
    {
        var button = new Button
        {
            Text = text,
            BackgroundColor = Color.FromArgb("#0D1B3E"),
            TextColor = Colors.White,
            CornerRadius = 6,
            WidthRequest = 100,
            HeightRequest = 36
        };
        button.Clicked += (_, _) => Complete(result);
        return button;
    }

    private void Complete(DreamineDialogResult result)
    {
        _blinkTimer?.Stop();
        if (!_tcs.Task.IsCompleted)
            _tcs.SetResult(result);

        Navigation.PopModalAsync(animated: false);
    }
}
