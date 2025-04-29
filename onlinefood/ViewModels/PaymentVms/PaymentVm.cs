using System;
using System.ComponentModel.DataAnnotations;
using onlinefood.Enums;

namespace onlinefood.ViewModels.PaymentVms;

public class PaymentVm
{
    public int PaymentId { get; set; }
    public int OrderId { get; set; }
    public string OrderStatus { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string PaymentToken { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; }
    public DateTime PaidAt { get; set; }
}
