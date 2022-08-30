using HLogging = Hangfire.Logging;
using System.Text.Json.Serialization;

namespace CGHangfireApp.Model.Settings
{
    /// <summary>
    /// Ustawienia aplikacji
    /// </summary>
    public class App
    {
        private readonly string _jsonFilesPathFallbackValue = "c:\\CGHangfireAppData\\json";
        
        /// <summary>
        /// Ciąg połączenia z bazą danych
        /// </summary>
        public string SQLConnectionString { get; set; }

        private string _jsonFilesPath;
        /// <summary>
        /// Ścieżka do plików JSON
        /// </summary>
        public string JsonFilesPath
        {
            get
            {
                return _jsonFilesPath;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _jsonFilesPath = _jsonFilesPathFallbackValue;
                }
                else
                {
                    _jsonFilesPath = value;
                }
            }
        }
    }
}
