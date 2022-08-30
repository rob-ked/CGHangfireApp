namespace CGHangfireApp.Model.Settings
{
    public class Job
    {
        /// <summary>
        /// Nazwa zadania
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Czy aktywne?
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Harmonogram CRON
        /// </summary>
        public string Schedule { get; set; }
    }
}
