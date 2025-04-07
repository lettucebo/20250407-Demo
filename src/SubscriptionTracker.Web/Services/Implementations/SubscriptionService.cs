using Microsoft.EntityFrameworkCore;
using SubscriptionTracker.Web.Models.Domain;
using SubscriptionTracker.Web.Repositories.Interfaces;
using SubscriptionTracker.Web.Services.Interfaces;

namespace SubscriptionTracker.Web.Services.Implementations
{
    /// <summary>
    /// Service implementation for managing subscriptions
    /// </summary>
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ICategoryRepository _categoryRepository;

        /// <summary>
        /// Initializes a new instance of the SubscriptionService class
        /// </summary>
        /// <param name="subscriptionRepository">The subscription repository</param>
        /// <param name="categoryRepository">The category repository</param>
        public SubscriptionService(
            ISubscriptionRepository subscriptionRepository,
            ICategoryRepository categoryRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _categoryRepository = categoryRepository;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync()
        {
            return await _subscriptionRepository.GetActiveWithCategories().ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Subscription?> GetSubscriptionByIdAsync(Guid id)
        {
            return await _subscriptionRepository.GetActiveWithCategories()
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Subscription>> GetSubscriptionsByCategoryAsync(Guid categoryId)
        {
            return await _subscriptionRepository.GetByCategoryId(categoryId).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Subscription>> GetSubscriptionsByDateRangeAsync(
            DateTimeOffset startDate, DateTimeOffset endDate)
        {
            return await _subscriptionRepository.GetByDateRange(startDate, endDate).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Subscription> CreateSubscriptionAsync(Subscription subscription)
        {
            await ValidateSubscriptionAsync(subscription);

            subscription.CreateTime = DateTimeOffset.UtcNow;
            subscription.ModifyTime = DateTimeOffset.UtcNow;

            var result = await _subscriptionRepository.CreateAsync(subscription);
            await _subscriptionRepository.SaveAsync();
            return result;
        }

        /// <inheritdoc/>
        public async Task<Subscription> UpdateSubscriptionAsync(Subscription subscription)
        {
            await ValidateSubscriptionAsync(subscription);

            var existingSubscription = await GetSubscriptionByIdAsync(subscription.Id)
                ?? throw new InvalidOperationException($"Subscription with ID {subscription.Id} not found.");

            existingSubscription.Name = subscription.Name;
            existingSubscription.Cost = subscription.Cost;
            existingSubscription.PaymentTime = subscription.PaymentTime;
            existingSubscription.CategoryId = subscription.CategoryId;
            existingSubscription.ModifyTime = DateTimeOffset.UtcNow;

            var result = await _subscriptionRepository.UpdateAsync(existingSubscription);
            await _subscriptionRepository.SaveAsync();
            return result;
        }

        /// <inheritdoc/>
        public async Task DeleteSubscriptionAsync(Guid id)
        {
            var subscription = await GetSubscriptionByIdAsync(id)
                ?? throw new InvalidOperationException($"Subscription with ID {id} not found.");

            await _subscriptionRepository.SoftDeleteAsync(id);
            await _subscriptionRepository.SaveAsync();
        }

        /// <inheritdoc/>
        public async Task<decimal> GetTotalSubscriptionCostAsync()
        {
            var subscriptions = await GetAllSubscriptionsAsync();
            return subscriptions.Sum(s => s.Cost);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Subscription>> GetUpcomingSubscriptionsAsync(int days = 30)
        {
            var endDate = DateTimeOffset.UtcNow.AddDays(days);
            var subscriptions = await GetSubscriptionsByDateRangeAsync(DateTimeOffset.UtcNow, endDate);
            return subscriptions.OrderBy(s => s.PaymentTime);
        }

        private async Task ValidateSubscriptionAsync(Subscription subscription)
        {
            if (string.IsNullOrWhiteSpace(subscription.Name))
            {
                throw new ArgumentException("Subscription name cannot be empty.");
            }

            if (subscription.Cost < 0)
            {
                throw new ArgumentException("Subscription cost cannot be negative.");
            }

            var category = await _categoryRepository.GetByIdWithSubscriptionsAsync(subscription.CategoryId);
            if (category == null)
            {
                throw new ArgumentException($"Category with ID {subscription.CategoryId} not found.");
            }

            if (category.IsDelete)
            {
                throw new ArgumentException($"Category with ID {subscription.CategoryId} has been deleted.");
            }
        }
    }
}