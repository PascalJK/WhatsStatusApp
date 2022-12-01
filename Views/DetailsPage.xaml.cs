namespace WhatsStatusApp.Views;

public partial class DetailsPage : ContentPage
{
	private readonly DetailsViewModel vm;

	public DetailsPage(DetailsViewModel viewModel)
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