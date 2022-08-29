using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGHangfireApp.Model
{
    /// <summary>
    /// Konfiguracja aplikacji
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Ciąg połączenia z bazą danych
        /// </summary>
        public string SQLConnectionString { get; set; }

        /// <summary>
        /// Konfiguracja Hangfire
        /// </summary>
        public Hangfire Hangfire { get; set; }

    }
}
