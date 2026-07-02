namespace Dreamine.UI.Maui;

/// <summary>
/// Light bulb indicator control with a shared API across WPF, WinForms, Blazor, and MAUI.
/// </summary>
public sealed class DreamineLightBulb : GraphicsView, IDrawable
{
    public static readonly BindableProperty IsOnProperty = BindableProperty.Create(
        nameof(IsOn), typeof(bool), typeof(DreamineLightBulb), false, propertyChanged: OnVisualChanged);

    public static readonly BindableProperty DiameterProperty = BindableProperty.Create(
        nameof(Diameter), typeof(double), typeof(DreamineLightBulb), 96d, propertyChanged: OnDiameterChanged);

    public bool IsOn
    {
        get => (bool)GetValue(IsOnProperty);
        set => SetValue(IsOnProperty, value);
    }

    public double Diameter
    {
        get => (double)GetValue(DiameterProperty);
        set => SetValue(DiameterProperty, value);
    }

    public DreamineLightBulb()
    {
        Drawable = this;
        InputTransparent = true;
        ApplySize();
    }

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

    private static void FillRib(ICanvas canvas, float cx, float y, float width)
    {
        canvas.FillRoundedRectangle(cx - width / 2f, y, width, 7f, 3.5f);
    }

    private static void OnVisualChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is DreamineLightBulb bulb)
            bulb.Invalidate();
    }

    private static void OnDiameterChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is DreamineLightBulb bulb)
        {
            bulb.ApplySize();
            bulb.Invalidate();
        }
    }

    private void ApplySize()
    {
        var d = Math.Max(32, Diameter);
        WidthRequest = d * 1.25;
        HeightRequest = d * 1.65;
    }
}
