namespace Dreamine.UI.Maui.Popup;

/// <summary>
/// WPF Dreamine.UI.Abstractions.Popup.BlinkPopupOptions / WinForms
/// Dreamine.UI.WinForms.Popup.BlinkPopupOptions에 대응하는 MAUI 전용 옵션.
/// </summary>
public sealed class BlinkPopupOptions
{
    public string? Title { get; set; }
    public string? Message { get; set; }
    public string? OkText { get; set; }
    public string? CancelText { get; set; }
    public bool UseBlink { get; set; } = true;
    public Color Color1 { get; set; } = Colors.Red;
    public Color Color2 { get; set; } = Colors.DarkRed;
    public Color ForegroundColor { get; set; } = Colors.Yellow;
    public int BlinkIntervalMs { get; set; } = 600;
}
