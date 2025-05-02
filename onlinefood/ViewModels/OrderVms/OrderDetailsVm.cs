using System;
using onlinefood.Enums;

namespace onlinefood.ViewModels.OrderVms
{
    public class OrderDetailsVm
    {
        // Order info
        public int OrderId { get; set; }
        public string Status { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        // User info
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }

        // Address (optional)
        public string DeliveryAddress { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        // Ordered items
        public List<OrderItemVm> OrderItems { get; set; }
    }

    public class OrderItemVm
    {
        public string FoodName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total => Quantity * Price;
    }
}
