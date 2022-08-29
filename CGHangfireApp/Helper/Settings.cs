using Microsoft.Extensions.Configuration;

namespace CGHangfireApp.Helper
{
    public sealed class Settings
    {
        private static Model.Settings _params;
        public static Model.Settings Params
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

            _params = config.GetRequiredSection("Settings").Get<Model.Settings>();
        }
    }
}
