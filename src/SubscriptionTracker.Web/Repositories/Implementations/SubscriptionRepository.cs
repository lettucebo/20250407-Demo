using Microsoft.EntityFrameworkCore;
using SubscriptionTracker.Web.Data;
using SubscriptionTracker.Web.Models.Domain;
using SubscriptionTracker.Web.Repositories.Interfaces;

namespace SubscriptionTracker.Web.Repositories.Implementations
{
    /// <summary>
    /// Repository implementation for Subscription entity
    /// </summary>
    public class SubscriptionRepository : GenericRepository<Subscription>, ISubscriptionRepository
    {
        /// <summary>
        /// Initializes a new instance of the SubscriptionRepository class
        /// </summary>
        /// <param name="context">The database context</param>
        public SubscriptionRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <inheritdoc/>
        public IQueryable<Subscription> GetActiveWithCategories()
        {
            return _dbSet
                .Include(s => s.Category)
                .Where(s => !s.IsDelete)
                .AsNoTracking();
        }

        /// <inheritdoc/>
        public IQueryable<Subscription> GetByCategoryId(Guid categoryId)
        {
            return _dbSet
                .Include(s => s.Category)
                .Where(s => s.CategoryId == categoryId && !s.IsDelete)
                .AsNoTracking();
        }

        /// <inheritdoc/>
        public IQueryable<Subscription> GetByDateRange(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            return _dbSet
                .Include(s => s.Category)
                .Where(s => s.PaymentTime >= startDate && 
                           s.PaymentTime <= endDate && 
                           !s.IsDelete)
                .AsNoTracking();
        }

        /// <inheritdoc/>
        public async Task SoftDeleteAsync(Guid id)
        {
            var subscription = await _dbSet.FindAsync(id);
            if (subscription != null)
            {
                subscription.IsDelete = true;
                subscription.ModifyTime = DateTimeOffset.UtcNow;
                await UpdateAsync(subscription);
            }
        }
    }
}