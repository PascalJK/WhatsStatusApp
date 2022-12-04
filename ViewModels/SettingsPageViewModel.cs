namespace WhatsStatusApp.ViewModels;

public partial class SettingsPageViewModel : BaseViewModel
{
    [ObservableProperty] string _appName = AppInfo.Current.Name;
    [ObservableProperty] string _appVersion = AppInfo.Current.VersionString;

    [RelayCommand]
    async Task ClearDatabaseAsync()
    {
        await RunTryCatchAsync(async () =>
        {
            var ans = await DisplayAlert("Warning", "You are about to clear all existing Status data, this process is irreversible!\nProceed?", "Yes");
            if (!ans)
                return;

            //await LocalDatabaseService.LocalDB.ClearDatabaseAsync();
        });
        /// gen 4-5 nums a user shoud re-enter
        /// also clear prefs data
        /// if matches the clear data from db and refresh home data.
    }
}
