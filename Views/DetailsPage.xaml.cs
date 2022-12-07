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

	/// <summary>
	/// Once the page sizechanges the window is updated so once a
	/// popup is loaded or active it resizes to match window size.
	/// </summary>
    private void DetailsPage_SizeChanged(object sender, EventArgs e)
    {
		if(vm.HideNavBar)
			return;

        var g = (ContentPage)sender;
        App.mobileWindow = g.Window;
        WeakReferenceMessenger.Default.Send(App.mobileWindow);
    }

    protected override bool OnBackButtonPressed()
	{
		vm.ClosePageCommand.Execute(null);
		return true;
	}
}