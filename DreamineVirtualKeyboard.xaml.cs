namespace Dreamine.UI.Maui;

/// <summary>
/// WPF/WinForms 가상 키보드와 동일한 목적의 MAUI용 화면 키보드 데모.
/// 실제 모바일/태블릿에서는 OS가 기본 키보드를 제공하므로 보통 필요 없지만,
/// 다른 플랫폼들과 시각적으로 동일한 느낌을 보여주기 위한 데모 컴포넌트다
/// (Windows 데스크톱 모드처럼 OS 키보드가 안 뜨는 환경에서 유용).
/// 표준 영문 QWERTY 배열(특수문자 포함)을 흉내낸다. 한글 조합은 지원하지 않는다.
/// </summary>
public partial class DreamineVirtualKeyboard : ContentView
{
    private enum ShiftState { Off, Once, Lock }

    // 표준 미국식 키보드와 동일하게, Shift를 누르면 숫자/기호 키도 같이 바뀐다.
    private static readonly (string Normal, string Shifted)[] NumberRow =
    {
        ("1", "!"), ("2", "@"), ("3", "#"), ("4", "$"), ("5", "%"),
        ("6", "^"), ("7", "&"), ("8", "*"), ("9", "("), ("0", ")"),
        ("-", "_"), ("=", "+"),
    };

    private static readonly (string Normal, string Shifted)[] Row2 =
    {
        ("q", "Q"), ("w", "W"), ("e", "E"), ("r", "R"), ("t", "T"), ("y", "Y"),
        ("u", "U"), ("i", "I"), ("o", "O"), ("p", "P"), ("[", "{"), ("]", "}"),
    };

    private static readonly (string Normal, string Shifted)[] Row3 =
    {
        ("a", "A"), ("s", "S"), ("d", "D"), ("f", "F"), ("g", "G"), ("h", "H"),
        ("j", "J"), ("k", "K"), ("l", "L"), (";", ":"), ("'", "\""),
    };

    private static readonly (string Normal, string Shifted)[] Row4 =
    {
        ("z", "Z"), ("x", "X"), ("c", "C"), ("v", "V"), ("b", "B"), ("n", "N"),
        ("m", "M"), (",", "<"), (".", ">"), ("/", "?"),
    };

    private Entry? _target;
    private ShiftState _shift = ShiftState.Off;
    private DateTime _lastShiftTap = DateTime.MinValue;
    private readonly List<Button> _shiftButtons = new();
    private readonly List<(Button Button, string Normal, string Shifted)> _shiftableButtons = new();

    public DreamineVirtualKeyboard()
    {
        InitializeComponent();
        RowsHost.Children.Add(BuildShiftableRow(NumberRow, trailing: MakeKey("⌫", Backspace, 44)));
        RowsHost.Children.Add(BuildShiftableRow(Row2));
        RowsHost.Children.Add(BuildShiftableRow(Row3, trailing: MakeKey("Enter", OnEnter, 60)));
        RowsHost.Children.Add(BuildRow4());
        RowsHost.Children.Add(BuildActionRow());
        RefreshKeyLabels();
    }

    /// <summary>이 키보드가 입력을 보낼 대상 Entry를 연결한다.</summary>
    public void Attach(Entry entry)
    {
        _target = entry;
        // Unfocused로 자동 숨김 처리하면, 키보드의 키 버튼을 누르는 순간 Entry가 포커스를
        // 잃으면서(버튼으로 포커스 이동) 키보드가 바로 사라지고 Click 처리까지 끊겨서
        // 글자가 입력되지 않는다. 그래서 숨김은 "Enter" 버튼으로만 한다.
        entry.Focused += (_, _) => IsVisible = true;
    }

    private HorizontalStackLayout BuildShiftableRow((string Normal, string Shifted)[] keys, Button? trailing = null)
    {
        var row = new HorizontalStackLayout { Spacing = 3, HorizontalOptions = LayoutOptions.Center };
        foreach (var key in keys)
            row.Children.Add(MakeShiftableKey(key.Normal, key.Shifted));

        if (trailing is not null)
            row.Children.Add(trailing);
        return row;
    }

