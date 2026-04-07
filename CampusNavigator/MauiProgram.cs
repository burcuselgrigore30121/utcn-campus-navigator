using CampusNavigator.Services;
using CampusNavigator.Views;
using Microsoft.Extensions.Logging;

namespace CampusNavigator
{
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
                });

            // Servicii
            builder.Services.AddSingleton<ScheduleService>();

            // Pagini
            builder.Services.AddTransient<SetupPage>();
            builder.Services.AddTransient<OrarPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}