using CommunityToolkit.Maui.Views;
using WhatsStatusApp.Views.Popups;

namespace WhatsStatusApp.ViewModels;

public partial class MainPageViewModel : BaseViewModel
{
    public ObservableRangeCollection<StatusGroup> StatusGroupCollection { get; set; } = new();

    #region readonly Fields
    readonly int textMaxLength = 700;
    readonly List<Color> backgroudColors = LoadStatusBackgroundColors();
    readonly List<string> fonts = LoadStatusFonts();
    #endregion

    #region ObservableProperties
    [ObservableProperty] string _StatusFont;
    [ObservableProperty] int _StatusTextMaxLength = int.MaxValue;
    [ObservableProperty] double _StatusFontSize = Preferences.Get("fontsize", 35.0);
    [ObservableProperty] Color _StatusBackgroundColor;
    [ObservableProperty] bool _ShowStatusEditor, _ShowStatusFontEditor, _IsStatusListEmpty, _IsTextLimited;
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
        }
    }
    #endregion

    public MainPageViewModel()
    {
        Shell.Current.Dispatcher.Dispatch(async () 
            => await LoadStatusListDataAsync());

        WeakReferenceMessenger.Default.Register<DetailsViewModel>(this, async (r, m) 
            => await LoadStatusListDataAsync());

        WeakReferenceMessenger.Default.Register<SettingsPageViewModel>(this, async (r, m) 
            => await GetSavedStatusListAsync());
    }

    private async Task LoadStatusListDataAsync()
    {
        IsBusy = true;
        await GetSavedStatusListAsync();
        await Task.Delay(500);
        IsBusy = false;
    }

    public override async Task OnBackButtonPressed()
    {
        if (!string.IsNullOrWhiteSpace(StatusText))
        {
            var ans = await Shell.Current.DisplayAlert("", "Discard text?", "Discard", "Cancel");
            if (!ans)
                return;
        }
        DismissStatusEditor();
    }

    #region StatusEditor
    void CheckStatusTextLength()
    {
        if (IsTextLimited && StatusText.Length >= textMaxLength)
            Shell.Current.DisplayAlert("Input Limit", $"Your status cannot exceed {textMaxLength} characters.", "OK").Wait(300);
    }

    void DismissStatusEditor()
    {
        StatusText = string.Empty;
        ShowStatusEditor = false;
        ShowStatusFontEditor = false;
        IsTextLimited = false;
        StatusTextMaxLength = int.MaxValue;
    }

    [RelayCommand]
    void LoadStatusEditor()
    {
        SetRandomStatusFont();
        SetRandomStatusBackgroundColor();
        ShowStatusEditor = true;
    }

    [RelayCommand]
    void ShowStatusFontEditorView()
        => ShowStatusFontEditor = !ShowStatusFontEditor;

    [RelayCommand]
    void SetRandomStatusFont()
        => StatusFont = fonts[random.Next(fonts.Count)];

    [RelayCommand]
    void SetRandomStatusBackgroundColor()
        => StatusBackgroundColor = backgroudColors[random.Next(backgroudColors.Count)];

    [RelayCommand]
    async Task StatusTextLimit()
    {
        await RunTryCatchAsync(async () =>
        {
            if (!IsTextLimited)
            {
                if (StatusText.Length > textMaxLength)
                    throw new Exception($"could not set limit since status text length({StatusText.Length}) is already over the set limit({textMaxLength})");
                StatusTextMaxLength = textMaxLength;
                IsTextLimited = true;
                return;
            }
            StatusTextMaxLength = int.MaxValue;
            IsTextLimited = false;
            await Task.CompletedTask;
        });
    }

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
            Color.FromArgb("810CA8"),
            Color.FromArgb("10A19D"),
            Color.FromArgb("512BD4"),
            Color.FromArgb("9C254D"),
            Color.FromArgb("735F32"),
            Color.FromArgb("CF0A0A"),
            Color.FromArgb("B2B2B2"),
            Color.FromArgb("647E68"),
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
            "TekoRegular",
            "AcmeRegular",
            "PermanentMarkerRegular",
        };
    }
    #endregion
    #endregion

    #region Status Details
    /// <summary>
    /// Navigates user to the Status Details Page along with status param data.
    /// </summary>
    /// <param name="status"></param>
    [RelayCommand]
    async Task LoadStatusDetailAsync(Status status)
        => await RunTryCatchAsync(async ()
            => await ShellGoToAsync(nameof(DetailsPage), new Dictionary<string, object> { { nameof(Status), status } }));

    /// <summary>
    /// Shows a popup with all status data grouped by date
    /// </summary>
    /// <param name="statuses"></param>
    [RelayCommand]
    async Task LoadStatusGroupData(StatusGroup statuses)
        => await RunTryCatchAsync(async ()
            => await Shell.Current.ShowPopupAsync(new StatusCarouselPopup(statuses)));
    #endregion

    [RelayCommand]
    async Task SaveStatusAsync()
    {
        await RunTryCatchAsync(async () =>
        {
            if (string.IsNullOrWhiteSpace(StatusText))
                throw new Exception("cannot save blank text");

            await LocalDatabaseService.LocalDB.AddNewStatusAsync(new Status().CreateNewStatusModel(this));
            Preferences.Set("fontsize", StatusFontSize);
            await GetSavedStatusListAsync();
            MakeToast("Status Added.");
            DismissStatusEditor();
        });
    }

    [RelayCommand]
    async Task GetSavedStatusListAsync()
    {
        List<StatusGroup> statusGroupCollection = new();

        var list = await LocalDatabaseService.LocalDB.GetStatusListAsync();
        var groups = list?.GroupBy(s => s.DateCreated.Date).OrderByDescending(s => s.Key);

        foreach (var l2 in groups)
            statusGroupCollection.Add(new StatusGroup(l2.Key, l2.OrderByDescending(s => s.DateCreated).ToList()));

        StatusGroupCollection.ReplaceRange(statusGroupCollection);
        IsStatusListEmpty = list.Count <= 0;
    }
}
