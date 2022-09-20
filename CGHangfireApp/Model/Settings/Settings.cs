using System.Collections.Generic;

namespace CGHangfireApp.Model.Settings
{
    /// <summary>
    /// Konfiguracja aplikacji
    /// </summary>
    public class Settings
    {
        public Api Api { get; set; }

        public App App { get; set; }

        /// <summary>
        /// Konfiguracja Hangfire
        /// </summary>
        public Hangfire Hangfire { get; set; }

        /// <summary>
        /// Lista zadań
        /// </summary>
        public List<Job> Jobs { get; set; }

    }
}
