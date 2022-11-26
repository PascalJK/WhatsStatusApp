using CommunityToolkit.Mvvm.Input;
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
    [ObservableProperty] string _StatusFont;
    [ObservableProperty] Color _StatusBackgroundColor;
    [ObservableProperty] int _LinksCount;
    [ObservableProperty] bool _ShowStatusEditor, _IsStatusListEmpty;
    [ObservableProperty] ObservableCollection<Status> _StatusCollection = new();
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

    public MainPageViewModel()
    {
        Shell.Current.Dispatcher.Dispatch(async () =>
        {
            await GetSavedStatusListAsync();
        });
        linkParser = RegexLinkParser();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    public async Task OnBackButtonPressed()
    {
        if (!string.IsNullOrWhiteSpace(StatusText))
        {
            var ans = await Shell.Current.DisplayAlert("", "Discard text?", "Discard", "Cancel");
            if (!ans)
                return;
        }
        DismissStatusEditor();
    }

    void CheckStatusTextLength()
    {
        if (StatusText.Length >= StatusTextLimit)
            Shell.Current.DisplayAlert("Input Limit", $"Your status cannot exceed {StatusTextLimit} characters.", "OK").Wait(300);
    }

    #region StatusEditor
    void DismissStatusEditor()
    {
        StatusText = string.Empty;
        ShowStatusEditor = false;
    }

    [RelayCommand]
    void LoadStatusEditor()
    {
        SetRandomStatusFont();
        SetRandomStatusBackgroundColor();
        ShowStatusEditor = true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="Exception"></exception>
    [RelayCommand]
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

    [RelayCommand]
    void SetRandomStatusFont()
        => StatusFont = fonts[random.Next(fonts.Count)];

    [RelayCommand]
    void SetRandomStatusBackgroundColor()
        => StatusBackgroundColor = backgroudColors[random.Next(backgroudColors.Count)];

    #region Static Status Properties
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
    #endregion
    #endregion

    #region Status Link
    void GetLinksCount() 
        => LinksCount = linkParser.Matches(StatusText).Count;

    [RelayCommand]
    async Task PreviewLinksAsync()
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
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    } 
    #endregion

    [RelayCommand]
    async Task SaveStatusAsync()
    {
        await RunTryCatchAsync(async () =>
        {
            if (string.IsNullOrWhiteSpace(StatusText))
                throw new Exception("cannot save blank text");

            await LocalDatabaseService.LocalDB.AddNewStatusAsync(new Status().CreateNewStatusModel(this));
            await GetSavedStatusListAsync();
            DismissStatusEditor();
        });
    }
    [RelayCommand]
    async Task GetSavedStatusListAsync()
    {
        StatusCollection.Clear();
        var list = await LocalDatabaseService.LocalDB.GetStatusListAsync();
        IsStatusListEmpty = list.Count <= 0;
        foreach (var s in list)
        {
            s.SetStatusBackGroundColor();
            StatusCollection.Add(s);
        }
    }


    [GeneratedRegex("\\b(?:https?://|www\\.)\\S+\\b", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace, "en-NA")]
    private static partial Regex RegexLinkParser();
}
