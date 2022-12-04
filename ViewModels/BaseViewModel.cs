using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Text.RegularExpressions;

namespace WhatsStatusApp.ViewModels;

[INotifyPropertyChanged]
public partial class BaseViewModel
{
    public static readonly Random random = new();
    public static readonly Regex linkParser = RegexLinkParser();

	#region ObservableProperties
	[ObservableProperty] bool _IsBusy, _HideNavBar;
	[ObservableProperty] int _LinksCount;
	#endregion

	#region ICommand
	public ICommand ClosePageCommand => new Command(async () => await OnBackButtonPressed());
	#endregion

	public virtual async Task OnBackButtonPressed()
    {
		await ShellGoToAsync("..");
    }

	public static void CheckConnection()
    {
        if (Connectivity.NetworkAccess is NetworkAccess.Internet)
            return;
        throw new Exception("No internet connection");
    }

    protected async Task RunTryCatchAsync(Func<Task> func)
    {
		if (IsBusy)
			return;
		IsBusy = true;
		try
		{
			await func();
		}
		catch (Exception x)
		{

/* Unmerged change from project 'WhatsStatusApp (net7.0-windows10.0.19041.0)'
Before:
			DisplayAlert("", x.Message);
After:
            BaseViewModel.DisplayAlert("", x.Message);
*/

/* Unmerged change from project 'WhatsStatusApp (net7.0-ios)'
Before:
			DisplayAlert("", x.Message);
After:
            BaseViewModel.DisplayAlert("", x.Message);
*/

/* Unmerged change from project 'WhatsStatusApp (net7.0-maccatalyst)'
Before:
			DisplayAlert("", x.Message);
After:
            BaseViewModel.DisplayAlert("", x.Message);
*/
            DisplayAlert("", x.Message);
		}
		finally
		{
			await Task.Delay(150);
			IsBusy = false;
		}
    }

	#region Alerts
	public static void DisplayAlert(string title, string message)
	{
		Shell.Current.DisplayAlert(title, message, "OK").Wait(250);
	}

	public static async Task<bool> DisplayAlert(string title, string message, string accept, string cancel = "Cancel")
	{
		return await Shell.Current.DisplayAlert(title, message, accept, cancel);
	}

	public static void MakeToast(string message, ToastDuration duration = ToastDuration.Short)
	{
		CancellationToken cancellationToken = new();
		Toast.Make(message, duration).Show(cancellationToken);
	}

	// Check out: https://learn.microsoft.com/en-us/dotnet/communitytoolkit/maui/alerts/snackbar?tabs=ios
	/*public void MakeSnackBar(string message, ToastDuration duration = ToastDuration.Short)
	{
		CancellationToken cancellationToken = new();
		var snackbaroptions = new SnackbarOptions
		{
			BackgroundColor = Color.FromArgb("#228C22"),
			CornerRadius = 5,
			ActionButtonTextColor = Colors.WhiteSmoke,
		};

		Snackbar.Make(message, actionButtonText:"Ok", action: async () =>
		{

		}).Show(cancellationToken);
	}*/
	#endregion

	#region Shell Navigation
	[RelayCommand]
    public async Task RunIsBusy_ShellGoToAsync(string page)
	{
		await RunTryCatchAsync(async ()
			=> await ShellGoToAsync(page));
	}

    public static async Task ShellGoToAsync(ShellNavigationState page)
	{
		await Shell.Current.GoToAsync(page);
	}

	public static async Task ShellGoToAsync(ShellNavigationState page, IDictionary<string, object> dictionary, bool animate = true)
	{
		await Shell.Current.GoToAsync(page, animate, dictionary);
	}
	#endregion

	#region Status Links
	public void GetLinksCount(string text) => LinksCount = linkParser.Matches(text).Count;
	#endregion

	[GeneratedRegex("\\b(?:https?://|www\\.)\\S+\\b", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace, "en-NA")]
    private static partial Regex RegexLinkParser();
}
