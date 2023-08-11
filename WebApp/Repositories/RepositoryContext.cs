using Entities.Models;
using Microsoft.EntityFrameworkCore;

using WebApp.Repositories.Config;

namespace WebApp.Repositories
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options) : base(options) { }


        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new BookConfig());
            //modelBuilder.ApplyConfiguration(new AuthorConfig());
            //modelBuilder.ApplyConfiguration(new CategoryConfig());


            modelBuilder.Entity<Book>()
                .HasOne<Author>(e => e.Author)
                .WithMany(e => e.Books)
                .HasForeignKey(e => e.AuthorId);

            modelBuilder.Entity<Book>()
                .HasOne<Category>(e => e.Category)
                .WithMany(e => e.Books)
                .HasForeignKey(e => e.CategoryId);

 
        }



    }
}
