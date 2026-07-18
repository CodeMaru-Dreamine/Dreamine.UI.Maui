namespace Dreamine.UI.Maui;

/// <summary>
/// \if KO
/// <para>체크 LED를 배치할 네 모서리 앵커를 지정합니다.</para>
/// \endif
/// \if EN
/// <para>Specifies one of four corner anchors for positioning a check LED.</para>
/// \endif
/// </summary>
public enum DreamineCheckLedCorner
{
    /// <summary>
    /// \if KO
    /// <para>왼쪽 위 모서리에 배치합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Positions the LED at the top-left corner.</para>
    /// \endif
    /// </summary>
    TopLeft,
    /// <summary>
    /// \if KO
    /// <para>오른쪽 위 모서리에 배치합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Positions the LED at the top-right corner.</para>
    /// \endif
    /// </summary>
    TopRight,
    /// <summary>
    /// \if KO
    /// <para>왼쪽 아래 모서리에 배치합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Positions the LED at the bottom-left corner.</para>
    /// \endif
    /// </summary>
    BottomLeft,
    /// <summary>
    /// \if KO
    /// <para>오른쪽 아래 모서리에 배치합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Positions the LED at the bottom-right corner.</para>
    /// \endif
    /// </summary>
    BottomRight
}

/// <summary>
/// \if KO
/// <para>켜짐, 맥동, 크기 및 모서리 배치를 지원하는 MAUI LED 표시 컨트롤입니다.</para>
/// \endif
/// \if EN
/// <para>Provides a MAUI LED indicator with on, pulse, size, and corner-placement support.</para>
/// \endif
/// </summary>
public partial class DreamineCheckLed : ContentView
{
    /// <summary>
    /// \if KO
    /// <para><see cref="IsOn"/> 바인딩 가능 속성을 식별합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Identifies the <see cref="IsOn"/> bindable property.</para>
    /// \endif
    /// </summary>
    public static readonly BindableProperty IsOnProperty = BindableProperty.Create(
        nameof(IsOn), typeof(bool), typeof(DreamineCheckLed), false, propertyChanged: OnVisualChanged);

    /// <summary>
    /// \if KO
    /// <para><see cref="IsPulse"/> 바인딩 가능 속성을 식별합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Identifies the <see cref="IsPulse"/> bindable property.</para>
    /// \endif
    /// </summary>
    public static readonly BindableProperty IsPulseProperty = BindableProperty.Create(
        nameof(IsPulse), typeof(bool), typeof(DreamineCheckLed), false, propertyChanged: OnVisualChanged);

    /// <summary>
    /// \if KO
    /// <para><see cref="Diameter"/> 바인딩 가능 속성을 식별합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Identifies the <see cref="Diameter"/> bindable property.</para>
    /// \endif
    /// </summary>
    public static readonly BindableProperty DiameterProperty = BindableProperty.Create(
        nameof(Diameter), typeof(double), typeof(DreamineCheckLed), 24d, propertyChanged: OnDiameterChanged);

    /// <summary>
    /// \if KO
    /// <para><see cref="Corner"/> 바인딩 가능 속성을 식별합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Identifies the <see cref="Corner"/> bindable property.</para>
    /// \endif
    /// </summary>
    public static readonly BindableProperty CornerProperty = BindableProperty.Create(
        nameof(Corner), typeof(DreamineCheckLedCorner), typeof(DreamineCheckLed), DreamineCheckLedCorner.TopLeft,
        propertyChanged: OnCornerChanged);

    /// <summary>
    /// \if KO
    /// <para>LED가 켜져 있는지 여부를 가져오거나 설정합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Gets or sets whether the LED is on.</para>
    /// \endif
    /// </summary>
    public bool IsOn
    {
        get => (bool)GetValue(IsOnProperty);
        set => SetValue(IsOnProperty, value);
    }

    /// <summary>
    /// \if KO
    /// <para>켜진 LED에 맥동 애니메이션을 적용할지 여부를 가져오거나 설정합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Gets or sets whether a pulse animation is applied while the LED is on.</para>
    /// \endif
    /// </summary>
    public bool IsPulse
    {
        get => (bool)GetValue(IsPulseProperty);
        set => SetValue(IsPulseProperty, value);
    }

    /// <summary>
    /// \if KO
    /// <para>LED 지름을 장치 독립 단위로 가져오거나 설정합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Gets or sets the LED diameter in device-independent units.</para>
    /// \endif
    /// </summary>
    public double Diameter
    {
        get => (double)GetValue(DiameterProperty);
        set => SetValue(DiameterProperty, value);
    }

    /// <summary>
    /// \if KO
    /// <para>부모 영역에서 LED를 고정할 모서리를 가져오거나 설정합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Gets or sets the corner to which the LED is anchored within its parent.</para>
    /// \endif
    /// </summary>
    public DreamineCheckLedCorner Corner
    {
        get => (DreamineCheckLedCorner)GetValue(CornerProperty);
        set => SetValue(CornerProperty, value);
    }

