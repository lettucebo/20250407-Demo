using System.ComponentModel.DataAnnotations;

namespace SubscriptionTracker.Web.Models.ViewModels
{
    /// <summary>
    /// View model for displaying category information
    /// </summary>
    public class CategoryViewModel
    {
        /// <summary>
        /// Gets or sets the category ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the category name
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the category color
        /// </summary>
        public string Color { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the total cost of subscriptions in this category
        /// </summary>
        public decimal TotalCost { get; set; }

        /// <summary>
        /// Gets or sets the number of subscriptions in this category
        /// </summary>
        public int SubscriptionCount { get; set; }
    }

    /// <summary>
    /// View model for category creation and editing
    /// </summary>
    public class CategoryFormViewModel
    {
        /// <summary>
        /// Gets or sets the category ID (null for creation)
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or sets the category name
        /// </summary>
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(100, ErrorMessage = "Category name cannot be longer than 100 characters")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the category color
        /// </summary>
        [Required(ErrorMessage = "Category color is required")]
        [RegularExpression("^#[0-9A-Fa-f]{6}$", ErrorMessage = "Category color must be a valid hex color code (e.g., #FF0000)")]
        public string Color { get; set; } = string.Empty;
    }
}