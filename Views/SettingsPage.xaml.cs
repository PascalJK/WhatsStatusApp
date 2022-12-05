namespace WhatsStatusApp.Views;

public partial class SettingsPage : ContentPage
{
	readonly SettingsPageViewModel vm;

	public SettingsPage(SettingsPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		vm = viewModel;
	}

	protected override bool OnBackButtonPressed()
	{
		vm.ClosePageCommand.Execute(null);
		return true;
	}
}