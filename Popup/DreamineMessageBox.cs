namespace Dreamine.UI.Maui.Popup;

/// <summary>
/// \if KO
/// <para>MAUI 모달 페이지 기반 메시지 상자 진입점을 제공합니다.</para>
/// \endif
/// \if EN
/// <para>Provides entry points for MAUI modal-page-based message boxes.</para>
/// \endif
/// </summary>
public static class DreamineMessageBox
{
    /// <summary>
    /// \if KO
    /// <para>확인 버튼 하나가 있는 메시지 상자를 표시합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Displays a message box with a single OK button.</para>
    /// \endif
    /// </summary>
    /// <param name="message">
    /// \if KO
    /// <para>표시할 메시지입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The message to display.</para>
    /// \endif
    /// </param>
    /// <param name="title">
    /// \if KO
    /// <para>메시지 상자 제목입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The message-box title.</para>
    /// \endif
    /// </param>
    /// <param name="autoClickDelaySeconds">
    /// \if KO
    /// <para>확인을 자동 선택하기 전 대기할 초입니다. 0 이하는 자동 선택을 사용하지 않습니다.</para>
    /// \endif
    /// \if EN
    /// <para>The number of seconds before automatically selecting OK. A non-positive value disables automatic selection.</para>
    /// \endif
    /// </param>
    /// <returns>
    /// \if KO
    /// <para>최종 대화 상자 결과를 생성하는 작업입니다.</para>
    /// \endif
    /// \if EN
    /// <para>A task that produces the final dialog result.</para>
    /// \endif
    /// </returns>
    public static Task<DreamineDialogResult> ShowAsync(
        string message,
        string title = "Information",
        int autoClickDelaySeconds = 0)
        => ShowAsync(message, title,
            new[] { (DreamineDialogResult.OK, "확인") },
            DreamineDialogResult.OK, autoClickDelaySeconds);

    /// <summary>
    /// \if KO
    /// <para>확인 및 취소 버튼이 있는 메시지 상자를 표시합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Displays a message box with OK and Cancel buttons.</para>
    /// \endif
    /// </summary>
    /// <param name="message">
    /// \if KO
    /// <para>표시할 메시지입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The message to display.</para>
    /// \endif
    /// </param>
    /// <param name="title">
    /// \if KO
    /// <para>메시지 상자 제목입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The message-box title.</para>
    /// \endif
    /// </param>
    /// <returns>
    /// \if KO
    /// <para>최종 대화 상자 결과를 생성하는 작업입니다.</para>
    /// \endif
    /// \if EN
    /// <para>A task that produces the final dialog result.</para>
    /// \endif
    /// </returns>
    public static Task<DreamineDialogResult> ShowOkCancelAsync(
        string message,
        string title = "Confirm")
        => ShowAsync(message, title,
            new[] { (DreamineDialogResult.Cancel, "취소"), (DreamineDialogResult.OK, "확인") });

    /// <summary>
    /// \if KO
    /// <para>예 및 아니요 버튼이 있는 메시지 상자를 표시합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Displays a message box with Yes and No buttons.</para>
    /// \endif
    /// </summary>
    /// <param name="message">
    /// \if KO
    /// <para>표시할 질문입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The question to display.</para>
    /// \endif
    /// </param>
    /// <param name="title">
    /// \if KO
    /// <para>메시지 상자 제목입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The message-box title.</para>
    /// \endif
    /// </param>
    /// <returns>
    /// \if KO
    /// <para>최종 대화 상자 결과를 생성하는 작업입니다.</para>
    /// \endif
    /// \if EN
    /// <para>A task that produces the final dialog result.</para>
    /// \endif
    /// </returns>
    public static Task<DreamineDialogResult> ShowYesNoAsync(
        string message,
        string title = "Question")
        => ShowAsync(message, title,
            new[] { (DreamineDialogResult.No, "아니오"), (DreamineDialogResult.Yes, "예") });

    /// <summary>
    /// \if KO
    /// <para>지정한 버튼과 자동 선택 설정으로 모달 메시지 상자 페이지를 표시합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Displays a modal message-box page with the specified buttons and automatic-selection settings.</para>
    /// \endif
    /// </summary>
    /// <param name="message">
    /// \if KO
    /// <para>표시할 메시지입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The message to display.</para>
    /// \endif
    /// </param>
    /// <param name="title">
    /// \if KO
    /// <para>메시지 상자 제목입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The message-box title.</para>
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
    /// <returns>
    /// \if KO
    /// <para>최종 대화 상자 결과를 생성하는 작업입니다.</para>
    /// \endif
    /// \if EN
    /// <para>A task that produces the final dialog result.</para>
    /// \endif
    /// </returns>
    /// <exception cref="NullReferenceException">
    /// \if KO
    /// <para>현재 MAUI 애플리케이션, 창 또는 페이지를 사용할 수 없을 때 발생합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Thrown when the current MAUI application, window, or page is unavailable.</para>
    /// \endif
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// \if KO
    /// <para>현재 애플리케이션에 창이 없을 때 발생합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Thrown when the current application has no windows.</para>
    /// \endif
    /// </exception>
    private static async Task<DreamineDialogResult> ShowAsync(
        string message,
        string title,
        IReadOnlyList<(DreamineDialogResult Result, string Text)> buttons,
        DreamineDialogResult autoClick = DreamineDialogResult.None,
        int autoClickDelaySeconds = 0)
    {
        var page = new DreamineMessageBoxPage(title, message, buttons, autoClick, autoClickDelaySeconds);
        var navigation = Application.Current!.Windows[0].Page!.Navigation;
        await navigation.PushModalAsync(page, animated: false);
        return await page.ResultTask;
    }
}
