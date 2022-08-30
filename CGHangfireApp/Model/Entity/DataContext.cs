using System.Data.Entity;

namespace CGHangfireApp.Model.Entity
{
    internal class DataContext : DbContext
    {
        public DataContext(string connectionString)
        {
            this.Database.Connection.ConnectionString = connectionString;
        }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Photo> Photos { get; set; }
    }
}
