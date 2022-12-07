namespace WhatsStatusApp.Views;

public partial class MainPage : ContentPage
{
	readonly MainPageViewModel vm;

	public MainPage(MainPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		vm = viewModel;
    }

    /// <summary>
    /// Once the page sizechanges the window is updated so once a
    /// popup is loaded or active it resizes to match window size.
    /// </summary>
    private void MainPage_SizeChanged(object sender, EventArgs e)
    {
        var g = (ContentPage)sender;
		App.mobileWindow = g.Window;
        WeakReferenceMessenger.Default.Send(App.mobileWindow);
    }

    protected override bool OnBackButtonPressed()
	{
		if (vm.ShowStatusEditor)
		{
			Shell.Current.Dispatcher.Dispatch(async () 
				=> await vm.OnBackButtonPressed());
			return true;
		}
		return false;
	}
}

