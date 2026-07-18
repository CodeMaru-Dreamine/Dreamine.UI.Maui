namespace Dreamine.UI.Maui;

/// <summary>
/// \if KO
/// <para>연결된 MAUI <see cref="Entry"/>를 직접 편집하는 5행 QWERTY 화면 키보드입니다.</para>
/// \endif
/// \if EN
/// <para>Provides a five-row QWERTY on-screen keyboard that directly edits an attached MAUI <see cref="Entry"/>.</para>
/// \endif
/// </summary>
/// <remarks>
/// \if KO
/// <para>운영체제 IME를 제어하지 않고 내부 한글 조합기를 사용하여 입력 값을 갱신합니다.</para>
/// \endif
/// \if EN
/// <para>Updates the input value with an internal Hangul composer instead of controlling the operating-system IME.</para>
/// \endif
/// </remarks>
public partial class DreamineVirtualKeyboard : ContentView
{
    /// <summary>
    /// \if KO
    /// <para>Key Background 값을 보관합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Stores the key background value.</para>
    /// \endif
    /// </summary>
    private static readonly Color KeyBackground = Color.FromArgb("#F1685E");
    /// <summary>
    /// \if KO
    /// <para>Key Selected Background 값을 보관합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Stores the key selected background value.</para>
    /// \endif
    /// </summary>
    private static readonly Color KeySelectedBackground = Color.FromArgb("#F59584");

    /// <summary>
    /// \if KO
    /// <para>text Keys 값을 보관합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Stores the text keys value.</para>
    /// \endif
    /// </summary>
    private readonly List<KeyButton> _textKeys = [];
    /// <summary>
    /// \if KO
    /// <para>composer 값을 보관합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Stores the composer value.</para>
    /// \endif
    /// </summary>
    private readonly HangulComposer _composer = new();
    /// <summary>
    /// \if KO
    /// <para>target 값을 보관합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Stores the target value.</para>
    /// \endif
    /// </summary>
    private Entry? _target;

    /// <summary>
    /// \if KO
    /// <para>shift 값을 보관합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Stores the shift value.</para>
    /// \endif
    /// </summary>
    private bool _shift;
    /// <summary>
    /// \if KO
    /// <para>caps Lock 값을 보관합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Stores the caps lock value.</para>
    /// \endif
    /// </summary>
    private bool _capsLock;
    /// <summary>
    /// \if KO
    /// <para>korean 값을 보관합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Stores the korean value.</para>
    /// \endif
    /// </summary>
    private bool _korean;
    /// <summary>
    /// \if KO
    /// <para>shift Button 값을 보관합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Stores the shift button value.</para>
    /// \endif
    /// </summary>
    private Button? _shiftButton;
    /// <summary>
    /// \if KO
    /// <para>caps Button 값을 보관합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Stores the caps button value.</para>
    /// \endif
    /// </summary>
    private Button? _capsButton;
    /// <summary>
    /// \if KO
    /// <para>language Button 값을 보관합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Stores the language button value.</para>
    /// \endif
    /// </summary>
    private Button? _languageButton;

    /// <summary>
    /// \if KO
    /// <para>키보드 UI를 만들고 초기 키 레이블과 상태를 적용합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Builds the keyboard UI and applies initial key labels and state.</para>
    /// \endif
    /// </summary>
    public DreamineVirtualKeyboard()
    {
        InitializeComponent();
        BuildKeyboard();
        RefreshKeys();
    }

    /// <summary>
    /// \if KO
    /// <para>가상 키보드 입력을 받을 <see cref="Entry"/>를 연결합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Attaches the <see cref="Entry"/> that receives virtual-keyboard input.</para>
    /// \endif
    /// </summary>
    /// <param name="entry">
    /// \if KO
    /// <para>편집하고 포커스 시 키보드를 표시할 입력 항목입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The input entry to edit and to show the keyboard for when focused.</para>
    /// \endif
    /// </param>
    /// <exception cref="NullReferenceException">
    /// \if KO
    /// <para><paramref name="entry"/>가 <see langword="null"/>일 때 발생합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Thrown when <paramref name="entry"/> is <see langword="null"/>.</para>
    /// \endif
    /// </exception>
    public void Attach(Entry entry)
    {
        _target = entry;
        entry.Focused += (_, _) => IsVisible = true;
    }

