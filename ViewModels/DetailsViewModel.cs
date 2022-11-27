namespace WhatsStatusApp.ViewModels;

[QueryProperty(nameof(StatusModel), nameof(Status))]
public partial class DetailsViewModel : BaseViewModel
{
    [ObservableProperty] Status _StatusModel;
}
