//using AndroidX.Startup;
using AuxDesk.Data;
using AuxDesk.Initialisation;
using AuxDesk.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AuxDesk
{
    public static class MauiProgram
    {
        public static async Task<MauiApp> CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            ConfigureServices(builder.Services);
            var app = builder.Build();

            // Initialize on startup
            var initialiser = app.Services.GetRequiredService<AppInitialiser>();
            await initialiser.InitialiseAsync();

            return app;
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ITaskRepository, JSONTaskRepository>();
            services.AddSingleton<IRecycleRepository, JSONRecycleRepository>();
            services.AddSingleton<ITaskService, TaskService>();
            services.AddSingleton<AppInitialiser>();
        }
    }
}