    /// <summary>
    /// \if KO
    /// <para>다섯 행의 텍스트 및 명령 키 사양을 구성하여 화면 키보드를 만듭니다.</para>
    /// \endif
    /// \if EN
    /// <para>Builds the on-screen keyboard from five rows of text-key and command-key specifications.</para>
    /// \endif
    /// </summary>
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

    /// <summary>
    /// \if KO
    /// <para>지정한 키 사양 목록을 하나의 키보드 행으로 만들어 호스트에 추가합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Creates one keyboard row from the specified key specifications and adds it to the host.</para>
    /// \endif
    /// </summary>
    /// <param name="specs">
    /// \if KO
    /// <para>행에 배치할 키 사양 목록입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The key specifications to place in the row.</para>
    /// \endif
    /// </param>
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

    /// <summary>
    /// \if KO
    /// <para>키 사양에 맞는 MAUI 버튼과 클릭 동작을 만듭니다.</para>
    /// \endif
    /// \if EN
    /// <para>Creates a MAUI button and click behavior for a key specification.</para>
    /// \endif
    /// </summary>
    /// <param name="spec">
    /// \if KO
    /// <para>만들 키의 종류, 레이블, 너비 및 동작입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The kind, label, width, and action of the key to create.</para>
    /// \endif
    /// </param>
    /// <returns>
    /// \if KO
    /// <para>구성된 키 버튼입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The configured key button.</para>
    /// \endif
    /// </returns>
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

    /// <summary>
    /// \if KO
    /// <para>현재 언어 및 수정 키 상태에 따라 텍스트 키를 입력합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Enters a text key according to the current language and modifier state.</para>
    /// \endif
    /// </summary>
    /// <param name="key">
    /// \if KO
    /// <para>입력할 기본 키 값입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The base key value to enter.</para>
    /// \endif
    /// </param>
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

    /// <summary>
    /// \if KO
    /// <para>한글 조합을 초기화하고 원시 텍스트를 현재 선택 또는 캐럿 위치에 삽입합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Resets Hangul composition and inserts raw text at the current selection or caret position.</para>
    /// \endif
    /// </summary>
    /// <param name="text">
    /// \if KO
    /// <para>삽입할 원시 텍스트입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The raw text to insert.</para>
    /// \endif
    /// </param>
    private void InsertRaw(string text)
    {
        if (_target is null)
            return;

        _composer.Reset();
        ReplaceTextTail(0, text);
    }

    /// <summary>
    /// \if KO
    /// <para>선택 영역을 교체하거나 캐럿 앞의 지정 문자 수를 새 텍스트로 교체합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Replaces the selection or a specified number of characters before the caret with new text.</para>
    /// \endif
    /// </summary>
    /// <param name="replaceCount">
    /// \if KO
    /// <para>선택 영역이 없을 때 캐럿 앞에서 교체할 문자 수입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The number of characters to replace before the caret when there is no selection.</para>
    /// \endif
    /// </param>
    /// <param name="text">
    /// \if KO
    /// <para>교체 위치에 삽입할 텍스트입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The text to insert at the replacement position.</para>
    /// \endif
    /// </param>
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

    /// <summary>
    /// \if KO
    /// <para>연결된 입력 항목에서 캐럿 앞의 텍스트를 가져옵니다.</para>
    /// \endif
    /// \if EN
    /// <para>Gets the text before the caret from the attached input entry.</para>
    /// \endif
    /// </summary>
    /// <returns>
    /// \if KO
    /// <para>캐럿 앞 텍스트이며 연결된 항목이 없으면 빈 문자열입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The text before the caret, or an empty string when no entry is attached.</para>
    /// \endif
    /// </returns>
    private string GetTextBeforeCaret()
    {
        if (_target is null)
            return string.Empty;

        var text = _target.Text ?? string.Empty;
        var caret = Math.Clamp(_target.CursorPosition, 0, text.Length);
        return text[..caret];
    }

