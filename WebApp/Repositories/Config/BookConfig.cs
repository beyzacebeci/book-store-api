using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Models;

namespace WebApp.Repositories.Config
{
    public class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasData(

                new Book {BookId = 1, BookName= "Beyaz Gemi",
                    Price =25,AuthorId = 1,CategoryId=2},
                new Book { BookId = 2, BookName = "İnsan Ne İle Yaşar?",  Price = 16,
                    AuthorId = 2, CategoryId = 3
                },
                new Book { BookId = 3, BookName = "Suç ve Ceza",  
                    Price = 50, AuthorId = 3,CategoryId = 1 }

                );

           // builder.HasOne(b => b.Author).WithMany(e => e.Books).HasForeignKey(b => b.AuthorId).IsRequired();
           // builder.HasOne(b => b.Category).WithMany(e => e.Books).HasForeignKey(b => b.CategoryId).IsRequired();
        }
    }
}
