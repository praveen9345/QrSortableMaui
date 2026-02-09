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
            // Check if AppCenter is enabled
            bool isCrashesEnabled = await Crashes.IsEnabledAsync();
            Console.WriteLine($"AppCenter Crashes Enabled: {isCrashesEnabled}");
            
            // Check for previous crashes
            bool didCrash = await Crashes.HasCrashedInLastSessionAsync();
            if (didCrash)
            {
                Console.WriteLine("App crashed in last session");
                var crashReport = await Crashes.GetLastSessionCrashReportAsync();
                Console.WriteLine($"Crash report: {crashReport?.Id}");
            }
            
            // Give AppCenter time to send previous crash reports
            await Task.Delay(3000); // Increased to 3 seconds
#endif

            try
            {
                await _appService.OnStartAsync();

#if IOS
                // Only generate test crash on SECOND launch and in DEBUG mode
                // Check a preference to see if we've already crashed once
                bool hasGeneratedTestCrash = Preferences.Get("HasGeneratedTestCrash", false);
                
                if (!hasGeneratedTestCrash)
                {
                    // Mark that we'll crash on next launch
                    Preferences.Set("HasGeneratedTestCrash", true);
                    Console.WriteLine("Test crash will be generated on next app launch");
                }
                else
                {
                    // This is the second launch - generate the test crash
                    Console.WriteLine("Generating test crash now...");
                    await Task.Delay(1000); // Small delay before crash
                    Crashes.GenerateTestCrash();
                }
#endif
            }
            catch (Exception ex)
            {
#if IOS
                System.Diagnostics.Debug.WriteLine($"Error in OnStart: {ex.Message}");
                // Track error in AppCenter
                Crashes.TrackError(ex);
#endif
            }
        }
    }
}