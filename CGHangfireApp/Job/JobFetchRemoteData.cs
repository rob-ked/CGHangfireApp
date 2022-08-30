using CGHangfireApp.Helper;
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
        private string _apiBaseURL = "https://jsonplaceholder.typicode.com";

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

        public void Run()
        {
            foreach (string endpoint in new string[] { "posts", "comments", "photos" })
            {
                DownloadAndSave(endpoint);
            }
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
