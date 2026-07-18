namespace Dreamine.UI.Maui;

/// <summary>
/// \if KO
/// <para>머리글 탭으로 콘텐츠를 펼치거나 접을 수 있는 MAUI 컨트롤입니다.</para>
/// \endif
/// \if EN
/// <para>Provides a MAUI control whose content can be expanded or collapsed by tapping its header.</para>
/// \endif
/// </summary>
public partial class DreamineExpander : ContentView
{
    /// <summary>
    /// \if KO
    /// <para><see cref="Header"/> 바인딩 가능 속성을 식별합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Identifies the <see cref="Header"/> bindable property.</para>
    /// \endif
    /// </summary>
    public static readonly BindableProperty HeaderProperty = BindableProperty.Create(
        nameof(Header), typeof(string), typeof(DreamineExpander), string.Empty,
        propertyChanged: (b, _, n) => ((DreamineExpander)b).HeaderLabel.Text = (string)n);

    /// <summary>
    /// \if KO
    /// <para><see cref="IsExpanded"/> 바인딩 가능 속성을 식별합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Identifies the <see cref="IsExpanded"/> bindable property.</para>
    /// \endif
    /// </summary>
    public static readonly BindableProperty IsExpandedProperty = BindableProperty.Create(
        nameof(IsExpanded), typeof(bool), typeof(DreamineExpander), true,
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanged: (b, _, n) => ((DreamineExpander)b).ApplyExpandedState());

    /// <summary>
    /// \if KO
    /// <para><see cref="ExpanderContent"/> 바인딩 가능 속성을 식별합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Identifies the <see cref="ExpanderContent"/> bindable property.</para>
    /// \endif
    /// </summary>
    public static readonly BindableProperty ExpanderContentProperty = BindableProperty.Create(
        nameof(ExpanderContent), typeof(View), typeof(DreamineExpander), null,
        propertyChanged: (b, _, n) => ((DreamineExpander)b).ContentHost.Content = (View?)n);

    /// <summary>
    /// \if KO
    /// <para>확장 영역의 머리글 텍스트를 가져오거나 설정합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Gets or sets the header text of the expandable region.</para>
    /// \endif
    /// </summary>
    public string Header
    {
        get => (string)GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    /// <summary>
    /// \if KO
    /// <para>콘텐츠가 펼쳐져 있는지 여부를 가져오거나 설정합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Gets or sets whether the content is expanded.</para>
    /// \endif
    /// </summary>
    public bool IsExpanded
    {
        get => (bool)GetValue(IsExpandedProperty);
        set => SetValue(IsExpandedProperty, value);
    }

    /// <summary>
    /// \if KO
    /// <para>펼쳤을 때 표시할 내부 콘텐츠를 가져오거나 설정합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Gets or sets the inner content displayed while expanded.</para>
    /// \endif
    /// </summary>
    public View? ExpanderContent
    {
        get => (View?)GetValue(ExpanderContentProperty);
        set => SetValue(ExpanderContentProperty, value);
    }

    /// <summary>
    /// \if KO
    /// <para>사용자 입력으로 확장 상태가 변경될 때 발생합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Occurs when user input changes the expansion state.</para>
    /// \endif
    /// </summary>
    public event EventHandler? ExpandedChanged;

    /// <summary>
    /// \if KO
    /// <para>확장 컨트롤 UI를 초기화하고 현재 확장 상태를 적용합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Initializes the expander UI and applies its current expansion state.</para>
    /// \endif
    /// </summary>
    public DreamineExpander()
    {
        InitializeComponent();
        ApplyExpandedState();
    }

    /// <summary>
    /// \if KO
    /// <para>머리글 탭에 응답하여 확장 상태를 전환하고 변경 이벤트를 발생시킵니다.</para>
    /// \endif
    /// \if EN
    /// <para>Toggles expansion in response to a header tap and raises the change event.</para>
    /// \endif
    /// </summary>
    /// <param name="sender">
    /// \if KO
    /// <para>이벤트를 발생시킨 객체입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The object that raised the event.</para>
    /// \endif
    /// </param>
    /// <param name="e">
    /// \if KO
    /// <para>탭 이벤트 인수입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The tap event arguments.</para>
    /// \endif
    /// </param>
    private void OnHeaderTapped(object? sender, TappedEventArgs e)
    {
        IsExpanded = !IsExpanded;
        ExpandedChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// \if KO
    /// <para>현재 확장 상태에 맞게 콘텐츠 표시와 화살표를 갱신합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Updates content visibility and the arrow to reflect the current expansion state.</para>
    /// \endif
    /// </summary>
    private void ApplyExpandedState()
    {
        ContentHost.IsVisible = IsExpanded;
        ArrowLabel.Text = IsExpanded ? "▼" : "▶";
    }
}