    /// <summary>
    /// \if KO
    /// <para>현재 선택 영역 또는 캐럿 앞의 문자 하나를 삭제합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Deletes the current selection or one character before the caret.</para>
    /// \endif
    /// </summary>
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

    /// <summary>
    /// \if KO
    /// <para>Shift 상태를 전환하고 키 레이블을 갱신합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Toggles Shift state and refreshes key labels.</para>
    /// \endif
    /// </summary>
    private void ToggleShift()
    {
        _shift = !_shift;
        RefreshKeys();
    }

    /// <summary>
    /// \if KO
    /// <para>Caps Lock 상태를 전환하고 한글 조합과 키 레이블을 갱신합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Toggles Caps Lock and refreshes Hangul composition and key labels.</para>
    /// \endif
    /// </summary>
    private void ToggleCapsLock()
    {
        _capsLock = !_capsLock;
        _composer.Reset();
        RefreshKeys();
    }

    /// <summary>
    /// \if KO
    /// <para>영어와 한국어 입력 상태를 전환하고 한글 조합과 키 레이블을 갱신합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Toggles English and Korean input state and refreshes Hangul composition and key labels.</para>
    /// \endif
    /// </summary>
    private void ToggleLanguage()
    {
        _korean = !_korean;
        _composer.Reset();
        RefreshKeys();
    }

    /// <summary>
    /// \if KO
    /// <para>연결된 입력 항목의 캐럿을 왼쪽으로 한 칸 이동하고 선택을 지웁니다.</para>
    /// \endif
    /// \if EN
    /// <para>Moves the attached entry's caret one position left and clears the selection.</para>
    /// \endif
    /// </summary>
    private void MoveLeft()
    {
        if (_target is null)
            return;

        _target.CursorPosition = Math.Max(0, _target.CursorPosition - 1);
        _target.SelectionLength = 0;
    }

    /// <summary>
    /// \if KO
    /// <para>연결된 입력 항목의 캐럿을 오른쪽으로 한 칸 이동하고 선택을 지웁니다.</para>
    /// \endif
    /// \if EN
    /// <para>Moves the attached entry's caret one position right and clears the selection.</para>
    /// \endif
    /// </summary>
    private void MoveRight()
    {
        if (_target is null)
            return;

        _target.CursorPosition = Math.Min((_target.Text ?? string.Empty).Length, _target.CursorPosition + 1);
        _target.SelectionLength = 0;
    }

    /// <summary>
    /// \if KO
    /// <para>한글 조합을 초기화하고 화면 키보드를 숨깁니다.</para>
    /// \endif
    /// \if EN
    /// <para>Resets Hangul composition and hides the on-screen keyboard.</para>
    /// \endif
    /// </summary>
    private void HideKeyboard()
    {
        _composer.Reset();
        IsVisible = false;
    }

    /// <summary>
    /// \if KO
    /// <para>한 줄 입력을 완료하고 포커스를 해제한 뒤 키보드를 숨깁니다.</para>
    /// \endif
    /// \if EN
    /// <para>Completes single-line input, removes focus, and hides the keyboard.</para>
    /// \endif
    /// </summary>
    private void OnEnter()
    {
        _composer.Reset();
        _target?.Unfocus();
        IsVisible = false;
    }

    /// <summary>
    /// \if KO
    /// <para>현재 Shift, Caps Lock 및 언어 상태에 맞게 모든 키 레이블과 선택 색상을 갱신합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Refreshes all key labels and selected colors for the current Shift, Caps Lock, and language state.</para>
    /// \endif
    /// </summary>
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

