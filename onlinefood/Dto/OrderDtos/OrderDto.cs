using System;

namespace onlinefood.Dto.OrderDtos;

public class OrderDto
{
    public int OrderId { get; set; }
    public string UserFullName { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Pending";
    public string DeliveryAddress { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}
