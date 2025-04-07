using SubscriptionTracker.Web.Models.Domain;

namespace SubscriptionTracker.Web.Repositories.Interfaces
{
    /// <summary>
    /// Interface for Category repository operations
    /// </summary>
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        /// <summary>
        /// Gets all active categories with their subscriptions
        /// </summary>
        /// <returns>IQueryable of active categories with subscriptions</returns>
        IQueryable<Category> GetActiveWithSubscriptions();

        /// <summary>
        /// Gets a category by ID including its subscriptions
        /// </summary>
        /// <param name="id">The category ID</param>
        /// <returns>The category with its subscriptions</returns>
        Task<Category?> GetByIdWithSubscriptionsAsync(Guid id);

        /// <summary>
        /// Soft deletes a category
        /// </summary>
        /// <param name="id">The category ID</param>
        /// <returns>Task representing the asynchronous operation</returns>
        Task SoftDeleteAsync(Guid id);
    }
}