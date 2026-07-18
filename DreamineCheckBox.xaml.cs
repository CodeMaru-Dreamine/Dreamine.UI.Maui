namespace Dreamine.UI.Maui;

/// <summary>
/// \if KO
/// <para>라벨 클릭과 양방향 바인딩을 지원하는 직접 그린 MAUI 체크 상자입니다.</para>
/// \endif
/// \if EN
/// <para>Provides a custom-drawn MAUI check box with label-click toggling and two-way binding.</para>
/// \endif
/// </summary>
/// <remarks>
/// \if KO
/// <para>WinUI 기본 체크 상자의 고정 최소 너비를 피하기 위해 상자와 체크 표시를 직접 렌더링합니다.</para>
/// \endif
/// \if EN
/// <para>Renders the box and check mark directly to avoid the fixed minimum width of the native WinUI check box.</para>
/// \endif
/// </remarks>
public partial class DreamineCheckBox : ContentView
{
    /// <summary>
    /// \if KO
    /// <para><see cref="IsChecked"/> 바인딩 가능 속성을 식별합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Identifies the <see cref="IsChecked"/> bindable property.</para>
    /// \endif
    /// </summary>
    public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(
        nameof(IsChecked), typeof(bool), typeof(DreamineCheckBox), false,
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanged: (b, _, n) => ((DreamineCheckBox)b).ApplyState());

    /// <summary>
    /// \if KO
    /// <para><see cref="Text"/> 바인딩 가능 속성을 식별합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Identifies the <see cref="Text"/> bindable property.</para>
    /// \endif
    /// </summary>
    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text), typeof(string), typeof(DreamineCheckBox), string.Empty,
        propertyChanged: (b, _, n) => ((DreamineCheckBox)b).TextLabel.Text = (string)n);

    /// <summary>
    /// \if KO
    /// <para>체크 상자가 선택되어 있는지 여부를 가져오거나 설정합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Gets or sets whether the check box is checked.</para>
    /// \endif
    /// </summary>
    public bool IsChecked
    {
        get => (bool)GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }

    /// <summary>
    /// \if KO
    /// <para>체크 상자 옆에 표시할 라벨 텍스트를 가져오거나 설정합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Gets or sets the label text displayed next to the check box.</para>
    /// \endif
    /// </summary>
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>
    /// \if KO
    /// <para>사용자 입력으로 체크 상태가 변경될 때 발생합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Occurs when user input changes the checked state.</para>
    /// \endif
    /// </summary>
    public event EventHandler<bool>? CheckedChanged;

    /// <summary>
    /// \if KO
    /// <para>체크 상자 UI를 초기화하고 현재 시각적 상태를 적용합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Initializes the check-box UI and applies its current visual state.</para>
    /// \endif
    /// </summary>
    public DreamineCheckBox()
    {
        InitializeComponent();
        ApplyState();
    }

    /// <summary>
    /// \if KO
    /// <para>탭 입력에 응답하여 체크 상태를 전환하고 변경 이벤트를 발생시킵니다.</para>
    /// \endif
    /// \if EN
    /// <para>Toggles the checked state in response to a tap and raises the change event.</para>
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
    private void OnTapped(object? sender, TappedEventArgs e)
    {
        IsChecked = !IsChecked;
        CheckedChanged?.Invoke(this, IsChecked);
    }

    /// <summary>
    /// \if KO
    /// <para>현재 체크 상태에 맞게 표시와 색상을 갱신합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Updates visibility and colors to reflect the current checked state.</para>
    /// \endif
    /// </summary>
    private void ApplyState()
    {
        CheckMark.IsVisible = IsChecked;
        Box.BackgroundColor = IsChecked ? Color.FromArgb("#0d6efd") : Colors.Transparent;
        Box.Stroke = IsChecked ? Color.FromArgb("#0d6efd") : Color.FromArgb("#7A8AA0");
    }
}
