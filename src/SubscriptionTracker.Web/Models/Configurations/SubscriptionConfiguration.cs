using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubscriptionTracker.Web.Models.Domain;

namespace SubscriptionTracker.Web.Models.Configurations
{
    /// <summary>
    /// Entity Framework Core configuration for the Subscription entity
    /// </summary>
    public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        /// <summary>
        /// Configures the entity mapping for Subscription
        /// </summary>
        /// <param name="builder">The entity type builder</param>
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Cost)
                .IsRequired();

            builder.Property(x => x.PaymentTime)
                .IsRequired();

            builder.Property(x => x.IsDelete)
                .IsRequired();

            builder.Property(x => x.CreateTime)
                .IsRequired();

            builder.Property(x => x.ModifyTime)
                .IsRequired();

            builder.HasOne(x => x.Category)
                .WithMany(x => x.Subscriptions)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}