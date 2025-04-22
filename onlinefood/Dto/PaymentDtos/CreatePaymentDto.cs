using System;
using System.ComponentModel.DataAnnotations;

namespace onlinefood.Dto.PaymentDtos;

public class CreatePaymentDto
{
    [Required]
    public int OrderId { get; set; }

    [Required]
    public string Provider { get; set; } = string.Empty;

    [Required]
    public string PaymentToken { get; set; } = string.Empty;

    [Required]
    public decimal Amount { get; set; }
}
