using System.Runtime.Serialization;

namespace Orders.Domain.Entities
{
    public enum OrderStatus
    {
        PendingPayment,
        Paid,
        Processing,
        Shipped,
        Delivered,
        Canceled
    }
}