using Microsoft.EntityFrameworkCore;
using SubscriptionTracker.Web.Models.Domain;
using SubscriptionTracker.Web.Repositories.Interfaces;
using SubscriptionTracker.Web.Services.Interfaces;

namespace SubscriptionTracker.Web.Services.Implementations
{
    /// <summary>
    /// Service implementation for managing categories
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        /// <summary>
        /// Initializes a new instance of the CategoryService class
        /// </summary>
        /// <param name="categoryRepository">The category repository</param>
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetActiveWithSubscriptions().ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Category?> GetCategoryByIdAsync(Guid id)
        {
            return await _categoryRepository.GetByIdWithSubscriptionsAsync(id);
        }

        /// <inheritdoc/>
        public async Task<Category> CreateCategoryAsync(Category category)
        {
            ValidateCategory(category);
            
            category.CreateTime = DateTimeOffset.UtcNow;
            category.ModifyTime = DateTimeOffset.UtcNow;
            
            var result = await _categoryRepository.CreateAsync(category);
            await _categoryRepository.SaveAsync();
            return result;
        }

        /// <inheritdoc/>
        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            ValidateCategory(category);

            var existingCategory = await _categoryRepository.GetByIdWithSubscriptionsAsync(category.Id)
                ?? throw new InvalidOperationException($"Category with ID {category.Id} not found.");

            existingCategory.Name = category.Name;
            existingCategory.Color = category.Color;
            existingCategory.ModifyTime = DateTimeOffset.UtcNow;

            var result = await _categoryRepository.UpdateAsync(existingCategory);
            await _categoryRepository.SaveAsync();
            return result;
        }

        /// <inheritdoc/>
        public async Task DeleteCategoryAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdWithSubscriptionsAsync(id)
                ?? throw new InvalidOperationException($"Category with ID {id} not found.");

            if (category.Subscriptions.Any())
            {
                throw new InvalidOperationException("Cannot delete category with active subscriptions.");
            }

            await _categoryRepository.SoftDeleteAsync(id);
            await _categoryRepository.SaveAsync();
        }

        /// <inheritdoc/>
        public async Task<decimal> GetTotalCostByCategoryAsync(Guid categoryId)
        {
            var category = await _categoryRepository.GetByIdWithSubscriptionsAsync(categoryId)
                ?? throw new InvalidOperationException($"Category with ID {categoryId} not found.");

            return category.Subscriptions.Where(s => !s.IsDelete).Sum(s => s.Cost);
        }

        private static void ValidateCategory(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                throw new ArgumentException("Category name cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(category.Color))
            {
                throw new ArgumentException("Category color cannot be empty.");
            }

            if (!category.Color.StartsWith("#") || category.Color.Length != 7)
            {
                throw new ArgumentException("Category color must be a valid hex color code (e.g., #FF0000).");
            }
        }
    }
}