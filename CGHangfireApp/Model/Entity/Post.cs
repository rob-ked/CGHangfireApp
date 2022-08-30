using System.ComponentModel.DataAnnotations;

namespace CGHangfireApp.Model.Entity
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
