using System;
using onlinefood.Dto.PaymentDtos;

namespace onlinefood.ViewModels.PaymentVms;

public class PaymentListVm
{
    public List<PaymentDto> Payments { get; set; } = new();
}   
