using Microsoft.Extensions.Configuration;
using CGHangfireAppSettings = CGHangfireApp.Model.Settings.Settings;

namespace CGHangfireApp.Helper
{
    public sealed class Settings
    {
        private static CGHangfireAppSettings _params;
        public static CGHangfireAppSettings Params
        {
            get
            {
                if (_params == null)
                {
                    ProcessSettingsFile();
                }

                return _params;
            }
        }

        private Settings()
        {

        }

        /// <summary>
        /// Przetwarza plik ustawień aplikacji
        /// </summary>
        private static void ProcessSettingsFile()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("Settings.json")
                .AddEnvironmentVariables()
                .Build();

            _params = config.GetRequiredSection("Settings").Get<CGHangfireAppSettings>();
        }
    }
}
