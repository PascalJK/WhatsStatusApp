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
		Shell.Current.Dispatcher.Dispatch(async () =>
		{
			var ans = await vm.OnBackButtonPressed();
			if (ans)
				Application.Current.Quit();
			// Todo Confrim Quit b4 Quit.
		});
		return true;
	}
}

