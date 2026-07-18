namespace Dreamine.UI.Maui.Popup;

/// <summary>
/// \if KO
/// <para>깜빡임 팝업의 콘텐츠와 애니메이션을 렌더링하는 내부 MAUI 모달 페이지입니다.</para>
/// \endif
/// \if EN
/// <para>Provides the internal MAUI modal page that renders blinking-popup content and animation.</para>
/// \endif
/// </summary>
public partial class BlinkPopupPage : ContentPage
{
    /// <summary>
    /// \if KO
    /// <para>tcs 값을 보관합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Stores the tcs value.</para>
    /// \endif
    /// </summary>
    private readonly TaskCompletionSource<DreamineDialogResult> _tcs = new();
    /// <summary>
    /// \if KO
    /// <para>options 값을 보관합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Stores the options value.</para>
    /// \endif
    /// </summary>
    private readonly BlinkPopupOptions _options;
    /// <summary>
    /// \if KO
    /// <para>blink Timer 값을 보관합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Stores the blink timer value.</para>
    /// \endif
    /// </summary>
    private IDispatcherTimer? _blinkTimer;
    /// <summary>
    /// \if KO
    /// <para>blink Phase 값을 보관합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Stores the blink phase value.</para>
    /// \endif
    /// </summary>
    private bool _blinkPhase;

    /// <summary>
    /// \if KO
    /// <para>사용자가 팝업을 완료할 때 최종 결과를 생성하는 작업을 가져옵니다.</para>
    /// \endif
    /// \if EN
    /// <para>Gets the task that produces the final result when the user completes the popup.</para>
    /// \endif
    /// </summary>
    public Task<DreamineDialogResult> ResultTask => _tcs.Task;

    /// <summary>
    /// \if KO
    /// <para>지정한 옵션으로 팝업 콘텐츠, 버튼 및 선택적 깜빡임 타이머를 초기화합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Initializes popup content, buttons, and the optional blink timer using the specified options.</para>
    /// \endif
    /// </summary>
    /// <param name="options">
    /// \if KO
    /// <para>팝업 콘텐츠와 표시 옵션입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The popup content and display options.</para>
    /// \endif
    /// </param>
    /// <exception cref="NullReferenceException">
    /// \if KO
    /// <para><paramref name="options"/>가 <see langword="null"/>일 때 발생합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Thrown when <paramref name="options"/> is <see langword="null"/>.</para>
    /// \endif
    /// </exception>
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

    /// <summary>
    /// \if KO
    /// <para>지정한 텍스트와 결과를 갖고 클릭 시 팝업을 완료하는 버튼을 만듭니다.</para>
    /// \endif
    /// \if EN
    /// <para>Creates a button with the specified text and result that completes the popup when clicked.</para>
    /// \endif
    /// </summary>
    /// <param name="text">
    /// \if KO
    /// <para>버튼에 표시할 텍스트입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The text displayed on the button.</para>
    /// \endif
    /// </param>
    /// <param name="result">
    /// \if KO
    /// <para>버튼을 클릭할 때 반환할 결과입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The result returned when the button is clicked.</para>
    /// \endif
    /// </param>
    /// <returns>
    /// \if KO
    /// <para>구성된 MAUI 버튼입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The configured MAUI button.</para>
    /// \endif
    /// </returns>
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

    /// <summary>
    /// \if KO
    /// <para>깜빡임 타이머를 중지하고 결과 작업을 완료한 뒤 모달 페이지를 닫습니다.</para>
    /// \endif
    /// \if EN
    /// <para>Stops the blink timer, completes the result task, and closes the modal page.</para>
    /// \endif
    /// </summary>
    /// <param name="result">
    /// \if KO
    /// <para>팝업의 최종 결과입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The final result of the popup.</para>
    /// \endif
    /// </param>
    private void Complete(DreamineDialogResult result)
    {
        _blinkTimer?.Stop();
        if (!_tcs.Task.IsCompleted)
            _tcs.SetResult(result);

        Navigation.PopModalAsync(animated: false);
    }
}
