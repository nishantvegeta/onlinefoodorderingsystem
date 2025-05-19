using System;
using onlinefood.Enums;

namespace onlinefood.ViewModels.PaymentVms;

public class ChoosePaymentVm
{
    public decimal TotalAmount { get; set; }
    public PaymentMethod? SelectedPaymentMethod { get; set; }
}
