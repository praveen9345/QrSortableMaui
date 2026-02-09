namespace QrSortable
{
    using Microsoft.AppCenter.Crashes;
    using QrSortable.Components.CoreFeatures.AppStart;

    public partial class App : Application
    {
        private readonly IAppService _appService;

        public App(IAppService appService)
        {
            InitializeComponent();
            _appService = appService;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        protected override async void OnStart()
        {
            base.OnStart();

            #if IOS
	                // Give AppCenter 3 seconds to upload any previous crash reports
	                await Task.Delay(1000);
            #endif

             try
             {
                 await _appService.OnStartAsync();
                
                 #if IOS
				       Crashes.GenerateTestCrash();
                 #endif
            }
            catch (Exception ex)
             {
                #if IOS
				      // Track error in AppCenter
				      Crashes.TrackError(ex);
                #endif

             }
        }
    }
}