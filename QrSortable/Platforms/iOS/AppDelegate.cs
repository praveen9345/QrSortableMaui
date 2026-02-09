using Foundation;
using UIKit;

namespace QrSortable
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                Console.WriteLine("🔥 iOS UnhandledException:");
                Console.WriteLine(e.ExceptionObject?.ToString());
            };

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                Console.WriteLine("🔥 iOS UnobservedTaskException:");
                Console.WriteLine(e.Exception?.ToString());
                e.SetObserved();
            };

            return base.FinishedLaunching(app, options);
        }
    }
}
