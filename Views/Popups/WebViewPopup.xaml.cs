using CommunityToolkit.Maui.Views;

namespace WhatsStatusApp.Views.Popups;

public partial class WebViewPopup : Popup
{
    string url;
	public WebViewPopup(Link link)
    {
        InitializeComponent();

        DeviceDisplay.MainDisplayInfoChanged += DeviceDisplay_MainDisplayInfoChanged;

        webview.Navigating += Webview_Navigating;

        GetDeviceSize(DeviceDisplay.MainDisplayInfo);

        webview.Source = link.URL;
    }

    private void DeviceDisplay_MainDisplayInfoChanged(object sender, DisplayInfoChangedEventArgs e)
        => GetDeviceSize(e.DisplayInfo);

    private void Webview_Navigating(object sender, WebNavigatingEventArgs e)
        => url = e.Url;

    private void GetDeviceSize(DisplayInfo mainDisplayInfo) 
        => popup.Size = new Size(mainDisplayInfo.Width / 2, mainDisplayInfo.Height / 2.2);

    private void WebViewGoBack_Tapped(object sender, TappedEventArgs e)
    {
        if(webview.CanGoBack)
            webview.GoBack();
    }

    private void WebViewGoForward_Tapped(object sender, TappedEventArgs e)
    {
        if(webview.CanGoForward)
            webview.GoForward();
    }

    private void WebViewReload_Tapped(object sender, TappedEventArgs e)
        => webview.Reload();

    private void WebViewDismiss_Tapped(object sender, TappedEventArgs e)
        => Close();

    private async void WebViewLaunchBrowser_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            await Launcher.OpenAsync(url);
            Close();
        }
        catch (Exception x)
        {
            await Shell.Current.DisplayAlert("", x.Message, "Ok");
        }
    }
}