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

