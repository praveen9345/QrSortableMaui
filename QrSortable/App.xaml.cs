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
    Console.WriteLine("=== AppCenter Crash Test ===");
    
    // Check if crashes are enabled
    bool isCrashesEnabled = await Crashes.IsEnabledAsync();
    Console.WriteLine($"Crashes Enabled: {isCrashesEnabled}");
    
    // Check for previous crash
    bool didCrash = await Crashes.HasCrashedInLastSessionAsync();
    Console.WriteLine($"Had previous crash: {didCrash}");
    
    if (didCrash)
    {
        var report = await Crashes.GetLastSessionCrashReportAsync();
        Console.WriteLine($"Previous crash ID: {report?.Id}");
    }
    
    // Wait for crash upload
    await Task.Delay(5000);
    
    // Initialize app
    await _appService.OnStartAsync();
    
    // Simple crash test - comment out after first successful test
    bool shouldTestCrash = Preferences.Get("TestCrashDone", false);
    if (!shouldTestCrash)
    {
        Preferences.Set("TestCrashDone", true);
        Console.WriteLine("CRASHING NOW!");
        await Task.Delay(1000);
        throw new Exception("Manual test crash - this should appear in AppCenter");
    }
    else
    {
        Console.WriteLine("Crash test already done. Check AppCenter portal.");
    }
#else
            await _appService.OnStartAsync();
#endif
        }


    }
}