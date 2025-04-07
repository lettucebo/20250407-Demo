using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubscriptionTracker.Web.Models.Domain;

namespace SubscriptionTracker.Web.Models.Configurations
{
    /// <summary>
    /// Entity Framework Core configuration for the Category entity
    /// </summary>
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        /// <summary>
        /// Configures the entity mapping for Category
        /// </summary>
        /// <param name="builder">The entity type builder</param>
        public void Configure(EntityTypeBuilder<Category> builder)
        {

            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Color)
                .IsRequired()
                .HasMaxLength(7); // For hex color codes (#RRGGBB)

            builder.Property(x => x.IsDelete)
                .IsRequired();

            builder.Property(x => x.CreateTime)
                .IsRequired();

            builder.Property(x => x.ModifyTime)
                .IsRequired();
        }
    }
}