using SubscriptionTracker.Web.Models.Domain;

namespace SubscriptionTracker.Web.Repositories.Interfaces
{
    /// <summary>
    /// Interface for Subscription repository operations
    /// </summary>
    public interface ISubscriptionRepository : IGenericRepository<Subscription>
    {
        /// <summary>
        /// Gets all active subscriptions with their categories
        /// </summary>
        /// <returns>IQueryable of active subscriptions with categories</returns>
        IQueryable<Subscription> GetActiveWithCategories();

        /// <summary>
        /// Gets subscriptions by category ID
        /// </summary>
        /// <param name="categoryId">The category ID</param>
        /// <returns>IQueryable of subscriptions for the specified category</returns>
        IQueryable<Subscription> GetByCategoryId(Guid categoryId);

        /// <summary>
        /// Gets subscriptions due within the specified date range
        /// </summary>
        /// <param name="startDate">The start date</param>
        /// <param name="endDate">The end date</param>
        /// <returns>IQueryable of subscriptions due within the date range</returns>
        IQueryable<Subscription> GetByDateRange(DateTimeOffset startDate, DateTimeOffset endDate);

        /// <summary>
        /// Soft deletes a subscription
        /// </summary>
        /// <param name="id">The subscription ID</param>
        /// <returns>Task representing the asynchronous operation</returns>
        Task SoftDeleteAsync(Guid id);
    }
}