namespace Dreamine.UI.Maui;

/// <summary>
/// WinUI 네이티브 CheckBox는 내부적으로 거대한 최소 너비를 가져서 라벨과의 간격을
/// WidthRequest로 줄일 수 없다. 그래서 직접 그린 체크박스로 대체했다(라벨 클릭도 토글됨).
/// </summary>
public partial class DreamineCheckBox : ContentView
{
    public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(
        nameof(IsChecked), typeof(bool), typeof(DreamineCheckBox), false,
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanged: (b, _, n) => ((DreamineCheckBox)b).ApplyState());

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text), typeof(string), typeof(DreamineCheckBox), string.Empty,
        propertyChanged: (b, _, n) => ((DreamineCheckBox)b).TextLabel.Text = (string)n);

    public bool IsChecked
    {
        get => (bool)GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public event EventHandler<bool>? CheckedChanged;

    public DreamineCheckBox()
    {
        InitializeComponent();
        ApplyState();
    }

    private void OnTapped(object? sender, TappedEventArgs e)
    {
        IsChecked = !IsChecked;
        CheckedChanged?.Invoke(this, IsChecked);
    }

    private void ApplyState()
    {
        CheckMark.IsVisible = IsChecked;
        Box.BackgroundColor = IsChecked ? Color.FromArgb("#0d6efd") : Colors.Transparent;
        Box.Stroke = IsChecked ? Color.FromArgb("#0d6efd") : Color.FromArgb("#7A8AA0");
    }
}
