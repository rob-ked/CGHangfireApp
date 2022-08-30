using CGHangfireApp.Helper;
using CGHangfireApp.Model.Entity;
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
    internal class JobCopyDataToDB : IJob
    {
        /// <summary>
        /// Konfiguracja harmonogramu zgodna z CRON (* * * * *)
        /// </summary>
        private string _schedule = null;

        public string GetName()
        {
            return "JobCopyDataToDB";
        }

        public string GetDescription()
        {
            return "Odczytuje dane z pliku i kopiuje do bazy danych, tworząc uprzednio odpowiednią strukturę.";
        }

        public void SetSchedule(string schedule)
        {
            _schedule = schedule;
        }

        public string GetSchedule()
        {
            return _schedule;
        }

        public string Run()
        {
            foreach (string file in new string[] { "posts", "comments", "photos" })
            {
                ReadAndSave(file);
            }

            return "Przetworzono dane";
        }

        /// <summary>
        /// Odczytuje wskazany plik i zapisuje jego zawartość do bazy danych
        /// </summary>
        /// <param name="file"></param>
        private void ReadAndSave(string file)
        {
            if (Directory.Exists(Settings.Params.App.JsonFilesPath) == false)
            {
                throw new Exception($"Katalog roboczy {Settings.Params.App.JsonFilesPath} nie istnieje. " +
                    $"Sprawdź Twój plik Settings.json i zweryfikuj istnienie wskazanego katalogu");
            }

            string filePath = Path.Combine(Settings.Params.App.JsonFilesPath, $"{file}.json");

            // plik powinien, ale nie musi istnieć
            if (File.Exists(filePath)) {
                string json = File.ReadAllText(filePath);
               
                using (var context = new DataContext(Settings.Params.App.SQLConnectionString))
                {
                    switch (file)
                    {
                        case "posts":
                            StorePostsData(json);
                            break;
                        case "photos":
                            StorePhotosData(json);
                            break;
                        case "comments":
                            StoreCommentsData(json);
                            break;
                        default:
                            break;
                    }                    
                }
            }            
        }

        /// <summary>
        /// Zapisuje dane komentarzy
        /// </summary>
        /// <param name="json">Dane w formacie JSON</param>
        /// <exception cref="NotImplementedException"></exception>
        private void StoreCommentsData(string json)
        {
            using (var con = new DataContext(Settings.Params.App.SQLConnectionString))
            {
                List<Comment> comments = JsonConvert.DeserializeObject<List<Comment>>(json);
                var newComments = comments.Where(c => !con.Comments.Any(ec => ec.Id != c.Id));
                con.Comments.AddRange(newComments);
                con.SaveChanges();
            }
        }

        /// <summary>
        /// Zapisuje dane zdjęć
        /// </summary>
        /// <param name="json">Dane w formacie JSON</param>
        /// <exception cref="NotImplementedException"></exception>
        private void StorePhotosData(string json)
        {
            using (var con = new DataContext(Settings.Params.App.SQLConnectionString))
            {
                List<Photo> photos = JsonConvert.DeserializeObject<List<Photo>>(json);
                var newPhotos = photos.Where(p => !con.Photos.Any(ep => ep.Id != p.Id));
                con.Photos.AddRange(newPhotos);
                con.SaveChanges();
            }
        }

        /// <summary>
        /// Zapisuje dane postów
        /// </summary>
        /// <param name="json">Dane w formacie JSON</param>
        /// <exception cref="NotImplementedException"></exception>
        private void StorePostsData(string json)
        {
            using (var con = new DataContext(Settings.Params.App.SQLConnectionString))
            {
                List<Post> posts = JsonConvert.DeserializeObject<List<Post>>(json);
                var newPosts = posts.Where(p => !con.Posts.Any(ep => ep.Id != p.Id));
                con.Posts.AddRange(newPosts);
                con.SaveChanges();
            }
        }
    }
}
