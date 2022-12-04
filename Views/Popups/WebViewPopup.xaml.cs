using CommunityToolkit.Maui.Views;

namespace WhatsStatusApp.Views.Popups;

public partial class WebViewPopup : Popup
{
    string url;
	public WebViewPopup(Link link)
    {
        InitializeComponent();

        WeakReferenceMessenger.Default.Register<DisplayInfoChangedEventArgs>(this, (r, m)
            => GetDeviceSize());

        webview.Navigating += Webview_Navigating;

        GetDeviceSize();

        webview.Source = link.URL;
    }

    private void GetDeviceSize()
        => popup.Size = App.GetDeviceSize();

    #region Webview
    private void Webview_Navigating(object sender, WebNavigatingEventArgs e)
    => url = e.Url;

    private void WebViewGoBack_Tapped(object sender, TappedEventArgs e)
    {
        if (webview.CanGoBack)
            webview.GoBack();
    }

    private void WebViewGoForward_Tapped(object sender, TappedEventArgs e)
    {
        if (webview.CanGoForward)
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
    #endregion

    private void Popup_Closed(object sender, CommunityToolkit.Maui.Core.PopupClosedEventArgs e)
    {
        WeakReferenceMessenger.Default.Unregister<DisplayInfoChangedEventArgs>(this);
    }
}