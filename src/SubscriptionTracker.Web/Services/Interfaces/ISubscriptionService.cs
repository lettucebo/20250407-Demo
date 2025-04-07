using SubscriptionTracker.Web.Models.Domain;

namespace SubscriptionTracker.Web.Services.Interfaces
{
    /// <summary>
    /// Service interface for managing subscriptions
    /// </summary>
    public interface ISubscriptionService
    {
        /// <summary>
        /// Gets all active subscriptions
        /// </summary>
        /// <returns>IEnumerable of active subscriptions</returns>
        Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync();

        /// <summary>
        /// Gets a subscription by its ID
        /// </summary>
        /// <param name="id">The subscription ID</param>
        /// <returns>The subscription if found, null otherwise</returns>
        Task<Subscription?> GetSubscriptionByIdAsync(Guid id);

        /// <summary>
        /// Gets subscriptions by category
        /// </summary>
        /// <param name="categoryId">The category ID</param>
        /// <returns>IEnumerable of subscriptions in the category</returns>
        Task<IEnumerable<Subscription>> GetSubscriptionsByCategoryAsync(Guid categoryId);

        /// <summary>
        /// Gets subscriptions by date range
        /// </summary>
        /// <param name="startDate">Start date of the range</param>
        /// <param name="endDate">End date of the range</param>
        /// <returns>IEnumerable of subscriptions within the date range</returns>
        Task<IEnumerable<Subscription>> GetSubscriptionsByDateRangeAsync(DateTimeOffset startDate, DateTimeOffset endDate);

        /// <summary>
        /// Creates a new subscription
        /// </summary>
        /// <param name="subscription">The subscription to create</param>
        /// <returns>The created subscription</returns>
        Task<Subscription> CreateSubscriptionAsync(Subscription subscription);

        /// <summary>
        /// Updates an existing subscription
        /// </summary>
        /// <param name="subscription">The subscription to update</param>
        /// <returns>The updated subscription</returns>
        Task<Subscription> UpdateSubscriptionAsync(Subscription subscription);

        /// <summary>
        /// Deletes a subscription
        /// </summary>
        /// <param name="id">The ID of the subscription to delete</param>
        /// <returns>Task representing the asynchronous operation</returns>
        Task DeleteSubscriptionAsync(Guid id);

        /// <summary>
        /// Gets total cost of all active subscriptions
        /// </summary>
        /// <returns>The total cost</returns>
        Task<decimal> GetTotalSubscriptionCostAsync();

        /// <summary>
        /// Gets upcoming subscriptions sorted by remaining days
        /// </summary>
        /// <param name="days">Number of days to look ahead (default 30)</param>
        /// <returns>IEnumerable of subscriptions ordered by payment date</returns>
        Task<IEnumerable<Subscription>> GetUpcomingSubscriptionsAsync(int days = 30);
    }
}