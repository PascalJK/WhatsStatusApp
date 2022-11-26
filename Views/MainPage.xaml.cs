namespace WhatsStatusApp.Views;

public partial class MainPage : ContentPage
{
	readonly MainPageViewModel vm = new();

	public MainPage()
	{
		InitializeComponent();
		BindingContext = vm;
	}

	protected override bool OnBackButtonPressed()
	{
		if (vm.ShowStatusEditor)
		{
			Shell.Current.Dispatcher.Dispatch(async () 
				=> await vm.OnBackButtonPressed());
			return true;
		}
		else
			return false;
	}
}