    /// <summary>
    /// \if KO
    /// <para>현재 언어와 수정 키 상태에서 기본 키가 생성할 텍스트를 계산합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Computes the text produced by a base key under the current language and modifier state.</para>
    /// \endif
    /// </summary>
    /// <param name="key">
    /// \if KO
    /// <para>변환할 기본 키 값입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The base key value to convert.</para>
    /// \endif
    /// </param>
    /// <returns>
    /// \if KO
    /// <para>화면에 표시하고 입력할 변환된 키 텍스트입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The converted key text to display and enter.</para>
    /// \endif
    /// </returns>
    /// <exception cref="NullReferenceException">
    /// \if KO
    /// <para><paramref name="key"/>가 <see langword="null"/>일 때 발생합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Thrown when <paramref name="key"/> is <see langword="null"/>.</para>
    /// \endif
    /// </exception>
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

    /// <summary>
    /// \if KO
    /// <para>Shift Keys 값을 보관합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Stores the shift keys value.</para>
    /// \endif
    /// </summary>
    private static readonly Dictionary<string, string> ShiftKeys = new()
    {
        ["`"] = "~", ["1"] = "!", ["2"] = "@", ["3"] = "#", ["4"] = "$",
        ["5"] = "%", ["6"] = "^", ["7"] = "&", ["8"] = "*", ["9"] = "(",
        ["0"] = ")", ["-"] = "_", ["="] = "+", ["["] = "{", ["]"] = "}",
        ["\\"] = "|", [";"] = ":", ["'"] = "\"", [","] = "<", ["."] = ">",
        ["/"] = "?",
    };

    /// <summary>
    /// \if KO
    /// <para>Korean Keys 값을 보관합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Stores the korean keys value.</para>
    /// \endif
    /// </summary>
    private static readonly Dictionary<string, string> KoreanKeys = new()
    {
        ["q"] = "ㅂ", ["w"] = "ㅈ", ["e"] = "ㄷ", ["r"] = "ㄱ", ["t"] = "ㅅ",
        ["y"] = "ㅛ", ["u"] = "ㅕ", ["i"] = "ㅑ", ["o"] = "ㅐ", ["p"] = "ㅔ",
        ["a"] = "ㅁ", ["s"] = "ㄴ", ["d"] = "ㅇ", ["f"] = "ㄹ", ["g"] = "ㅎ",
        ["h"] = "ㅗ", ["j"] = "ㅓ", ["k"] = "ㅏ", ["l"] = "ㅣ",
        ["z"] = "ㅋ", ["x"] = "ㅌ", ["c"] = "ㅊ", ["v"] = "ㅍ", ["b"] = "ㅠ",
        ["n"] = "ㅜ", ["m"] = "ㅡ",
    };

    /// <summary>
    /// \if KO
    /// <para>Korean Shift Keys 값을 보관합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Stores the korean shift keys value.</para>
    /// \endif
    /// </summary>
    private static readonly Dictionary<string, string> KoreanShiftKeys = new()
    {
        ["q"] = "ㅃ", ["w"] = "ㅉ", ["e"] = "ㄸ", ["r"] = "ㄲ", ["t"] = "ㅆ",
        ["o"] = "ㅒ", ["p"] = "ㅖ",
    };

    /// <summary>
    /// \if KO
    /// <para>화면 버튼과 변환 전 기본 키 텍스트의 연결을 나타냅니다.</para>
    /// \endif
    /// \if EN
    /// <para>Represents the association between a visual button and its unconverted base-key text.</para>
    /// \endif
    /// </summary>
    /// <param name="Button">
    /// \if KO
    /// <para>화면에 표시된 MAUI 버튼입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The MAUI button displayed on screen.</para>
    /// \endif
    /// </param>
    /// <param name="BaseText">
    /// \if KO
    /// <para>언어 및 수정 키 변환 전 기본 텍스트입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The base text before language and modifier conversion.</para>
    /// \endif
    /// </param>
    private sealed record KeyButton(Button Button, string BaseText);

