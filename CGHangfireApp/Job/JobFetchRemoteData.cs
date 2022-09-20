using CGHangfireApp.Helper;
using Hangfire.Console;
using Hangfire.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CGHangfireApp.Job
{
    internal class JobFetchRemoteData : IJob
    {
        /// <summary>
        /// Bazowe URL API
        /// </summary>
        private string _apiBaseURL = Settings.Params.Api.BaseURL;

        /// <summary>
        /// Konfiguracja harmonogramu zgodna z CRON (* * * * *)
        /// </summary>
        private string _schedule = null;

        public string GetName()
        {
            return "JobFetchRemoteData";
        }

        public string GetDescription()
        {
            return "Pobiera dane z zewnętrznego serwisu i zapisuje do plików json";
        }

        public void SetSchedule(string schedule)
        {
            _schedule = schedule;
        }

        public string GetSchedule()
        {
            return _schedule;
        }

        public string Run(PerformContext context)
        {
            context.WriteLine($"Rozpoczynam pracę");
            foreach (string endpoint in Settings.Params.Api.Endpoints)
            {
                context.WriteLine($"Przetwarzam {endpoint}");
                DownloadAndSave(endpoint);
            }
            context.WriteLine($"Konczę pracę");
            return "Pobrano dane";
        }

        /// <summary>
        /// Pobiera i zapisuje dane do plików
        /// </summary>
        /// <param name="endpoint"></param>
        private void DownloadAndSave(string endpoint)
        {
            string json = new WebClient().DownloadString($"{_apiBaseURL}/{endpoint}");
            if (string.IsNullOrEmpty(json) == false)
            {
                var serialized = JsonConvert.SerializeObject(
                    JsonConvert.DeserializeObject(json),
                    Formatting.Indented
                ); 

                if (Directory.Exists(Settings.Params.App.JsonFilesPath) == false)
                {
                    Directory.CreateDirectory(Settings.Params.App.JsonFilesPath);
                } 

                File.WriteAllText($"{Settings.Params.App.JsonFilesPath}\\{endpoint}.json", serialized);
            }            
        }
    }
}
