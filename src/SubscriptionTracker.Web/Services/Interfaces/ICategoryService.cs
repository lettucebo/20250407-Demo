using SubscriptionTracker.Web.Models.Domain;

namespace SubscriptionTracker.Web.Services.Interfaces
{
    /// <summary>
    /// Service interface for managing categories
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Gets all active categories with their subscriptions
        /// </summary>
        /// <returns>IEnumerable of active categories</returns>
        Task<IEnumerable<Category>> GetAllCategoriesAsync();

        /// <summary>
        /// Gets a category by its ID
        /// </summary>
        /// <param name="id">The category ID</param>
        /// <returns>The category if found, null otherwise</returns>
        Task<Category?> GetCategoryByIdAsync(Guid id);

        /// <summary>
        /// Creates a new category
        /// </summary>
        /// <param name="category">The category to create</param>
        /// <returns>The created category</returns>
        Task<Category> CreateCategoryAsync(Category category);

        /// <summary>
        /// Updates an existing category
        /// </summary>
        /// <param name="category">The category to update</param>
        /// <returns>The updated category</returns>
        Task<Category> UpdateCategoryAsync(Category category);

        /// <summary>
        /// Deletes a category
        /// </summary>
        /// <param name="id">The ID of the category to delete</param>
        /// <returns>Task representing the asynchronous operation</returns>
        Task DeleteCategoryAsync(Guid id);

        /// <summary>
        /// Gets total subscription cost by category
        /// </summary>
        /// <param name="categoryId">The category ID</param>
        /// <returns>The total cost of all subscriptions in the category</returns>
        Task<decimal> GetTotalCostByCategoryAsync(Guid categoryId);
    }
}