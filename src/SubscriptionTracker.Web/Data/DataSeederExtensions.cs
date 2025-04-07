using Microsoft.EntityFrameworkCore;
using SubscriptionTracker.Web.Models.Domain;

namespace SubscriptionTracker.Web.Data
{
    /// <summary>
    /// Extension methods for seeding data
    /// </summary>
    public static class DataSeederExtensions
    {
        /// <summary>
        /// Seeds the database with initial sample data
        /// </summary>
        /// <param name="app">The web application instance</param>
        public static async Task SeedDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Ensure database is created
            await context.Database.EnsureCreatedAsync();

            // Check if data already exists
            if (await context.Categories.AnyAsync())
            {
                return;
            }

            // Sample categories
            var categories = new List<Category>
            {
                new() { Id = Guid.NewGuid(), Name = "Streaming Services", Color = "#FF4444", CreateTime = DateTimeOffset.UtcNow, ModifyTime = DateTimeOffset.UtcNow },
                new() { Id = Guid.NewGuid(), Name = "Cloud Storage", Color = "#4444FF", CreateTime = DateTimeOffset.UtcNow, ModifyTime = DateTimeOffset.UtcNow },
                new() { Id = Guid.NewGuid(), Name = "Software Licenses", Color = "#44FF44", CreateTime = DateTimeOffset.UtcNow, ModifyTime = DateTimeOffset.UtcNow },
                new() { Id = Guid.NewGuid(), Name = "Gaming", Color = "#FF44FF", CreateTime = DateTimeOffset.UtcNow, ModifyTime = DateTimeOffset.UtcNow },
                new() { Id = Guid.NewGuid(), Name = "Music", Color = "#FFFF44", CreateTime = DateTimeOffset.UtcNow, ModifyTime = DateTimeOffset.UtcNow }
            };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            // Sample subscriptions
            var subscriptions = new List<Subscription>
            {
                new() { Name = "Netflix", Cost = 15.99m, PaymentTime = DateTimeOffset.UtcNow.AddDays(5), CategoryId = categories[0].Id, CreateTime = DateTimeOffset.UtcNow, ModifyTime = DateTimeOffset.UtcNow },
                new() { Name = "Spotify", Cost = 9.99m, PaymentTime = DateTimeOffset.UtcNow.AddDays(10), CategoryId = categories[4].Id, CreateTime = DateTimeOffset.UtcNow, ModifyTime = DateTimeOffset.UtcNow },
                new() { Name = "Google Drive", Cost = 1.99m, PaymentTime = DateTimeOffset.UtcNow.AddDays(15), CategoryId = categories[1].Id, CreateTime = DateTimeOffset.UtcNow, ModifyTime = DateTimeOffset.UtcNow },
                new() { Name = "Adobe Creative Cloud", Cost = 52.99m, PaymentTime = DateTimeOffset.UtcNow.AddDays(8), CategoryId = categories[2].Id, CreateTime = DateTimeOffset.UtcNow, ModifyTime = DateTimeOffset.UtcNow },
                new() { Name = "Xbox Game Pass", Cost = 14.99m, PaymentTime = DateTimeOffset.UtcNow.AddDays(20), CategoryId = categories[3].Id, CreateTime = DateTimeOffset.UtcNow, ModifyTime = DateTimeOffset.UtcNow },
                new() { Name = "Disney+", Cost = 7.99m, PaymentTime = DateTimeOffset.UtcNow.AddDays(12), CategoryId = categories[0].Id, CreateTime = DateTimeOffset.UtcNow, ModifyTime = DateTimeOffset.UtcNow },
                new() { Name = "iCloud+", Cost = 2.99m, PaymentTime = DateTimeOffset.UtcNow.AddDays(25), CategoryId = categories[1].Id, CreateTime = DateTimeOffset.UtcNow, ModifyTime = DateTimeOffset.UtcNow },
                new() { Name = "GitHub Pro", Cost = 4.99m, PaymentTime = DateTimeOffset.UtcNow.AddDays(18), CategoryId = categories[2].Id, CreateTime = DateTimeOffset.UtcNow, ModifyTime = DateTimeOffset.UtcNow },
                new() { Name = "PlayStation Plus", Cost = 9.99m, PaymentTime = DateTimeOffset.UtcNow.AddDays(22), CategoryId = categories[3].Id, CreateTime = DateTimeOffset.UtcNow, ModifyTime = DateTimeOffset.UtcNow },
                new() { Name = "Apple Music", Cost = 10.99m, PaymentTime = DateTimeOffset.UtcNow.AddDays(28), CategoryId = categories[4].Id, CreateTime = DateTimeOffset.UtcNow, ModifyTime = DateTimeOffset.UtcNow }
            };

            await context.Subscriptions.AddRangeAsync(subscriptions);
            await context.SaveChangesAsync();
        }
    }
}