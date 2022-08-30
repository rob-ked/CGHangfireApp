using System.ComponentModel.DataAnnotations;

namespace CGHangfireApp.Model.Entity
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }
        public int AlbumId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }         
    }
}
