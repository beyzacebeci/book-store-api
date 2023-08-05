using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Models;

namespace WebApp.Repositories.Config
{
    public class AuthorConfig : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.HasData(

                new Author { AuthorId = 1, AuthorName = "Cengiz Aytmatov"},
                new Author { AuthorId = 2, AuthorName = "Tolstoy"},
                new Author { AuthorId = 3, AuthorName = "Dostoyevski" }
    


            );

          //  builder.HasMany(e => e.Books).WithOne(e => e.Author).HasForeignKey(e => e.AuthorId);
        }
    }
}
