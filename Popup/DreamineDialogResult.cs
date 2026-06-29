namespace Dreamine.UI.Maui.Popup;

/// <summary>
/// WPF의 MessageBoxResult / WinForms의 DialogResult와 동일한 개념의 MAUI용 결과값.
/// 플랫폼마다 이름이 다른 걸 통일해서 샘플 코드를 비교하기 쉽게 한다.
/// </summary>
public enum DreamineDialogResult
{
    None,
    OK,
    Cancel,
    Yes,
    No
}
