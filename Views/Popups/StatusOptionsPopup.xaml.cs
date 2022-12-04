using CommunityToolkit.Maui.Views;

namespace WhatsStatusApp.Views.Popups;

public partial class StatusOptionsPopup : Popup
{
	public StatusOptionsPopup()
    {
        InitializeComponent();
    }

    private void DeleteStatus_Tapped(object sender, TappedEventArgs e)
        => Close("delete");

    private void ShareStatus_Tapped(object sender, TappedEventArgs e)
        => Close("share");

    private void ViewLinksStatus_Tapped(object sender, TappedEventArgs e)
        => Close("links");

    private void Button_Clicked(object sender, EventArgs e)
        => Close();
}