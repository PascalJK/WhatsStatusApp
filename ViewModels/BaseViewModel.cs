namespace WhatsStatusApp.ViewModels;

[INotifyPropertyChanged]
public partial class BaseViewModel
{
    [ObservableProperty] bool _IsBusy;

    public ICommand ClosePageCommand { get; set; }

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
}
