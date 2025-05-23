using System;
using System.ComponentModel.DataAnnotations;
using onlinefood.Enums;

namespace onlinefood.ViewModels.PaymentVms;

public class CreatePaymentVm
{
    [Required]
    public int OrderId { get; set; }

    [Required]
    public PaymentMethod PaymentMethod{ get; set; }

    [Required]
    public string PaymentToken { get; set; }

    [Required]
    public decimal Amount { get; set; }
}
