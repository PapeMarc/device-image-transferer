using device_image_transferer.ViewModels;
using Microsoft.Extensions.Logging;
using UraniumUI;

namespace device_image_transferer
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseUraniumUI()
                .UseUraniumUIMaterial()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .Services.AddSingleton<MainPage>()
                .AddSingleton<MainPageViewModel>()

                .AddTransient<RecieverPage>()
                .AddTransient<RecieverPageViewModel>()
                
                .AddTransient<TransmitterPage>()
                .AddTransient<TransmitterPageViewModel>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}
