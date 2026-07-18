namespace Dreamine.UI.Maui;

/// <summary>
/// \if KO
/// <para>플랫폼 간 공통 API로 상태를 직접 그리는 MAUI 전구 표시 컨트롤입니다.</para>
/// \endif
/// \if EN
/// <para>Provides a MAUI light-bulb indicator that draws its state directly through a cross-platform API.</para>
/// \endif
/// </summary>
public sealed class DreamineLightBulb : GraphicsView, IDrawable
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
        nameof(IsOn), typeof(bool), typeof(DreamineLightBulb), false, propertyChanged: OnVisualChanged);

    /// <summary>
    /// \if KO
    /// <para><see cref="Diameter"/> 바인딩 가능 속성을 식별합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Identifies the <see cref="Diameter"/> bindable property.</para>
    /// \endif
    /// </summary>
    public static readonly BindableProperty DiameterProperty = BindableProperty.Create(
        nameof(Diameter), typeof(double), typeof(DreamineLightBulb), 96d, propertyChanged: OnDiameterChanged);

    /// <summary>
    /// \if KO
    /// <para>전구가 켜져 있는지 여부를 가져오거나 설정합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Gets or sets whether the light bulb is on.</para>
    /// \endif
    /// </summary>
    public bool IsOn
    {
        get => (bool)GetValue(IsOnProperty);
        set => SetValue(IsOnProperty, value);
    }

    /// <summary>
    /// \if KO
    /// <para>전구 유리 부분의 기준 지름을 가져오거나 설정합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Gets or sets the reference diameter of the bulb's glass portion.</para>
    /// \endif
    /// </summary>
    public double Diameter
    {
        get => (double)GetValue(DiameterProperty);
        set => SetValue(DiameterProperty, value);
    }

    /// <summary>
    /// \if KO
    /// <para>전구 그리기 공급자와 초기 크기를 구성합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Configures the bulb drawing provider and initial size.</para>
    /// \endif
    /// </summary>
    public DreamineLightBulb()
    {
        Drawable = this;
        InputTransparent = true;
        ApplySize();
    }

    /// <summary>
    /// \if KO
    /// <para>현재 켜짐 상태에 맞게 전구 유리, 필라멘트 및 소켓을 그립니다.</para>
    /// \endif
    /// \if EN
    /// <para>Draws the bulb glass, filament, and socket for the current on state.</para>
    /// \endif
    /// </summary>
    /// <param name="canvas">
    /// \if KO
    /// <para>전구를 그릴 MAUI 캔버스입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The MAUI canvas on which the bulb is drawn.</para>
    /// \endif
    /// </param>
    /// <param name="dirtyRect">
    /// \if KO
    /// <para>다시 그려야 하는 영역입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The region that must be redrawn.</para>
    /// \endif
    /// </param>
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        var d = (float)Math.Max(32, Diameter);
        var cx = dirtyRect.Center.X;
        var top = dirtyRect.Top + 2f;
        var glassFill = IsOn ? Color.FromArgb("#FFD666") : Color.FromRgba(100, 116, 139, 42);
        var glassStroke = IsOn ? Color.FromArgb("#FFC400") : Color.FromArgb("#66758B");
        var filament = IsOn ? Color.FromArgb("#7A4B00") : Color.FromArgb("#64748B");
        var baseFill = Color.FromArgb("#708098");

        canvas.SaveState();
        canvas.Antialias = true;

        if (IsOn)
        {
            canvas.FillColor = Color.FromRgba(255, 214, 102, 72);
            canvas.FillEllipse(cx - d * .62f, top + d * .50f - d * .62f, d * 1.24f, d * 1.24f);
        }

        var glass = CreateGlassPath(cx, top, d);
        canvas.FillColor = glassFill;
        canvas.StrokeColor = glassStroke;
        canvas.StrokeSize = 4;
        canvas.FillPath(glass);
        canvas.DrawPath(glass);

        var filamentPath = new PathF();
        filamentPath.MoveTo(cx - d * .22f, top + d * .56f);
        filamentPath.CurveTo(cx - d * .12f, top + d * .38f, cx - d * .02f, top + d * .72f, cx + d * .10f, top + d * .53f);
        filamentPath.CurveTo(cx + d * .16f, top + d * .44f, cx + d * .21f, top + d * .49f, cx + d * .25f, top + d * .55f);
        canvas.StrokeColor = filament;
        canvas.StrokeSize = 4;
        canvas.StrokeLineCap = LineCap.Round;
        canvas.DrawPath(filamentPath);

        var neckTop = top + d * .92f;
        var neck = new PathF();
        neck.MoveTo(cx - d * .30f, neckTop);
        neck.LineTo(cx + d * .30f, neckTop);
        neck.LineTo(cx + d * .20f, neckTop + d * .26f);
        neck.LineTo(cx - d * .20f, neckTop + d * .26f);
        neck.Close();

        canvas.FillColor = baseFill;
        canvas.FillPath(neck);
        FillRib(canvas, cx, neckTop + d * .10f, d * .44f);
        FillRib(canvas, cx, neckTop + d * .22f, d * .36f);
        FillRib(canvas, cx, neckTop + d * .34f, d * .27f);

        canvas.RestoreState();
    }

    /// <summary>
    /// \if KO
    /// <para>전구 유리 외곽선을 나타내는 경로를 만듭니다.</para>
    /// \endif
    /// \if EN
    /// <para>Creates a path representing the outline of the bulb glass.</para>
    /// \endif
    /// </summary>
    /// <param name="cx">
    /// \if KO
    /// <para>전구 중심의 X 좌표입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The X coordinate of the bulb center.</para>
    /// \endif
    /// </param>
    /// <param name="top">
    /// \if KO
    /// <para>전구 위쪽 Y 좌표입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The top Y coordinate of the bulb.</para>
    /// \endif
    /// </param>
    /// <param name="d">
    /// \if KO
    /// <para>기준 지름입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The reference diameter.</para>
    /// \endif
    /// </param>
    /// <returns>
    /// \if KO
    /// <para>닫힌 전구 유리 경로입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The closed bulb-glass path.</para>
    /// \endif
    /// </returns>
    private static PathF CreateGlassPath(float cx, float top, float d)
    {
        var path = new PathF();
        path.MoveTo(cx, top + d * .02f);
        path.CurveTo(cx - d * .36f, top + d * .02f, cx - d * .52f, top + d * .27f, cx - d * .52f, top + d * .54f);
        path.CurveTo(cx - d * .52f, top + d * .74f, cx - d * .35f, top + d * .87f, cx - d * .25f, top + d * .96f);
        path.LineTo(cx + d * .25f, top + d * .96f);
        path.CurveTo(cx + d * .35f, top + d * .87f, cx + d * .52f, top + d * .74f, cx + d * .52f, top + d * .54f);
        path.CurveTo(cx + d * .52f, top + d * .27f, cx + d * .36f, top + d * .02f, cx, top + d * .02f);
        path.Close();
        return path;
    }

    /// <summary>
    /// \if KO
    /// <para>전구 소켓의 둥근 가로 홈 하나를 채웁니다.</para>
    /// \endif
    /// \if EN
    /// <para>Fills one rounded horizontal rib of the bulb socket.</para>
    /// \endif
    /// </summary>
    /// <param name="canvas">
    /// \if KO
    /// <para>홈을 그릴 캔버스입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The canvas on which the rib is drawn.</para>
    /// \endif
    /// </param>
    /// <param name="cx">
    /// \if KO
    /// <para>홈 중심의 X 좌표입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The X coordinate of the rib center.</para>
    /// \endif
    /// </param>
    /// <param name="y">
    /// \if KO
    /// <para>홈의 Y 좌표입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The Y coordinate of the rib.</para>
    /// \endif
    /// </param>
    /// <param name="width">
    /// \if KO
    /// <para>홈 너비입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The rib width.</para>
    /// \endif
    /// </param>
    private static void FillRib(ICanvas canvas, float cx, float y, float width)
    {
        canvas.FillRoundedRectangle(cx - width / 2f, y, width, 7f, 3.5f);
    }

    /// <summary>
    /// \if KO
    /// <para>켜짐 값 변경에 응답하여 전구를 다시 그리도록 요청합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Requests a redraw in response to an on-state value change.</para>
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
        if (bindable is DreamineLightBulb bulb)
            bulb.Invalidate();
    }

    /// <summary>
    /// \if KO
    /// <para>지름 값 변경에 응답하여 컨트롤 크기를 갱신하고 다시 그리도록 요청합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Updates control size and requests a redraw in response to a diameter change.</para>
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
        if (bindable is DreamineLightBulb bulb)
        {
            bulb.ApplySize();
            bulb.Invalidate();
        }
    }

    /// <summary>
    /// \if KO
    /// <para>최소 지름을 적용하여 그리기 뷰의 요청 너비와 높이를 계산합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Calculates the drawing view's requested width and height using the minimum diameter.</para>
    /// \endif
    /// </summary>
    private void ApplySize()
    {
        var d = Math.Max(32, Diameter);
        WidthRequest = d * 1.25;
        HeightRequest = d * 1.65;
    }
}
