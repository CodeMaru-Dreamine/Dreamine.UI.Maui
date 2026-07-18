namespace Dreamine.UI.Maui.Popup;

/// <summary>
/// \if KO
/// <para>메시지, 선택 버튼 및 선택적 자동 선택 카운트다운을 렌더링하는 내부 MAUI 모달 페이지입니다.</para>
/// \endif
/// \if EN
/// <para>Provides the internal MAUI modal page that renders a message, choice buttons, and an optional automatic-selection countdown.</para>
/// \endif
/// </summary>
/// <remarks>
/// \if KO
/// <para>별도의 운영체제 창 대신 반투명 배경의 모달 페이지로 팝업 동작을 제공합니다.</para>
/// \endif
/// \if EN
/// <para>Uses a modal page with a translucent background to provide popup behavior instead of creating a separate operating-system window.</para>
/// \endif
/// </remarks>
public partial class DreamineMessageBoxPage : ContentPage
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
    /// <para>auto Click Timer 값을 보관합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Stores the auto click timer value.</para>
    /// \endif
    /// </summary>
    private IDispatcherTimer? _autoClickTimer;
    /// <summary>
    /// \if KO
    /// <para>auto Click Remaining Seconds 값을 보관합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Stores the auto click remaining seconds value.</para>
    /// \endif
    /// </summary>
    private int _autoClickRemainingSeconds;
    /// <summary>
    /// \if KO
    /// <para>auto Click Result 값을 보관합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Stores the auto click result value.</para>
    /// \endif
    /// </summary>
    private DreamineDialogResult _autoClickResult;
    /// <summary>
    /// \if KO
    /// <para>base Message 값을 보관합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Stores the base message value.</para>
    /// \endif
    /// </summary>
    private string _baseMessage = string.Empty;

    /// <summary>
    /// \if KO
    /// <para>사용자가 메시지 상자를 완료할 때 최종 결과를 생성하는 작업을 가져옵니다.</para>
    /// \endif
    /// \if EN
    /// <para>Gets the task that produces the final result when the user completes the message box.</para>
    /// \endif
    /// </summary>
    public Task<DreamineDialogResult> ResultTask => _tcs.Task;

    /// <summary>
    /// \if KO
    /// <para>지정한 콘텐츠, 버튼 및 자동 선택 설정으로 메시지 상자 페이지를 초기화합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Initializes the message-box page with the specified content, buttons, and automatic-selection settings.</para>
    /// \endif
    /// </summary>
    /// <param name="title">
    /// \if KO
    /// <para>메시지 상자 제목입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The message-box title.</para>
    /// \endif
    /// </param>
    /// <param name="message">
    /// \if KO
    /// <para>표시할 메시지입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The message to display.</para>
    /// \endif
    /// </param>
    /// <param name="buttons">
    /// \if KO
    /// <para>결과와 표시 텍스트로 구성된 버튼 목록입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The button list composed of results and display text.</para>
    /// \endif
    /// </param>
    /// <param name="autoClick">
    /// \if KO
    /// <para>카운트다운 만료 시 자동 선택할 결과입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The result selected automatically when the countdown expires.</para>
    /// \endif
    /// </param>
    /// <param name="autoClickDelaySeconds">
    /// \if KO
    /// <para>자동 선택 전 대기할 초입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The number of seconds before automatic selection.</para>
    /// \endif
    /// </param>
    /// <exception cref="NullReferenceException">
    /// \if KO
    /// <para><paramref name="buttons"/>가 <see langword="null"/>일 때 발생합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Thrown when <paramref name="buttons"/> is <see langword="null"/>.</para>
    /// \endif
    /// </exception>
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

    /// <summary>
    /// \if KO
    /// <para>기본 메시지에 자동 선택까지 남은 시간을 추가하여 표시합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Displays the base message with the remaining time before automatic selection.</para>
    /// \endif
    /// </summary>
    private void UpdateCountdownText()
        => MessageLabel.Text = $"{_baseMessage}\n\n({_autoClickRemainingSeconds}초 후 자동으로 닫힙니다)";

    /// <summary>
    /// \if KO
    /// <para>자동 선택 타이머를 중지하고 결과 작업을 완료한 뒤 모달 페이지를 닫습니다.</para>
    /// \endif
    /// \if EN
    /// <para>Stops the automatic-selection timer, completes the result task, and closes the modal page.</para>
    /// \endif
    /// </summary>
    /// <param name="result">
    /// \if KO
    /// <para>메시지 상자의 최종 결과입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The final result of the message box.</para>
    /// \endif
    /// </param>
    private void Complete(DreamineDialogResult result)
    {
        _autoClickTimer?.Stop();
        if (!_tcs.Task.IsCompleted)
            _tcs.SetResult(result);

        Navigation.PopModalAsync(animated: false);
    }
}
