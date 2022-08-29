using HLogging = Hangfire.Logging;
using System.Text.Json.Serialization;

namespace CGHangfireApp.Model
{
    /// <summary>
    /// Ustawienia specyficzne dla Hanfgire
    /// </summary>
    public class Hangfire
    {
        private readonly string _addressFallbackValue = "127.0.0.1";
        private readonly string _portFallbackValue = "7777";
        private readonly string _pathFallbackValue = "hangfire";

        private string _address;
        /// <summary>
        /// URL pulpitu
        /// </summary>
        public string Address
        {
            get
            {
                return _address;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _address = _addressFallbackValue;
                }
                else
                {
                    _address = value;
                }
            }
        }

        private string _port;

        /// <summary>
        /// Numer portu
        /// </summary>
        public string Port
        {
            get
            {
                return _port;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _port = _portFallbackValue;
                }
                else
                {
                    _port = value;
                }
            }
        }

        private string _path;

        /// <summary>
        /// Ścieżka
        /// </summary>
        public string Path
        {
            get
            {
                return _path;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _path = _pathFallbackValue;
                }
                else
                {
                    _path = value;
                }
            }
        }

        /// <summary>
        /// URL witryny
        /// </summary>
        public string HangfireURL
        {
            get
            {
                return $"http://{_address}:{_port}/{_path}";
            }
        }

        private HLogging.LogLevel _logLevel;

        /// <summary>
        /// Poziom wpisów w dzienniku
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public HLogging.LogLevel LogLevel
        {
            get
            {
                return _logLevel;
            }

            set
            {
                 _logLevel = value;
            }
        }
    }
}
