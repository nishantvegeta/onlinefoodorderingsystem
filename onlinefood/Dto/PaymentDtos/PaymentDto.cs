using System;

namespace onlinefood.Dto.PaymentDtos;

public class PaymentDto
{
    public int PaymentId { get; set; }
    public int OrderId { get; set; }
    public string OrderStatus { get; set; }
    public string Provider { get; set; }
    public string PaymentToken { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; }
    public DateTime PaidAt { get; set; }
}