    /// <summary>
    /// \if KO
    /// <para>키보드 키의 종류, 레이블, 상대 너비 및 선택적 명령을 나타냅니다.</para>
    /// \endif
    /// \if EN
    /// <para>Represents a keyboard key's kind, label, relative width, and optional command.</para>
    /// \endif
    /// </summary>
    /// <param name="Kind">
    /// \if KO
    /// <para>텍스트 또는 명령 키 종류입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The text or command key kind.</para>
    /// \endif
    /// </param>
    /// <param name="Label">
    /// \if KO
    /// <para>키에 표시할 기본 레이블입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The base label displayed on the key.</para>
    /// \endif
    /// </param>
    /// <param name="Width">
    /// \if KO
    /// <para>행에서 사용할 상대 너비입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The relative width used within the row.</para>
    /// \endif
    /// </param>
    /// <param name="Action">
    /// \if KO
    /// <para>명령 키를 클릭할 때 실행할 선택적 동작입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The optional action invoked when a command key is clicked.</para>
    /// \endif
    /// </param>
    private sealed record KeySpec(KeyKind Kind, string Label, int Width, Action? Action)
    {
        /// <summary>
        /// \if KO
        /// <para>지정한 레이블과 너비로 텍스트 입력 키 사양을 만듭니다.</para>
        /// \endif
        /// \if EN
        /// <para>Creates a text-input key specification with the specified label and width.</para>
        /// \endif
        /// </summary>
        /// <param name="label">
        /// \if KO
        /// <para>키의 기본 레이블과 입력 값입니다.</para>
        /// \endif
        /// \if EN
        /// <para>The key's base label and input value.</para>
        /// \endif
        /// </param>
        /// <param name="width">
        /// \if KO
        /// <para>행에서 사용할 상대 너비입니다.</para>
        /// \endif
        /// \if EN
        /// <para>The relative width used within the row.</para>
        /// \endif
        /// </param>
        /// <returns>
        /// \if KO
        /// <para>새 텍스트 키 사양입니다.</para>
        /// \endif
        /// \if EN
        /// <para>A new text-key specification.</para>
        /// \endif
        /// </returns>
        public static KeySpec Text(string label, int width = 54) => new(KeyKind.Text, label, width, null);

        /// <summary>
        /// \if KO
        /// <para>지정한 레이블, 너비 및 동작으로 명령 키 사양을 만듭니다.</para>
        /// \endif
        /// \if EN
        /// <para>Creates a command-key specification with the specified label, width, and action.</para>
        /// \endif
        /// </summary>
        /// <param name="label">
        /// \if KO
        /// <para>키에 표시할 레이블입니다.</para>
        /// \endif
        /// \if EN
        /// <para>The label displayed on the key.</para>
        /// \endif
        /// </param>
        /// <param name="width">
        /// \if KO
        /// <para>행에서 사용할 상대 너비입니다.</para>
        /// \endif
        /// \if EN
        /// <para>The relative width used within the row.</para>
        /// \endif
        /// </param>
        /// <param name="action">
        /// \if KO
        /// <para>키를 클릭할 때 실행할 동작입니다.</para>
        /// \endif
        /// \if EN
        /// <para>The action invoked when the key is clicked.</para>
        /// \endif
        /// </param>
        /// <returns>
        /// \if KO
        /// <para>새 명령 키 사양입니다.</para>
        /// \endif
        /// \if EN
        /// <para>A new command-key specification.</para>
        /// \endif
        /// </returns>
        /// \fn KeySpec Command(string label, int width, Action action)
        public static KeySpec Command(string label, int width, Action action) => new(KeyKind.Command, label, width, action);
    }

    /// <summary>
    /// \if KO
    /// <para>가상 키가 직접 텍스트를 입력하는지 명령을 실행하는지 지정합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Specifies whether a virtual key enters text directly or executes a command.</para>
    /// \endif
    /// </summary>
    private enum KeyKind
    {
        /// <summary>
        /// \if KO
        /// <para>키가 변환된 텍스트를 입력함을 나타냅니다.</para>
        /// \endif
        /// \if EN
        /// <para>Indicates that the key enters converted text.</para>
        /// \endif
        /// </summary>
        Text,
        /// <summary>
        /// \if KO
        /// <para>키가 연결된 동작을 실행함을 나타냅니다.</para>
        /// \endif
        /// \if EN
        /// <para>Indicates that the key executes an associated action.</para>
        /// \endif
        /// </summary>
        Command
    }
}
