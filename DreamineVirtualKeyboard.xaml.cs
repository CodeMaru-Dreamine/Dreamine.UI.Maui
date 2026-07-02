namespace Dreamine.UI.Maui;

/// <summary>
/// WPF/WinForms 가상 키보드와 같은 5행 QWERTY 레이아웃의 MAUI 화면 키보드.
/// 브라우저/모바일 OS IME 제어가 아니라, 연결된 Entry에 직접 텍스트를 편집한다.
/// </summary>
public partial class DreamineVirtualKeyboard : ContentView
{
    private static readonly Color KeyBackground = Color.FromArgb("#F1685E");
    private static readonly Color KeySelectedBackground = Color.FromArgb("#F59584");

    private readonly List<KeyButton> _textKeys = [];
    private readonly HangulComposer _composer = new();
    private Entry? _target;

    private bool _shift;
    private bool _capsLock;
    private bool _korean;
    private Button? _shiftButton;
    private Button? _capsButton;
    private Button? _languageButton;

    public DreamineVirtualKeyboard()
    {
        InitializeComponent();
        BuildKeyboard();
        RefreshKeys();
    }

    /// <summary>이 키보드가 입력을 보낼 대상 Entry를 연결한다.</summary>
    public void Attach(Entry entry)
    {
        _target = entry;
        entry.Focused += (_, _) => IsVisible = true;
    }

    private void BuildKeyboard()
    {
        RowsHost.Children.Clear();

        AddRow(
        [
            KeySpec.Command("Esc", 54, HideKeyboard),
            KeySpec.Text("`"), KeySpec.Text("1"), KeySpec.Text("2"), KeySpec.Text("3"), KeySpec.Text("4"),
            KeySpec.Text("5"), KeySpec.Text("6"), KeySpec.Text("7"), KeySpec.Text("8"), KeySpec.Text("9"),
            KeySpec.Text("0"), KeySpec.Text("-"), KeySpec.Text("="),
            KeySpec.Command("Backspace", 116, Backspace)
        ]);

        AddRow(
        [
            KeySpec.Command("Tab", 100, () => InsertRaw("    ")),
            KeySpec.Text("q"), KeySpec.Text("w"), KeySpec.Text("e"), KeySpec.Text("r"), KeySpec.Text("t"),
            KeySpec.Text("y"), KeySpec.Text("u"), KeySpec.Text("i"), KeySpec.Text("o"), KeySpec.Text("p"),
            KeySpec.Text("["), KeySpec.Text("]"), KeySpec.Text("\\", 100)
        ]);

        AddRow(
        [
            KeySpec.Command("Caps Lock", 116, ToggleCapsLock),
            KeySpec.Text("a"), KeySpec.Text("s"), KeySpec.Text("d"), KeySpec.Text("f"), KeySpec.Text("g"),
            KeySpec.Text("h"), KeySpec.Text("j"), KeySpec.Text("k"), KeySpec.Text("l"),
            KeySpec.Text(";"), KeySpec.Text("'"),
            KeySpec.Command("Enter", 116, OnEnter)
        ]);

        AddRow(
        [
            KeySpec.Command("Shift", 138, ToggleShift),
            KeySpec.Text("z"), KeySpec.Text("x"), KeySpec.Text("c"), KeySpec.Text("v"), KeySpec.Text("b"),
            KeySpec.Text("n"), KeySpec.Text("m"), KeySpec.Text(","), KeySpec.Text("."), KeySpec.Text("/"),
            KeySpec.Command("◀", 58, MoveLeft),
            KeySpec.Command("▶", 58, MoveRight)
        ]);

        AddRow(
        [
            KeySpec.Command("Ctrl", 124, () => { }),
            KeySpec.Command("Space", 696, () => InsertRaw(" ")),
            KeySpec.Command("abc", 72, ToggleLanguage)
        ]);
    }

