using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace WhatsStatusApp.ViewModels;
internal partial class MainPageViewModel : BaseViewModel
{
    #region readonly Fields
    public int StatusTextLimit { get; } = 999;
    readonly List<Color> backgroudColors = LoadStatusBackgroundColors();
    readonly List<string> fonts = LoadStatusFonts();
    readonly Regex linkParser;
    readonly Random random = new();
    #endregion

    #region Props
    public List<Link> Links { get; set; } = new();
    public ObservableCollection<Link> LinksCollection { get; set; } = new();
    #endregion

    #region ObservableProperties
    [ObservableProperty] string _StatusTextFamily, _StatusFont;
    [ObservableProperty] Color _StatusBackgroundColor;
    [ObservableProperty] int _LinksCount;
    [ObservableProperty] TextTransform _StatusTextTransform = TextTransform.None;
    #endregion

    #region Full Props
    string _StatusText = string.Empty;
    public string StatusText
    {
        get => _StatusText;
        set
        {
            SetProperty(ref _StatusText, value);
            CheckStatusTextLength();
            GetLinksCount();
        }
    }
    #endregion

    #region ICommands
    public ICommand ChangeStatusTextTransformCommand => new Command(SetStatusTextTransform);
    public ICommand ChangeStatusFontCommand => new Command(SetRandomStatusFont);
    public ICommand ChangeStatusBackgroundColorCommand => new Command(SetRandomStatusBackgroundColor);
    public ICommand GetLinksCommand => new Command(CreateLinksPreview);
    public ICommand SaveStatusCommand => new Command(async () => await SaveStatusAsync()); 
    #endregion

    public MainPageViewModel()
    {
        ClosePageCommand = new Command(async () => await OnBackButtonPressed());
        linkParser = RegexLinkParser();
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

    void GetLinksCount() 
        => LinksCount = linkParser.Matches(StatusText).Count;

    async void CreateLinksPreview()
    {
        LinksCollection.Clear();
        var links = linkParser.Matches(StatusText);

        foreach (var item in links)
        {
            string url = item.ToString();
            var link = Links.FirstOrDefault(element => element.URL == url);
            link ??= await GetLinkData(url);
            if (link != null)
                LinksCollection.Add(link);
        }
        Links = LinksCollection.ToList();
    }

    private static async Task<Link> GetLinkData(string url)
    {
        try
        { 
            var graph = await OpenGraph.ParseUrlAsync(url);
            return new Link().DecodeMetaInformation(graph);
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    private async Task SaveStatusAsync()
    {
        await RunTryCatchAsync(async () =>
        {
            if (string.IsNullOrWhiteSpace(StatusText))
                throw new Exception("cannot save blank text");

            await LocalDatabaseService.LocalDB.AddNewStatusAsync(new Status().CreateNewStatusModel(this));
        });
    }


    [GeneratedRegex("\\b(?:https?://|www\\.)\\S+\\b", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace, "en-NA")]
    private static partial Regex RegexLinkParser();
}
