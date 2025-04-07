using Microsoft.EntityFrameworkCore;
using SubscriptionTracker.Web.Data;
using SubscriptionTracker.Web.Models.Domain;
using SubscriptionTracker.Web.Repositories.Interfaces;

namespace SubscriptionTracker.Web.Repositories.Implementations
{
    /// <summary>
    /// Repository implementation for Category entity
    /// </summary>
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        /// <summary>
        /// Initializes a new instance of the CategoryRepository class
        /// </summary>
        /// <param name="context">The database context</param>
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <inheritdoc/>
        public IQueryable<Category> GetActiveWithSubscriptions()
        {
            return _dbSet
                .Include(c => c.Subscriptions)
                .Where(c => !c.IsDelete)
                .AsNoTracking();
        }

        /// <inheritdoc/>
        public async Task<Category?> GetByIdWithSubscriptionsAsync(Guid id)
        {
            return await _dbSet
                .Include(c => c.Subscriptions)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDelete);
        }

        /// <inheritdoc/>
        public async Task SoftDeleteAsync(Guid id)
        {
            var category = await _dbSet.FindAsync(id);
            if (category != null)
            {
                category.IsDelete = true;
                category.ModifyTime = DateTimeOffset.UtcNow;
                await UpdateAsync(category);
            }
        }
    }
}