    private void AddRow(IReadOnlyList<KeySpec> specs)
    {
        var row = new Grid
        {
            ColumnSpacing = 4,
            HeightRequest = 54,
            HorizontalOptions = LayoutOptions.Fill
        };

        for (var i = 0; i < specs.Count; i++)
        {
            var width = specs[i].Width;
            row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(width, GridUnitType.Star) });
            var button = MakeKey(specs[i]);
            row.Add(button, i, 0);
        }

        RowsHost.Children.Add(row);
    }

    private Button MakeKey(KeySpec spec)
    {
        var button = new Button
        {
            Text = spec.Label,
            HeightRequest = 54,
            Padding = 0,
            FontSize = spec.Width > 90 ? 14 : 16,
            BackgroundColor = KeyBackground,
            TextColor = Colors.White,
            CornerRadius = 4
        };

        if (spec.Kind == KeyKind.Text)
            _textKeys.Add(new KeyButton(button, spec.Label));

        if (spec.Label == "Shift")
            _shiftButton = button;
        else if (spec.Label == "Caps Lock")
            _capsButton = button;
        else if (spec.Label is "abc" or "가")
            _languageButton = button;

        button.Clicked += (_, _) =>
        {
            if (_target is null)
                return;

            if (spec.Kind == KeyKind.Text)
                InsertKey(spec.Label);
            else
                spec.Action?.Invoke();
        };

        return button;
    }

    private void InsertKey(string key)
    {
        var text = GetKeyText(key);
        if (_korean && HangulComposer.IsComposableJamo(text))
        {
            var edit = _composer.Input(text, GetTextBeforeCaret());
            ReplaceTextTail(edit.ReplaceCount, edit.Text);
        }
        else
        {
            _composer.Reset();
            InsertRaw(text);
        }

        if (_shift)
        {
            _shift = false;
            RefreshKeys();
        }
    }

    private void InsertRaw(string text)
    {
        if (_target is null)
            return;

        _composer.Reset();
        ReplaceTextTail(0, text);
    }

    private void ReplaceTextTail(int replaceCount, string text)
    {
        if (_target is null)
            return;

        var current = _target.Text ?? string.Empty;
        var selectionStart = Math.Clamp(_target.CursorPosition, 0, current.Length);
        var selectionLength = Math.Clamp(_target.SelectionLength, 0, current.Length - selectionStart);

        if (selectionLength > 0)
        {
            _target.Text = current.Remove(selectionStart, selectionLength).Insert(selectionStart, text);
            _target.CursorPosition = selectionStart + text.Length;
        }
        else
        {
            var removeStart = Math.Max(0, selectionStart - replaceCount);
            removeStart = Math.Min(removeStart, current.Length);
            var removeLength = Math.Clamp(selectionStart - removeStart, 0, current.Length - removeStart);
            _target.Text = current.Remove(removeStart, removeLength).Insert(removeStart, text);
            _target.CursorPosition = removeStart + text.Length;
        }
    }

    private string GetTextBeforeCaret()
    {
        if (_target is null)
            return string.Empty;

        var text = _target.Text ?? string.Empty;
        var caret = Math.Clamp(_target.CursorPosition, 0, text.Length);
        return text[..caret];
    }

    private void Backspace()
    {
        if (_target is null)
            return;

        _composer.Reset();
        var current = _target.Text ?? string.Empty;
        var selectionStart = Math.Clamp(_target.CursorPosition, 0, current.Length);
        var selectionLength = Math.Clamp(_target.SelectionLength, 0, current.Length - selectionStart);

        if (selectionLength > 0)
        {
            _target.Text = current.Remove(selectionStart, selectionLength);
            _target.CursorPosition = selectionStart;
        }
        else if (selectionStart > 0)
        {
            _target.Text = current.Remove(selectionStart - 1, 1);
            _target.CursorPosition = selectionStart - 1;
        }
    }

    private void ToggleShift()
    {
        _shift = !_shift;
        RefreshKeys();
    }

    private void ToggleCapsLock()
    {
        _capsLock = !_capsLock;
        _composer.Reset();
        RefreshKeys();
    }

    private void ToggleLanguage()
    {
        _korean = !_korean;
        _composer.Reset();
        RefreshKeys();
    }

    private void MoveLeft()
    {
        if (_target is null)
            return;

        _target.CursorPosition = Math.Max(0, _target.CursorPosition - 1);
        _target.SelectionLength = 0;
    }

    private void MoveRight()
    {
        if (_target is null)
            return;

        _target.CursorPosition = Math.Min((_target.Text ?? string.Empty).Length, _target.CursorPosition + 1);
        _target.SelectionLength = 0;
    }

    private void HideKeyboard()
    {
        _composer.Reset();
        IsVisible = false;
    }

    /// <summary>Entry는 한 줄짜리라 줄바꿈 대신 입력 완료로 취급해 키보드를 닫는다.</summary>
    private void OnEnter()
    {
        _composer.Reset();
        _target?.Unfocus();
        IsVisible = false;
    }

    private void RefreshKeys()
    {
        foreach (var key in _textKeys)
            key.Button.Text = GetKeyText(key.BaseText);

        if (_shiftButton is not null)
            _shiftButton.BackgroundColor = _shift ? KeySelectedBackground : KeyBackground;

        if (_capsButton is not null)
            _capsButton.BackgroundColor = _capsLock ? KeySelectedBackground : KeyBackground;

        if (_languageButton is not null)
        {
            _languageButton.Text = _korean ? "가" : "abc";
            _languageButton.BackgroundColor = _korean ? KeySelectedBackground : KeyBackground;
        }
    }

    private string GetKeyText(string key)
    {
        if (_korean && KoreanKeys.TryGetValue(key, out var korean))
            return _shift && KoreanShiftKeys.TryGetValue(key, out var koreanShift) ? koreanShift : korean;

        if (_shift && ShiftKeys.TryGetValue(key, out var shifted))
            return shifted;

        if (key.Length == 1 && char.IsLetter(key[0]))
            return _shift ^ _capsLock ? key.ToUpperInvariant() : key.ToLowerInvariant();

        return key;
    }

    private static readonly Dictionary<string, string> ShiftKeys = new()
    {
        ["`"] = "~", ["1"] = "!", ["2"] = "@", ["3"] = "#", ["4"] = "$",
        ["5"] = "%", ["6"] = "^", ["7"] = "&", ["8"] = "*", ["9"] = "(",
        ["0"] = ")", ["-"] = "_", ["="] = "+", ["["] = "{", ["]"] = "}",
        ["\\"] = "|", [";"] = ":", ["'"] = "\"", [","] = "<", ["."] = ">",
        ["/"] = "?",
    };

    private static readonly Dictionary<string, string> KoreanKeys = new()
    {
        ["q"] = "ㅂ", ["w"] = "ㅈ", ["e"] = "ㄷ", ["r"] = "ㄱ", ["t"] = "ㅅ",
        ["y"] = "ㅛ", ["u"] = "ㅕ", ["i"] = "ㅑ", ["o"] = "ㅐ", ["p"] = "ㅔ",
        ["a"] = "ㅁ", ["s"] = "ㄴ", ["d"] = "ㅇ", ["f"] = "ㄹ", ["g"] = "ㅎ",
        ["h"] = "ㅗ", ["j"] = "ㅓ", ["k"] = "ㅏ", ["l"] = "ㅣ",
        ["z"] = "ㅋ", ["x"] = "ㅌ", ["c"] = "ㅊ", ["v"] = "ㅍ", ["b"] = "ㅠ",
        ["n"] = "ㅜ", ["m"] = "ㅡ",
    };

    private static readonly Dictionary<string, string> KoreanShiftKeys = new()
    {
        ["q"] = "ㅃ", ["w"] = "ㅉ", ["e"] = "ㄸ", ["r"] = "ㄲ", ["t"] = "ㅆ",
        ["o"] = "ㅒ", ["p"] = "ㅖ",
    };

    private sealed record KeyButton(Button Button, string BaseText);
    private sealed record KeySpec(KeyKind Kind, string Label, int Width, Action? Action)
    {
        public static KeySpec Text(string label, int width = 54) => new(KeyKind.Text, label, width, null);
        public static KeySpec Command(string label, int width, Action action) => new(KeyKind.Command, label, width, action);
    }

    private enum KeyKind
    {
        Text,
        Command
    }
}
