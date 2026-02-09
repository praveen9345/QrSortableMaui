namespace QrSortable
{
    using CommunityToolkit.Maui;
    using Microsoft.AppCenter;
    using Microsoft.AppCenter.Crashes;
    using Microsoft.Maui.Controls.Compatibility.Hosting;
    using System.Reflection;
    using Microsoft.AppCenter.Analytics;

    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCompatibility()
                .UseMauiCommunityToolkit()
                .RegisterServices()
                .RegisterViewsAndViewModels()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if IOS
            // Initialize AppCenter with both Analytics and Crashes
            AppCenter.Start("ios=1c2e9b8a-3f9b-498c-b519-3ebc4ee3221f",
                typeof(Analytics), typeof(Crashes));
            
            // Enable verbose logging for debugging
            AppCenter.LogLevel = LogLevel.Verbose;
#endif

            return builder.Build();
        }


        public static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
        {

            string[] singletonTypeEndings =
            {
                "Service", "Manager", "Wrapper", "Logger", "Provider", "Scheduler", "Handler", "Client", "Builder",
                "Validator", "Helper"
            };

            var exportedTypes = Assembly.GetExecutingAssembly().GetExportedTypes();

            foreach (var singletonType in singletonTypeEndings)
            {
                foreach (var service in exportedTypes)
                {
                    if (!service.IsInterface && service.Name.EndsWith(singletonType)
                                             && !service.Name.EndsWith("HttpClientWrapper") && !service.IsAbstract)
                    {
                        var interfaceType = service.GetInterfaces().FirstOrDefault(type =>
                            type.Name.EndsWith(service.Name));

                        if (interfaceType != null)
                        {
                            builder.Services.AddSingleton(interfaceType, service);
                        }
                    }
                }
            }

            //External Services

            return builder;
        }


        /// <summary>
        ///     Registers all classes of which the name ends with "ViewModel" 
        ///     and tries to register a matching view for each view model.
        /// </summary>
        /// <param name="builder">The app builder.</param>
        public static MauiAppBuilder RegisterViewsAndViewModels(this MauiAppBuilder builder)
        {
            var types = Assembly.GetExecutingAssembly().GetExportedTypes();

            foreach (var type in types)
            {
                if (type.Name.EndsWith("ViewModel") && !type.IsAbstract)
                {
                    var viewType = types.FirstOrDefault(t => t.Name == type.Name.Replace("ViewModel", "View"));
                    if (viewType != null)
                    {
                        builder.Services.AddTransient(type);
                        builder.Services.AddTransient(viewType);
                    }
                }
            }
            return builder;
        }
    }
}
