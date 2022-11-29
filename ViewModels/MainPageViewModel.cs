using System.Collections.ObjectModel;

namespace WhatsStatusApp.ViewModels;
public partial class MainPageViewModel : BaseViewModel
{
    #region readonly Fields
    readonly int textMaxLength = 700;
    readonly List<Color> backgroudColors = LoadStatusBackgroundColors();
    readonly List<string> fonts = LoadStatusFonts();
    readonly Random random = new();
    #endregion

    #region ObservableProperties
    [ObservableProperty] string _StatusFont;
    [ObservableProperty] int _StatusTextMaxLength = int.MaxValue;
    [ObservableProperty] double _StatusFontSize = Preferences.Get("fontsize", 35.0);
    [ObservableProperty] Color _StatusBackgroundColor;
    [ObservableProperty] bool _ShowStatusEditor, _ShowStatusFontEditor, _IsStatusListEmpty, _IsTextLimited;
    [ObservableProperty] ObservableCollection<Status> _StatusCollection = new();
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
    }

    private async Task LoadStatusListDataAsync()
    {
        IsBusy = true;
        await GetSavedStatusListAsync();
        await Task.Delay(500);
        IsBusy = false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
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

    void CheckStatusTextLength()
    {
        if (IsTextLimited && StatusText.Length >= textMaxLength)
            Shell.Current.DisplayAlert("Input Limit", $"Your status cannot exceed {textMaxLength} characters.", "OK").Wait(300);
    }

    #region StatusEditor
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

    #region Status Details
    [RelayCommand]
    static async Task LoadStatusDetailAsync(Status status)
    {
        await ShellGoToAsync(nameof(DetailsPage), new Dictionary<string, object> { { nameof(Status), status } });
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
            Preferences.Set("fontsize", StatusFontSize);
            await GetSavedStatusListAsync();
            MakeToast("Status Added.");
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
}
