namespace Dreamine.UI.Maui.Popup;

/// <summary>
/// \if KO
/// <para>MAUI 깜빡임 팝업의 콘텐츠, 색상 및 애니메이션을 구성합니다.</para>
/// \endif
/// \if EN
/// <para>Configures the content, colors, and animation of a MAUI blinking popup.</para>
/// \endif
/// </summary>
public sealed class BlinkPopupOptions
{
    /// <summary>
    /// \if KO
    /// <para>팝업 제목을 가져오거나 설정합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Gets or sets the popup title.</para>
    /// \endif
    /// </summary>
    public string? Title { get; set; }
    /// <summary>
    /// \if KO
    /// <para>팝업 메시지를 가져오거나 설정합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Gets or sets the popup message.</para>
    /// \endif
    /// </summary>
    public string? Message { get; set; }
    /// <summary>
    /// \if KO
    /// <para>확인 버튼 텍스트를 가져오거나 설정합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Gets or sets the OK button text.</para>
    /// \endif
    /// </summary>
    public string? OkText { get; set; }
    /// <summary>
    /// \if KO
    /// <para>취소 버튼 텍스트를 가져오거나 설정합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Gets or sets the Cancel button text.</para>
    /// \endif
    /// </summary>
    public string? CancelText { get; set; }
    /// <summary>
    /// \if KO
    /// <para>배경 깜빡임 효과를 사용할지 여부를 가져오거나 설정합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Gets or sets whether the background blinking effect is enabled.</para>
    /// \endif
    /// </summary>
    public bool UseBlink { get; set; } = true;
    /// <summary>
    /// \if KO
    /// <para>깜빡임 애니메이션의 첫 번째 배경색을 가져오거나 설정합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Gets or sets the first background color of the blinking animation.</para>
    /// \endif
    /// </summary>
    public Color Color1 { get; set; } = Colors.Red;
    /// <summary>
    /// \if KO
    /// <para>깜빡임 애니메이션의 두 번째 배경색을 가져오거나 설정합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Gets or sets the second background color of the blinking animation.</para>
    /// \endif
    /// </summary>
    public Color Color2 { get; set; } = Colors.DarkRed;
    /// <summary>
    /// \if KO
    /// <para>팝업 콘텐츠의 전경색을 가져오거나 설정합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Gets or sets the foreground color of the popup content.</para>
    /// \endif
    /// </summary>
    public Color ForegroundColor { get; set; } = Colors.Yellow;
    /// <summary>
    /// \if KO
    /// <para>한 방향 깜빡임 전환 간격을 밀리초 단위로 가져오거나 설정합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Gets or sets the one-way blink transition interval in milliseconds.</para>
    /// \endif
    /// </summary>
    public int BlinkIntervalMs { get; set; } = 600;
}
