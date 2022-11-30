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
    [ObservableProperty] bool _ShowOptionDrawer, _ShowUrlListView;
    #endregion

    [RelayCommand]
    public void LoadOptionsDrawer()
    {
        ShowOptionDrawer = !ShowOptionDrawer;
        HideNavBar = ShowOptionDrawer;
    }

    [RelayCommand]
    async Task DeleteStatusAsync()
    {
        await RunTryCatchAsync(async () =>
        {
            var ans = await Shell.Current.DisplayAlert("", "Delete Status?", "Yes", "Cancel");
            if (!ans)
                return;
            await LocalDatabaseService.LocalDB.DeleteStatusAsync(StatusModel);
            LoadOptionsDrawer();
            WeakReferenceMessenger.Default.Send(this);
            MakeToast("Status Deleted.");
            await base.OnBackButtonPressed();
        });
    }

    [RelayCommand]
    async Task ShareStatusTextAsync()
    {
        await RunTryCatchAsync(async () =>
        {
            await Share.Default.RequestAsync(new ShareTextRequest
            {
                Text = StatusModel.Text,
                Title = "Status"
            });
            await Task.Delay(500);
            LoadOptionsDrawer();
        });
    }

    #region Status Link
    [RelayCommand]
    async Task OpenLinkWebViewPopup(Link link)
    {
        await RunTryCatchAsync(async () =>
        {
            await Shell.Current.ShowPopupAsync(new WebViewPopup(link));
        });
    }

    /// <summary>
    /// Todo 
    /// 1. Once linkParser is done check links to make sure they dont have any matching urls.
    /// 2. Once complete then fetch link data from internet using a parallel task.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    [RelayCommand]
    async Task PreviewLinksAsync()
    {
        LinksCollection.Clear();
        await RunTryCatchAsync(async () =>
        {
            var links = linkParser.Matches(StatusModel.Text);

            if (links.Count <= 0)
                throw new Exception("No links found in your status text.");

            foreach (var item in links)
            {
                string url = item.ToString();
                var link = Links.FirstOrDefault(element => element.URL == url);
                link ??= await GetLinkData(url);
                if (link != null)
                    LinksCollection.Add(link);
            }
            Links = LinksCollection.ToList();
            LoadOptionsDrawer();
            ShowUrlListView = true;
        });
    }

    private static async Task<Link> GetLinkData(string url)
    {
        try
        {
            CheckConnection();
            var graph = await OpenGraph.ParseUrlAsync(url);
            return new Link().DecodeMetaInformation(graph);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
    #endregion

    public override async Task OnBackButtonPressed()
    {
        await RunTryCatchAsync(async () =>
        {
            if (ShowOptionDrawer)
            {
                LoadOptionsDrawer();
                return;
            }
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
