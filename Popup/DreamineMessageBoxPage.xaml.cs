namespace Dreamine.UI.Maui.Popup;

/// <summary>
/// DreamineMessageBox가 내부적으로 띄우는 모달 페이지.
/// WPF DreamineMessageBoxWindow / WinForms DreamineMessageBoxForm과 같은 역할이다.
/// MAUI는 여러 개의 OS 창을 즉석에서 만들기 어려워서, 모달 페이지 + 반투명 배경으로
/// "팝업"을 흉내낸다.
/// </summary>
public partial class DreamineMessageBoxPage : ContentPage
{
    private readonly TaskCompletionSource<DreamineDialogResult> _tcs = new();
    private IDispatcherTimer? _autoClickTimer;
    private int _autoClickRemainingSeconds;
    private DreamineDialogResult _autoClickResult;
    private string _baseMessage = string.Empty;

    public Task<DreamineDialogResult> ResultTask => _tcs.Task;

    public DreamineMessageBoxPage(
        string title,
        string message,
        IReadOnlyList<(DreamineDialogResult Result, string Text)> buttons,
        DreamineDialogResult autoClick = DreamineDialogResult.None,
        int autoClickDelaySeconds = 0)
    {
        InitializeComponent();

        TitleLabel.Text = title;
        MessageLabel.Text = message;
        _autoClickResult = autoClick;

        foreach (var (result, text) in buttons)
        {
            var button = new Button
            {
                Text = text,
                BackgroundColor = Color.FromArgb("#0D1B3E"),
                TextColor = Colors.White,
                CornerRadius = 6,
                WidthRequest = 90,
                HeightRequest = 36
            };
            button.Clicked += (_, _) => Complete(result);
            ButtonRow.Children.Add(button);
        }

        if (autoClickDelaySeconds > 0 && autoClick != DreamineDialogResult.None)
        {
            _autoClickRemainingSeconds = autoClickDelaySeconds;
            _baseMessage = message;
            UpdateCountdownText();

            _autoClickTimer = Dispatcher.CreateTimer();
            _autoClickTimer.Interval = TimeSpan.FromSeconds(1);
            _autoClickTimer.Tick += (_, _) =>
            {
                _autoClickRemainingSeconds--;
                if (_autoClickRemainingSeconds <= 0)
                {
                    _autoClickTimer?.Stop();
                    Complete(_autoClickResult);
                }
                else
                {
                    UpdateCountdownText();
                }
            };
            _autoClickTimer.Start();
        }
    }

    private void UpdateCountdownText()
        => MessageLabel.Text = $"{_baseMessage}\n\n({_autoClickRemainingSeconds}초 후 자동으로 닫힙니다)";

    private void Complete(DreamineDialogResult result)
    {
        _autoClickTimer?.Stop();
        if (!_tcs.Task.IsCompleted)
            _tcs.SetResult(result);

        Navigation.PopModalAsync(animated: false);
    }
}
