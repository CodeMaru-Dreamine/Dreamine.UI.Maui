namespace Dreamine.UI.Maui;

/// <summary>WPF DreamineCheckLed.Corner와 동일한 4방향 모서리 앵커.</summary>
public enum DreamineCheckLedCorner
{
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight
}

/// <summary>
/// WPF/WinForms DreamineCheckLed와 동일한 개념의 MAUI용 LED 표시 컨트롤.
/// .NET MAUI에는 대응 컨트롤이 없어서 직접 만들었다.
/// </summary>
public partial class DreamineCheckLed : ContentView
{
    public static readonly BindableProperty IsOnProperty = BindableProperty.Create(
        nameof(IsOn), typeof(bool), typeof(DreamineCheckLed), false, propertyChanged: OnVisualChanged);

    public static readonly BindableProperty IsPulseProperty = BindableProperty.Create(
        nameof(IsPulse), typeof(bool), typeof(DreamineCheckLed), false, propertyChanged: OnVisualChanged);

    public static readonly BindableProperty DiameterProperty = BindableProperty.Create(
        nameof(Diameter), typeof(double), typeof(DreamineCheckLed), 24d, propertyChanged: OnDiameterChanged);

    /// <summary>
    /// 이 LED를 부모 컨테이너(예: 카드 박스)의 어느 모서리에 붙일지 지정한다.
    /// WPF DreamineCheckLed.Corner처럼, LED를 다른 컨트롤 위에 상태 배지로 얹을 때 쓴다.
    /// </summary>
    public static readonly BindableProperty CornerProperty = BindableProperty.Create(
        nameof(Corner), typeof(DreamineCheckLedCorner), typeof(DreamineCheckLed), DreamineCheckLedCorner.TopLeft,
        propertyChanged: OnCornerChanged);

    public bool IsOn
    {
        get => (bool)GetValue(IsOnProperty);
        set => SetValue(IsOnProperty, value);
    }

    public bool IsPulse
    {
        get => (bool)GetValue(IsPulseProperty);
        set => SetValue(IsPulseProperty, value);
    }

    public double Diameter
    {
        get => (double)GetValue(DiameterProperty);
        set => SetValue(DiameterProperty, value);
    }

    public DreamineCheckLedCorner Corner
    {
        get => (DreamineCheckLedCorner)GetValue(CornerProperty);
        set => SetValue(CornerProperty, value);
    }

    public DreamineCheckLed()
    {
        InitializeComponent();
        ApplyVisualState();
        ApplyCorner();

        // 생성자 시점엔 펄스를 건너뛰었으니, 실제로 화면에 붙어 Handler가 생기면 다시 적용한다.
        HandlerChanged += (_, _) => ApplyVisualState();
    }

    private static void OnCornerChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is DreamineCheckLed led)
            led.ApplyCorner();
    }

    private void ApplyCorner()
    {
        HorizontalOptions = Corner is DreamineCheckLedCorner.TopLeft or DreamineCheckLedCorner.BottomLeft
            ? LayoutOptions.Start
            : LayoutOptions.End;

        VerticalOptions = Corner is DreamineCheckLedCorner.TopLeft or DreamineCheckLedCorner.TopRight
            ? LayoutOptions.Start
            : LayoutOptions.End;
    }

    private static void OnVisualChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is DreamineCheckLed led)
            led.ApplyVisualState();
    }

    private static void OnDiameterChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is DreamineCheckLed led)
        {
            led.Dot.WidthRequest = led.Diameter;
            led.Dot.HeightRequest = led.Diameter;
            led.Dot.StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = led.Diameter / 2 };
        }
    }

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
