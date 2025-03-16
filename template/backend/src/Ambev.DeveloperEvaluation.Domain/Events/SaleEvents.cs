namespace Ambev.DeveloperEvaluation.Domain.Events
{
    /// <summary>
    /// Event published when a sale is created.
    /// </summary>
    public class SaleCreatedEvent
    {
        public Guid SaleId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// Event published when a sale is modified.
    /// </summary>
    public class SaleModifiedEvent
    {
        public Guid SaleId { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// Event published when a sale is cancelled.
    /// </summary>
    public class SaleCancelledEvent
    {
        public Guid SaleId { get; set; }
        public DateTime CancelledAt { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// Event published when an item in a sale is cancelled.
    /// </summary>
    public class ItemCancelledEvent
    {
        public Guid SaleId { get; set; }
        public Guid SaleItemId { get; set; }
        public DateTime CancelledAt { get; set; } = DateTime.UtcNow;
    }
}
