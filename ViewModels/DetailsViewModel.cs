using CommunityToolkit.Maui.Views;
using WhatsStatusApp.Views.Popups;

namespace WhatsStatusApp.ViewModels;

[QueryProperty(nameof(StatusModel), nameof(Status))]
public partial class DetailsViewModel : BaseViewModel
{
    #region Props
    public List<Link> Links { get; set; } = new();
    public ObservableRangeCollection<Link> LinksCollection { get; set; } = new();
    #endregion

    #region ObservableProperty
    [ObservableProperty] Status _StatusModel;
    [ObservableProperty] bool _ShowUrlListView;
    #endregion

    [RelayCommand]
    async Task OpenOptionsPopupAsync()
    {
        if (ShowUrlListView)
            return;

        await RunTryCatchAsync(async () =>
        {
            HideNavBar = true;
            var option = await Shell.Current.ShowPopupAsync(new StatusOptionsPopup());

            switch ((string)option)
            {
                case "delete":
                    await DeleteStatusAsync();
                    break;
                case "share":
                    await ShareStatusTextAsync();
                    break;
                case "links":
                    await PreviewLinksAsync();
                    break;
                default:
                    break;
            }
        });
        HideNavBar = false;
    }

    #region Options Popup Actions
    async Task DeleteStatusAsync()
    {
        var ans = await DisplayAlert("", "Delete Status?", "Yes");
        if (!ans)
            return;
        await LocalDatabaseService.LocalDB.DeleteStatusAsync(StatusModel);
        WeakReferenceMessenger.Default.Send(this);
        Preferences.Set("status_deleted", Preferences.Get("status_deleted", 0) + 1);
        MakeToast("Status Deleted.");
        await base.OnBackButtonPressed();
    }

    async Task ShareStatusTextAsync()
    {
        await Share.Default.RequestAsync(new ShareTextRequest
        {
            Text = StatusModel.Text,
            Title = "Status"
        });
        await Task.Delay(500);
    }

    /// <summary>
    /// Todo 
    /// 1. Once linkParser is done check links to make sure they dont have any matching urls.
    /// 2. Once complete then fetch link data from internet using a parallel task.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    async Task PreviewLinksAsync()
    {
        LinksCollection.Clear();
        var matches = linkParser.Matches(StatusModel.Text);

        if (matches.Count <= 0)
            throw new Exception("No links found in your status text.");

        var links = matches.ToLookup(m => m.Value);

        CheckConnection();

        var task = links.Select(async match =>
        {
            string url = match.Key;
            var link = Links.FirstOrDefault(element => element.URL == url);

            link ??= await GetLinkData(url);

            if (link is not null)
                LinksCollection.Add(link);
        });

        await Task.WhenAll(task);

        Links = LinksCollection.ToList();
        ShowUrlListView = Links.Count > 0;
    }
    #endregion

    #region Status Link
    [RelayCommand]
    async Task OpenLinkWebViewPopup(Link link)
    {
        await RunTryCatchAsync(async () =>
        {
            CheckConnection();
            await Shell.Current.ShowPopupAsync(new WebViewPopup(link));
        });
    }

    private static async Task<Link> GetLinkData(string url)
    {
        try
        {
            var graph = await OpenGraph.ParseUrlAsync(url);
            return new Link().DecodeMetaInformation(graph);
        }
        catch (Exception ex)
        {
            MakeToast($"{url} \nerror:{ex.Message}");
            return null;
        }
    }
    #endregion

    public override async Task OnBackButtonPressed()
    {
        await RunTryCatchAsync(async () =>
        {
            if (ShowUrlListView)
            {
                ShowUrlListView = false;
                HideNavBar = false;
                return;
            }

            await base.OnBackButtonPressed();
        });
    }
}
