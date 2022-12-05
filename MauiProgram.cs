using CommunityToolkit.Maui;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace WhatsStatusApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseSkiaSharp()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("DancingScript-Regular.ttf", "DancingScriptRegular");
				fonts.AddFont("IndieFlower-Regular.ttf", "IndieFlowerRegular");
				fonts.AddFont("Pacifico-Regular.ttf", "PacificoRegular");
				fonts.AddFont("PlayfairDisplay-Regular.ttf", "PlayfairDisplayRegular");
				fonts.AddFont("RubikDistressed-Regular.ttf", "RubikDistressedRegular");
				fonts.AddFont("Ubuntu-Regular.ttf", "UbuntuRegular");
				fonts.AddFont("Teko-Regular.ttf", "TekoRegular");
				fonts.AddFont("Acme-Regular.ttf", "AcmeRegular");
				fonts.AddFont("PermanentMarker-Regular.ttf", "PermanentMarkerRegular");
				fonts.AddFont("materialdesignicons.ttf", "MDI");
			});

        // ViewModels
        builder.Services.AddSingleton<DetailsViewModel>();
		builder.Services.AddSingleton<MainPageViewModel>();
		builder.Services.AddTransient<SettingsPageViewModel>();

		// Views
		builder.Services.AddTransient<DetailsPage>();
		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddTransient<SettingsPage>();

		return builder.Build();
	}
}
