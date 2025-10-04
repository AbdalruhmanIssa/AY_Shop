using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYShop.DAL.Models
{
    public enum OrderStatusEnum
    {
        Pending = 1,
        Canceled = 2,
        Approved = 3,
        Shipped = 4,
        Delivered = 5
    }

    public enum PaymentMethodEnum
    {
        Cash = 1,
        Visa = 2
    }

    public class Order
    {
        // Order
        public int Id { get; set; }
        public OrderStatusEnum Status { get; set; } = OrderStatusEnum.Pending;
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public DateTime? ShippedDate { get; set; }
        public decimal TotalPrice { get; set; }

        // Payment
        public PaymentMethodEnum PaymentMethod { get; set; }
        public string? PaymentId { get; set; }

        // Carrier
        public string? CarrierName { get; set; }
        public string? TrackingNumber { get; set; }

        // Relation
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public List<OrderItem> orderItems { get; set; }= new List<OrderItem>();

    }

}
