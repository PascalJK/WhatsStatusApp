namespace WhatsStatusApp;

public partial class App : Application
{
	public static Window _Window;
	public static Window mobileWindow = new();

	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
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

	public static Size GetDeviceSize()
    {
        if (DeviceInfo.Current.Idiom == DeviceIdiom.Desktop)
            return new Size(_Window.Width / 1.1, _Window.Height / 1.2);

		if(mobileWindow.Width > mobileWindow.Height)
			return new Size(mobileWindow.Width / 1.088, mobileWindow.Height / 1.07);
		return new Size(mobileWindow.Width / 1.07, mobileWindow.Height / 1.088);

    }
}
