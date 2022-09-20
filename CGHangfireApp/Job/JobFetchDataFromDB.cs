using CGHangfireApp.Helper;
using CGHangfireApp.Model.Entity;
using Newtonsoft.Json;
using System;
using System.Linq;
using CommentsPage = CGHangfireApp.Model.Page.Comments;
using PostsPage = CGHangfireApp.Model.Page.Posts;
using PhotosPage = CGHangfireApp.Model.Page.Photos;
using System.Text.RegularExpressions;
using Hangfire.Server;
using Hangfire.Console;

namespace CGHangfireApp.Job
{
    internal class JobFetchDataFromDB : IJob
    {
        /// <summary>
        /// Liczba elementów na stronę wyników
        /// </summary>
        private int _perPageItems = 22;

        /// <summary>
        /// Konfiguracja harmonogramu zgodna z CRON (* * * * *)
        /// </summary>
        private string _schedule = null;

        public string GetName()
        {
            return "JobFetchDataFromDB";
        }

        public string GetDescription()
        {
            return "Odczytuje dane z bazy i wyświetla w formacie JSON.";
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
            context.WriteLine("Rozpoczynam pracę");
            return ReadAndDisplay();            
        }

        /// <summary>
        /// 
        /// </summary>
        private string ReadAndDisplay()
        {
            return JsonConvert.SerializeObject(new
            {
                Posts = GetPostsPage(),
                Comments = GetCommentsPage(),
                Photos = GetPhotosPage()
            });
        }

        /// <summary>
        /// Zwraca losową stornę wyników z tabeli zdjęć
        /// </summary>
        /// <returns></returns>
        private PhotosPage GetPhotosPage()
        {
            using (var con = new DataContext(Settings.Params.App.SQLConnectionString))
            {
                var photos = con.Photos;
                var photosTotalPages = (int)Math.Ceiling((double)photos.Count() / _perPageItems);
                var photosPageNo = (new Random()).Next(1, photosTotalPages);
                var photosPortion = photos.OrderBy(c => c.Id)
                    .Skip(photosPageNo * _perPageItems)
                    .Take(_perPageItems);

                return new PhotosPage()
                {
                    Data = photosPortion.ToList(),
                    TotalItems = photos.Count(),
                    TotalPages = photosTotalPages,
                    CurrentPage = photosPageNo,
                    PerPageItems = _perPageItems
                };
            }
        }

        /// <summary>
        /// Zwraca losową stronę wyników z tabeli komentarzy
        /// </summary>
        /// <returns></returns>
        private CommentsPage GetCommentsPage()
        {
            using (var con = new DataContext(Settings.Params.App.SQLConnectionString))
            {
                var comments = con.Comments;
                var commentsTotalPages = (int)Math.Ceiling((double)comments.Count() / _perPageItems);
                var commentsPageNo = (new Random()).Next(1, commentsTotalPages);
                var commentsPortion = comments.OrderBy(c => c.Id)
                    .Skip(commentsPageNo * _perPageItems)
                    .Take(_perPageItems);

                return new CommentsPage()
                {
                    Data = commentsPortion.ToList(),
                    TotalItems = comments.Count(),
                    TotalPages = commentsTotalPages,
                    CurrentPage = commentsPageNo,
                    PerPageItems = _perPageItems
                };
            }
        }

        /// <summary>
        /// Zwraca losową stronę wyników z tabeli postów
        /// </summary>
        /// <returns></returns>
        private PostsPage GetPostsPage()
        {
            using (var con = new DataContext(Settings.Params.App.SQLConnectionString))
            {
                var posts = con.Posts;
                var postsTotalPages = (int)Math.Ceiling((double)posts.Count() / _perPageItems);
                var postsPageNo = (new Random()).Next(1, postsTotalPages);
                var postsPortion = posts.OrderBy(c => c.Id)
                    .Skip(postsPageNo * _perPageItems)
                    .Take(_perPageItems);

                return new PostsPage()
                {
                    Data = postsPortion.ToList(),
                    TotalItems = posts.Count(),
                    TotalPages = postsTotalPages,
                    CurrentPage = postsPageNo,
                    PerPageItems = _perPageItems
                };
            }
        }
    }
}
