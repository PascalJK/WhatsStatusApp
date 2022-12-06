using CommunityToolkit.Maui.Views;

namespace WhatsStatusApp.Views.Popups;

public partial class StatusCarouselPopup : Popup
{
	public StatusCarouselPopup(StatusGroup statuses)
	{
		InitializeComponent();

        WeakReferenceMessenger.Default.Register<DisplayInfoChangedEventArgs>(this, (r, m)
            => GetDeviceSize());

        GetDeviceSize();

        carouselView.ItemsSource = statuses;
        carouselView.IsUserInteractionEnabled = statuses.Count > 1;
    }

    private void GetDeviceSize()
        => popup.Size = App.GetDeviceSize();

    private void Popup_Closed(object sender, CommunityToolkit.Maui.Core.PopupClosedEventArgs e)
    {
        WeakReferenceMessenger.Default.Unregister<DisplayInfoChangedEventArgs>(this);
    }

    private void ClosePopup_Tapped(object sender, TappedEventArgs e) 
        => Close();
}