namespace SubscriptionTracker.Web.Models.Domain
{
    /// <summary>
    /// Represents a category for subscription services
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Gets or sets the unique identifier for the category
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the category
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the color used for displaying the category
        /// </summary>
        public string Color { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether this category is deleted
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// Gets or sets the creation time of the category
        /// </summary>
        public DateTimeOffset CreateTime { get; set; }

        /// <summary>
        /// Gets or sets the last modification time of the category
        /// </summary>
        public DateTimeOffset ModifyTime { get; set; }

        /// <summary>
        /// Gets or sets the collection of subscriptions in this category
        /// </summary>
        public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}