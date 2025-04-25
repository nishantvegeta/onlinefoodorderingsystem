using System;

namespace onlinefood.ViewModels.OrderVms;

public class OrderVm
{
    public int OrderId { get; set; }
    public decimal TotalAmount { get; set; }
    public string CustomerName { get; set; }
    public string Status { get; set; }
    public DateTime OrderDate { get; set; }
    public string DeliveryAddress { get; set; }
}
