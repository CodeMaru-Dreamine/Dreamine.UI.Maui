namespace Dreamine.UI.Maui.Popup;

/// <summary>
/// WPF Dreamine.UI.Wpf.Controls.MessageBox.DreamineMessageBox / WinForms
/// Dreamine.UI.WinForms.MessageBox.DreamineMessageBox와 동일한 역할을 하는 MAUI용 메시지박스.
/// </summary>
public static class DreamineMessageBox
{
    /// <summary>OK 버튼 하나만 있는 메시지박스를 표시한다.</summary>
    public static Task<DreamineDialogResult> ShowAsync(
        string message,
        string title = "Information",
        int autoClickDelaySeconds = 0)
        => ShowAsync(message, title,
            new[] { (DreamineDialogResult.OK, "확인") },
            DreamineDialogResult.OK, autoClickDelaySeconds);

    /// <summary>확인/취소 버튼이 있는 메시지박스를 표시한다.</summary>
    public static Task<DreamineDialogResult> ShowOkCancelAsync(
        string message,
        string title = "Confirm")
        => ShowAsync(message, title,
            new[] { (DreamineDialogResult.Cancel, "취소"), (DreamineDialogResult.OK, "확인") });

    /// <summary>예/아니오 버튼이 있는 메시지박스를 표시한다.</summary>
    public static Task<DreamineDialogResult> ShowYesNoAsync(
        string message,
        string title = "Question")
        => ShowAsync(message, title,
            new[] { (DreamineDialogResult.No, "아니오"), (DreamineDialogResult.Yes, "예") });

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