    private HorizontalStackLayout BuildRow4()
    {
        var row = new HorizontalStackLayout { Spacing = 3, HorizontalOptions = LayoutOptions.Center };
        row.Children.Add(MakeShiftKey());
        foreach (var key in Row4)
            row.Children.Add(MakeShiftableKey(key.Normal, key.Shifted));
        row.Children.Add(MakeShiftKey());
        return row;
    }

    private HorizontalStackLayout BuildActionRow()
    {
        var row = new HorizontalStackLayout { Spacing = 3, HorizontalOptions = LayoutOptions.Center };
        row.Children.Add(MakeKey("Space", () => InsertText(" "), 280));
        return row;
    }

    private Button MakeShiftableKey(string normal, string shifted)
    {
        var button = MakeKey(normal, () => InsertShiftable(normal, shifted));
        _shiftableButtons.Add((button, normal, shifted));
        return button;
    }

    private Button MakeShiftKey()
    {
        var button = MakeKey("⇧", OnShiftTapped, 46);
        _shiftButtons.Add(button);
        return button;
    }

    private Button MakeKey(string text, Action onTap, double width = 32)
    {
        var button = new Button
        {
            Text = text,
            WidthRequest = width,
            HeightRequest = 32,
            Padding = 0,
            FontSize = 12,
            BackgroundColor = Color.FromArgb("#16284D"),
            TextColor = Colors.White,
            CornerRadius = 4
        };
        button.Clicked += (_, _) => onTap();
        return button;
    }

    private void OnShiftTapped()
    {
        var isDoubleTap = (DateTime.Now - _lastShiftTap).TotalMilliseconds < 400;
        _lastShiftTap = DateTime.Now;

        _shift = isDoubleTap
            ? (_shift == ShiftState.Lock ? ShiftState.Off : ShiftState.Lock)
            : (_shift == ShiftState.Off ? ShiftState.Once : ShiftState.Off);

        RefreshKeyLabels();
    }

    /// <summary>Shift/Caps 상태에 맞춰 글자·숫자·기호 키에 실제로 표시되는 텍스트를 전부 갱신한다.</summary>
    private void RefreshKeyLabels()
    {
        var shifted = _shift != ShiftState.Off;
        foreach (var (button, normal, shiftedText) in _shiftableButtons)
            button.Text = shifted ? shiftedText : normal;

        foreach (var shiftButton in _shiftButtons)
            shiftButton.BackgroundColor = _shift switch
            {
                ShiftState.Lock => Color.FromArgb("#0d6efd"),
                ShiftState.Once => Color.FromArgb("#3a6fc4"),
                _ => Color.FromArgb("#16284D")
            };
    }

    private void InsertShiftable(string normal, string shifted)
    {
        InsertText(_shift != ShiftState.Off ? shifted : normal);
        if (_shift == ShiftState.Once)
        {
            _shift = ShiftState.Off;
            RefreshKeyLabels();
        }
    }

    private void InsertText(string text)
    {
        if (_target is null) return;
        var cursor = _target.CursorPosition;
        _target.Text = (_target.Text ?? string.Empty).Insert(Math.Min(cursor, _target.Text?.Length ?? 0), text);
        _target.CursorPosition = cursor + text.Length;
    }

    private void Backspace()
    {
        if (_target is null || string.IsNullOrEmpty(_target.Text)) return;
        var cursor = _target.CursorPosition;
        if (cursor <= 0) return;
        _target.Text = _target.Text.Remove(cursor - 1, 1);
        _target.CursorPosition = cursor - 1;
    }

    /// <summary>Entry는 한 줄짜리라 줄바꿈 대신 입력 완료로 취급해 키보드를 닫는다.</summary>
    private void OnEnter()
    {
        _target?.Unfocus();
        IsVisible = false;
    }
}
