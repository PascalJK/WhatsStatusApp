namespace WhatsStatusApp.ViewModels;

public partial class SettingsPageViewModel : BaseViewModel
{
    #region ObservableProperty
    [ObservableProperty] string _appName = AppInfo.Current.Name;
    [ObservableProperty] string _appVersion = AppInfo.Current.VersionString;
    [ObservableProperty] int _OpenedCount, _DeletedCount, _LinksVisitedCount;
    #endregion

    public SettingsPageViewModel()
    {
        GetStatusStats();
    }

    void GetStatusStats()
    {
        OpenedCount = Preferences.Get("status_opened", 0);
        DeletedCount = Preferences.Get("status_deleted", 0);
        LinksVisitedCount = Preferences.Get("status_links_visited", 0);
    }

    [RelayCommand]
    async Task ClearDatabaseAsync()
    {
        await RunTryCatchAsync(async () =>
        {
            var ans = await DisplayAlert("Warning!", "You are about to clear all existing status data\n[Note]this process is irreversible..!\nProceed..?", "Yes");
            if (!ans)
                return;

            var numbers = random.Next(10001, 99999).ToString();

            string result = await Shell.Current.DisplayPromptAsync("Confirmation"
                , $"Please re-enter the numbers [{numbers}] below to confirm your action", keyboard: Keyboard.Numeric, placeholder: "enter numbers here");

            if (string.IsNullOrWhiteSpace(result))
                throw new Exception("Empty field not accepted");

            if (!numbers.Equals(result?.Trim()))
                throw new Exception("Numbers entered don't match generated numbers");

            Preferences.Clear();
            GetStatusStats();
            await LocalDatabaseService.LocalDB.ClearDatabaseAsync();
            MakeToast("Status data cleared");
            WeakReferenceMessenger.Default.Send(this);
        });
    }
}
