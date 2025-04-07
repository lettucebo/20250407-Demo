namespace SubscriptionTracker.Web.Models.Domain
{
    /// <summary>
    /// Represents a subscription service
    /// </summary>
    public class Subscription
    {
        /// <summary>
        /// Gets or sets the unique identifier for the subscription
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the subscription service
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the cost of the subscription
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// Gets or sets the payment time of the subscription
        /// </summary>
        public DateTimeOffset PaymentTime { get; set; }

        /// <summary>
        /// Gets or sets the category ID of the subscription
        /// </summary>
        public Guid CategoryId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this subscription is deleted
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// Gets or sets the creation time of the subscription
        /// </summary>
        public DateTimeOffset CreateTime { get; set; }

        /// <summary>
        /// Gets or sets the last modification time of the subscription
        /// </summary>
        public DateTimeOffset ModifyTime { get; set; }

        /// <summary>
        /// Gets or sets the category of the subscription
        /// </summary>
        public virtual Category Category { get; set; } = null!;
    }
}