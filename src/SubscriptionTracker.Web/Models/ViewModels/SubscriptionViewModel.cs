using System.ComponentModel.DataAnnotations;

namespace SubscriptionTracker.Web.Models.ViewModels
{
    /// <summary>
    /// View model for displaying subscription information
    /// </summary>
    public class SubscriptionViewModel
    {
        /// <summary>
        /// Gets or sets the subscription ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the subscription name
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the subscription cost
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// Gets or sets the payment time
        /// </summary>
        public DateTimeOffset PaymentTime { get; set; }

        /// <summary>
        /// Gets or sets the days until next payment
        /// </summary>
        public int DaysUntilPayment { get; set; }

        /// <summary>
        /// Gets or sets the category ID
        /// </summary>
        public Guid CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the category name
        /// </summary>
        public string CategoryName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the category color
        /// </summary>
        public string CategoryColor { get; set; } = string.Empty;
    }

    /// <summary>
    /// View model for subscription creation and editing
    /// </summary>
    public class SubscriptionFormViewModel
    {
        /// <summary>
        /// Gets or sets the subscription ID (null for creation)
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or sets the subscription name
        /// </summary>
        [Required(ErrorMessage = "Subscription name is required")]
        [StringLength(200, ErrorMessage = "Subscription name cannot be longer than 200 characters")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the subscription cost
        /// </summary>
        [Required(ErrorMessage = "Cost is required")]
        [Range(0.01, 9999999.99, ErrorMessage = "Cost must be greater than 0")]
        public decimal Cost { get; set; }

        /// <summary>
        /// Gets or sets the payment time
        /// </summary>
        [Required(ErrorMessage = "Payment time is required")]
        public DateTimeOffset PaymentTime { get; set; }

        /// <summary>
        /// Gets or sets the category ID
        /// </summary>
        [Required(ErrorMessage = "Category is required")]
        public Guid CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the available categories for selection
        /// </summary>
        public IEnumerable<CategoryViewModel> AvailableCategories { get; set; } = new List<CategoryViewModel>();
    }

    /// <summary>
    /// View model for displaying subscription dashboard information
    /// </summary>
    public class SubscriptionDashboardViewModel
    {
        /// <summary>
        /// Gets or sets the total monthly cost
        /// </summary>
        public decimal TotalMonthlyCost { get; set; }

        /// <summary>
        /// Gets or sets the upcoming payments
        /// </summary>
        public IEnumerable<SubscriptionViewModel> UpcomingPayments { get; set; } = new List<SubscriptionViewModel>();

        /// <summary>
        /// Gets or sets the category breakdown
        /// </summary>
        public IEnumerable<CategoryViewModel> CategoryBreakdown { get; set; } = new List<CategoryViewModel>();

        /// <summary>
        /// Gets or sets all active subscriptions
        /// </summary>
        public IEnumerable<SubscriptionViewModel> ActiveSubscriptions { get; set; } = new List<SubscriptionViewModel>();
    }
}