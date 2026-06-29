namespace Dreamine.UI.Maui;

/// <summary>
/// WPF/WinForms DreamineExpander와 동일한 개념의 MAUI용 펼치기/접기 컨트롤.
/// .NET MAUI에는 대응 컨트롤이 없어서 직접 만들었다.
/// </summary>
public partial class DreamineExpander : ContentView
{
    public static readonly BindableProperty HeaderProperty = BindableProperty.Create(
        nameof(Header), typeof(string), typeof(DreamineExpander), string.Empty,
        propertyChanged: (b, _, n) => ((DreamineExpander)b).HeaderLabel.Text = (string)n);

    public static readonly BindableProperty IsExpandedProperty = BindableProperty.Create(
        nameof(IsExpanded), typeof(bool), typeof(DreamineExpander), true,
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanged: (b, _, n) => ((DreamineExpander)b).ApplyExpandedState());

    /// <summary>펼쳤을 때 보여줄 내부 콘텐츠. ContentView 고유의 Content와 별개다.</summary>
    public static readonly BindableProperty ExpanderContentProperty = BindableProperty.Create(
        nameof(ExpanderContent), typeof(View), typeof(DreamineExpander), null,
        propertyChanged: (b, _, n) => ((DreamineExpander)b).ContentHost.Content = (View?)n);

    public string Header
    {
        get => (string)GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public bool IsExpanded
    {
        get => (bool)GetValue(IsExpandedProperty);
        set => SetValue(IsExpandedProperty, value);
    }

    public View? ExpanderContent
    {
        get => (View?)GetValue(ExpanderContentProperty);
        set => SetValue(ExpanderContentProperty, value);
    }

    public event EventHandler? ExpandedChanged;

    public DreamineExpander()
    {
        InitializeComponent();
        ApplyExpandedState();
    }

    private void OnHeaderTapped(object? sender, TappedEventArgs e)
    {
        IsExpanded = !IsExpanded;
        ExpandedChanged?.Invoke(this, EventArgs.Empty);
    }

    private void ApplyExpandedState()
    {
        ContentHost.IsVisible = IsExpanded;
        ArrowLabel.Text = IsExpanded ? "▼" : "▶";
    }
}
