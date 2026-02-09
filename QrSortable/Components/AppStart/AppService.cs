namespace QrSortable.Components.CoreFeatures.AppStart
{
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a service responsible for initializing and managing various application components.
    /// We use this in favor of the base class to migrate the databases on app start and to initialize
    /// the connection to App Center for error logging.
    /// </summary>
    public class AppService : IAppService
    {
    

        private const string DatabaseName = "QrSortable.sqlite3";
        private const string BackendDatabaseName = "QrSortable.sqlite3";

        /// <summary>
        ///     Initializes the application.
        /// </summary>
        public AppService()
        {
            
        }

        public async Task OnStartAsync()
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Hello",
                    "This is a MAUI alert",
                    "OK"
                );
            });
        }
    }
}
