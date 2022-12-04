namespace WhatsStatusApp;

public partial class App : Application
{
	public static Window _Window;
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();

        DeviceDisplay.MainDisplayInfoChanged += DeviceDisplay_MainDisplayInfoChanged;
	}

    protected override Window CreateWindow(IActivationState activationState)
    {
		if (DeviceInfo.Current.Idiom != DeviceIdiom.Desktop)
			return base.CreateWindow(activationState);

        var window = base.CreateWindow(activationState);
		window.MinimumHeight = 555;
		window.MinimumWidth = 800;
		window.SizeChanged += Window_SizeChanged;
		return window;
	}

    private void Window_SizeChanged(object sender, EventArgs e)
	{
		_Window = (Window)sender;
    }

    private void DeviceDisplay_MainDisplayInfoChanged(object sender, DisplayInfoChangedEventArgs e)
		=> WeakReferenceMessenger.Default.Send(e);

	public static Size GetDeviceSize()
    {
        if (DeviceInfo.Current.Idiom == DeviceIdiom.Desktop)
            return new Size(_Window.Width / 1.1, _Window.Height / 1.2);

		var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;

		if(mainDisplayInfo.Orientation is DisplayOrientation.Landscape)
			return new Size(mainDisplayInfo.Width / 2.1, mainDisplayInfo.Height / 2);
		return new Size(mainDisplayInfo.Width / 2, mainDisplayInfo.Height / 2.1);

    }
}
