using System;
using System.ComponentModel.DataAnnotations;

namespace onlinefood.ViewModels.OrderVms;

public class CreateOrderVm
{
    [Required]
    public int UserId { get; set; }

    public string UserName { get; set; } = string.Empty;

    [Required]
    public decimal TotalAmount { get; set; }

    [Required(ErrorMessage = "Delivery address is required")]
    public string DeliveryAddress { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public string Email { get; set; }

    [Required(ErrorMessage = "Phone number is required")]
    [Phone]
    public string Phone { get; set; }
}
