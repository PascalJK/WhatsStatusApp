namespace WhatsStatusApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
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
				fonts.AddFont("materialdesignicons.ttf", "MDI");
			});

		//Services
		builder.Services.AddSingleton<MainPageViewModel>();

		return builder.Build();
	}
}
