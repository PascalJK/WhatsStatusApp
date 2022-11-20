namespace WhatsStatusApp.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty] bool _IsBusy;

    public ICommand ClosePageCommand { get; set; }
}
