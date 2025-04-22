using System;
using System.ComponentModel.DataAnnotations;

namespace onlinefood.ViewModels.PaymentVms;

public class CreatePaymentVm
{
    [Required]
    public int OrderId { get; set; }

    [Required]
    public string Provider { get; set; }

    [Required]
    public string PaymentToken { get; set; }

    [Required]
    public decimal Amount { get; set; }
}
