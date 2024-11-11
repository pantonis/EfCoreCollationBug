using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EfCoreCollationBug
{
    public class BloggingContext : DbContext
    {
        string collName = """en_US.utf8_ci""";

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasCollation(name: collName, locale: "en_US.utf8", provider: "icu", deterministic: false);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=BlogDb;Username=YourUsername;Password=YourPassword");
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);

            configurationBuilder.Properties<string>().UseCollation(collName);
        }
    }


    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public List<Post> Posts { get; set; } = new();
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }

}
