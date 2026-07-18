namespace Dreamine.UI.Maui.Popup;

/// <summary>
/// \if KO
/// <para>MAUI 모달 페이지를 사용하여 깜빡임 팝업을 표시합니다.</para>
/// \endif
/// \if EN
/// <para>Displays blinking popups using MAUI modal pages.</para>
/// \endif
/// </summary>
public static class DreamineBlinkPopup
{
    /// <summary>
    /// \if KO
    /// <para>지정한 옵션으로 깜빡임 모달 페이지를 표시하고 사용자의 결과를 기다립니다.</para>
    /// \endif
    /// \if EN
    /// <para>Displays a blinking modal page using the specified options and waits for the user's result.</para>
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
    /// <returns>
    /// \if KO
    /// <para>팝업의 최종 대화 상자 결과를 생성하는 작업입니다.</para>
    /// \endif
    /// \if EN
    /// <para>A task that produces the popup's final dialog result.</para>
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
    public static async Task<DreamineDialogResult> ShowAsync(BlinkPopupOptions options)
    {
        var page = new BlinkPopupPage(options);
        var navigation = Application.Current!.Windows[0].Page!.Navigation;
        await navigation.PushModalAsync(page, animated: false);
        return await page.ResultTask;
    }
}
