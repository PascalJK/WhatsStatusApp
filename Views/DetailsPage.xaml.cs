namespace WhatsStatusApp.Views;

public partial class DetailsPage : ContentPage
{
	private readonly DetailsViewModel vm;

	public DetailsPage(DetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		vm = viewModel;
        Preferences.Set("status_opened", Preferences.Get("status_opened", 0) + 1);
    }

	protected override bool OnBackButtonPressed()
	{
		vm.ClosePageCommand.Execute(null);
		return true;
	}
}