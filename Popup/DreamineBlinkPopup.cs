namespace Dreamine.UI.Maui.Popup;

/// <summary>
/// WPF IPopupService.ShowBlinkAsync / WinForms DreamineBlinkPopup과 동일한 역할을 하는
/// MAUI용 깜빡임 팝업 서비스.
/// </summary>
public static class DreamineBlinkPopup
{
    public static async Task<DreamineDialogResult> ShowAsync(BlinkPopupOptions options)
    {
        var page = new BlinkPopupPage(options);
        var navigation = Application.Current!.Windows[0].Page!.Navigation;
        await navigation.PushModalAsync(page, animated: false);
        return await page.ResultTask;
    }
}
