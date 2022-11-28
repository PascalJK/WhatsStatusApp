namespace WhatsStatusApp.ViewModels;

[INotifyPropertyChanged]
public partial class BaseViewModel
{
    [ObservableProperty] bool _IsBusy;
    [ObservableProperty] bool _HideNavBar;

	public ICommand ClosePageCommand 
		=> new Command(async () => await OnBackButtonPressed());

    public virtual async Task OnBackButtonPressed()
    {
		await ShellGoToAsync("..");
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
			await Shell.Current.DisplayAlert("", x.Message, "OK");
		}
		finally
		{
			await Task.Delay(150);
			IsBusy = false;
		}
    }

	public static async Task ShellGoToAsync(ShellNavigationState page)
	{
		await Shell.Current.GoToAsync(page);
	}

	public static async Task ShellGoToAsync(ShellNavigationState page, IDictionary<string, object> dictionary, bool animate = true)
	{
		await Shell.Current.GoToAsync(page, animate, dictionary);
	}
}
