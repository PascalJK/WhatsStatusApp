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
}