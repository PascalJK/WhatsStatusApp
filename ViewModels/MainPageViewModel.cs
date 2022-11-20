namespace WhatsStatusApp.ViewModels;
internal partial class MainPageViewModel : BaseViewModel
{
    readonly List<Color> backgroudColors;
    readonly Random random = new();

    [ObservableProperty] bool _IsAllCaps;
    [ObservableProperty] string _StatusText;
    [ObservableProperty] string _StatusFont;
    [ObservableProperty] Color _StatusBackgroundColor;

    public ICommand ChangeStatusBackgroundColorCommand => new Command(SetRandomStatusBackgroundColor);

    public MainPageViewModel()
    {
        backgroudColors = LoadStatusBackgroundColors();
        SetRandomStatusBackgroundColor();
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

    void SetRandomStatusBackgroundColor()
        => StatusBackgroundColor = backgroudColors[random.Next(backgroudColors.Count)];
}
