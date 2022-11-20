namespace WhatsStatusApp.ViewModels;
internal partial class MainPageViewModel : BaseViewModel
{
    public int StatusTextLimit { get; } = 999;
    readonly List<Color> backgroudColors = LoadStatusBackgroundColors();
    readonly List<string> fonts = LoadStatusFonts();
    readonly Random random = new();
    string _StatusText = string.Empty;

    [ObservableProperty] string _StatusTextFamily, _StatusFont;
    [ObservableProperty] Color _StatusBackgroundColor;
    [ObservableProperty] TextTransform _StatusTextTransform = TextTransform.None;

    public string StatusText
    {
        get => _StatusText;
        set
        {
            SetProperty(ref _StatusText, value);
            CheckStatusTextLength();
        }
    }

    public ICommand ChangeStatusTextTransformCommand => new Command(SetStatusTextTransform);
    public ICommand ChangeStatusFontCommand => new Command(SetRandomStatusFont);
    public ICommand ChangeStatusBackgroundColorCommand => new Command(SetRandomStatusBackgroundColor);

    public MainPageViewModel()
    {
        ClosePageCommand = new Command(async () => await OnBackButtonPressed());
        SetRandomStatusFont();
        SetRandomStatusBackgroundColor();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<bool> OnBackButtonPressed()
    {
        if (!string.IsNullOrWhiteSpace(StatusText))
        {
            var ans = await Shell.Current.DisplayAlert("", "Discard text?", "Discard", "Cancel");
            return ans;
        }
        return true;
    }

    void CheckStatusTextLength()
    {
        if (StatusText.Length >= StatusTextLimit)
            Shell.Current.DisplayAlert("Input Limit", $"Your status cannot exceed {StatusTextLimit} characters.", "OK").Wait(300);
    }

    static List<Color> LoadStatusBackgroundColors()
    {
        return new List<Color>()
        {
            Color.FromArgb("2980B9"),
            Color.FromArgb("27AE60"),
            Color.FromArgb("D35400"),
            Color.FromArgb("2d3436"),
            Color.FromArgb("FFC107"),
            Color.FromArgb("FF4081"),
            Color.FromArgb("2d3436"),
            Color.FromArgb("512BD4"),
        };
    }

    static List<string> LoadStatusFonts()
    {
        return new List<string>()
        {
            "OpenSansRegular",
            "OpenSansSemibold",
            "DancingScriptRegular",
            "IndieFlowerRegular",
            "PacificoRegular",
            "PlayfairDisplayRegular",
            "RubikDistressedRegular",
            "UbuntuRegular",
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="Exception"></exception>
    void SetStatusTextTransform()
    {
        StatusTextTransform = StatusTextTransform switch
        {
            TextTransform.None => TextTransform.Lowercase,
            TextTransform.Default => TextTransform.Lowercase,
            TextTransform.Lowercase => TextTransform.Uppercase,
            TextTransform.Uppercase => TextTransform.Default,
            _ => throw new Exception("TextTronsform threw an unexpected exception"),
        };
        Console.WriteLine(StatusText);
    }

    void SetRandomStatusFont()
        => StatusFont = fonts[random.Next(fonts.Count)];

    void SetRandomStatusBackgroundColor()
        => StatusBackgroundColor = backgroudColors[random.Next(backgroudColors.Count)];
}
