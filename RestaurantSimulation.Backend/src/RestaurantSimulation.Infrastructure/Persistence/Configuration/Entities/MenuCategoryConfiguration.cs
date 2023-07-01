using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantSimulation.Domain.Entities.Restaurant;

namespace RestaurantSimulation.Infrastructure.Persistence.Configuration.Entities
{
    public class MenuCategoryConfiguration : IEntityTypeConfiguration<MenuCategory>
    {
        public void Configure(EntityTypeBuilder<MenuCategory> builder)
        {
            builder.ToTable("MenuCategory");

            builder.HasKey(x => x.Id);

            builder.HasMany(g => g.Products)
                .WithOne(s => s.Category)
                .HasForeignKey(s => s.CategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
