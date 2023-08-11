using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApp.Repositories.Config
{
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(

               new Category { CategoryId = 1, CategoryName = "Psikolojik Roman" },
               new Category { CategoryId = 2, CategoryName = "Dram"},
               new Category { CategoryId = 3, CategoryName = "Kısa Hikaye" }

            );

            //builder.HasMany(e => e.Books).WithOne(e => e.Category).HasForeignKey(e => e.CategoryId);
        }
    }
}
