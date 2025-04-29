using System;
using System.ComponentModel.DataAnnotations;
using onlinefood.Enums;

namespace onlinefood.Dto.PaymentDtos;

public class CreatePaymentDto
{
    [Required]
    public int OrderId { get; set; }

    [Required]
    public PaymentMethod PaymentMethod { get; set; } 

    [Required]
    public string PaymentToken { get; set; } = string.Empty;

    [Required]
    public decimal Amount { get; set; }
}