    /// <summary>
    /// \if KO
    /// <para>LED UI와 현재 시각 및 모서리 상태를 초기화합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Initializes the LED UI and its current visual and corner states.</para>
    /// \endif
    /// </summary>
    public DreamineCheckLed()
    {
        InitializeComponent();
        ApplyVisualState();
        ApplyCorner();

        // 생성자 시점엔 펄스를 건너뛰었으니, 실제로 화면에 붙어 Handler가 생기면 다시 적용한다.
        HandlerChanged += (_, _) => ApplyVisualState();
    }

    /// <summary>
    /// \if KO
    /// <para>모서리 바인딩 값 변경에 응답하여 컨트롤 배치를 갱신합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Updates control placement in response to a corner bindable-value change.</para>
    /// \endif
    /// </summary>
    /// <param name="bindable">
    /// \if KO
    /// <para>값이 변경된 바인딩 가능 객체입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The bindable object whose value changed.</para>
    /// \endif
    /// </param>
    /// <param name="oldValue">
    /// \if KO
    /// <para>이전 모서리 값입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The previous corner value.</para>
    /// \endif
    /// </param>
    /// <param name="newValue">
    /// \if KO
    /// <para>새 모서리 값입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The new corner value.</para>
    /// \endif
    /// </param>
    private static void OnCornerChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is DreamineCheckLed led)
            led.ApplyCorner();
    }

    /// <summary>
    /// \if KO
    /// <para>현재 모서리에 맞게 수평 및 수직 배치 옵션을 적용합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Applies horizontal and vertical layout options for the current corner.</para>
    /// \endif
    /// </summary>
    private void ApplyCorner()
    {
        HorizontalOptions = Corner is DreamineCheckLedCorner.TopLeft or DreamineCheckLedCorner.BottomLeft
            ? LayoutOptions.Start
            : LayoutOptions.End;

        VerticalOptions = Corner is DreamineCheckLedCorner.TopLeft or DreamineCheckLedCorner.TopRight
            ? LayoutOptions.Start
            : LayoutOptions.End;
    }

    /// <summary>
    /// \if KO
    /// <para>켜짐 또는 맥동 값 변경에 응답하여 시각적 상태를 갱신합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Updates visual state in response to an on or pulse value change.</para>
    /// \endif
    /// </summary>
    /// <param name="bindable">
    /// \if KO
    /// <para>값이 변경된 바인딩 가능 객체입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The bindable object whose value changed.</para>
    /// \endif
    /// </param>
    /// <param name="oldValue">
    /// \if KO
    /// <para>이전 값입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The previous value.</para>
    /// \endif
    /// </param>
    /// <param name="newValue">
    /// \if KO
    /// <para>새 값입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The new value.</para>
    /// \endif
    /// </param>
    private static void OnVisualChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is DreamineCheckLed led)
            led.ApplyVisualState();
    }

    /// <summary>
    /// \if KO
    /// <para>지름 값 변경에 응답하여 LED 크기와 원형 스트로크를 갱신합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Updates LED dimensions and circular stroke in response to a diameter change.</para>
    /// \endif
    /// </summary>
    /// <param name="bindable">
    /// \if KO
    /// <para>값이 변경된 바인딩 가능 객체입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The bindable object whose value changed.</para>
    /// \endif
    /// </param>
    /// <param name="oldValue">
    /// \if KO
    /// <para>이전 지름 값입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The previous diameter value.</para>
    /// \endif
    /// </param>
    /// <param name="newValue">
    /// \if KO
    /// <para>새 지름 값입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The new diameter value.</para>
    /// \endif
    /// </param>
    private static void OnDiameterChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is DreamineCheckLed led)
        {
            led.Dot.WidthRequest = led.Diameter;
            led.Dot.HeightRequest = led.Diameter;
            led.Dot.StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = led.Diameter / 2 };
        }
    }

    /// <summary>
    /// \if KO
    /// <para>현재 켜짐 및 맥동 상태에 맞게 색상, 불투명도와 애니메이션을 적용합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Applies colors, opacity, and animation for the current on and pulse state.</para>
    /// \endif
    /// </summary>
    private void ApplyVisualState()
    {
        Dot.BackgroundColor = IsOn ? Color.FromArgb("#1FD36B") : Color.FromArgb("#1A2A1A");
        Dot.Stroke = IsOn ? Color.FromArgb("#7DD9B7") : Color.FromArgb("#556677");

        // Handler가 아직 없으면(= 화면에 붙기 전, 보통 생성자에서 호출되는 경우) IAnimationManager가
        // 없어서 AbortAnimation/Animation.Commit이 ArgumentException을 던진다. 그 시점엔 애니메이션을
        // 건너뛰고 단순히 최종 Opacity만 맞춰둔다 — 실제 애니메이션은 화면에 붙은 뒤 토글될 때 적용된다.
        if (Handler is null)
        {
            Dot.Opacity = 1.0;
            return;
        }

        this.AbortAnimation("PulseAnimation");

        if (IsOn && IsPulse)
        {
            var animation = new Animation(v => Dot.Opacity = v, 1.0, 0.4);
            animation.Commit(this, "PulseAnimation", 16, 450, Easing.SinInOut, finished: null, repeat: () => IsOn && IsPulse);
        }
        else
        {
            Dot.Opacity = 1.0;
        }
    }
}
