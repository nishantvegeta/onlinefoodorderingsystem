using System;
using System.ComponentModel.DataAnnotations;
using onlinefood.Enums;

namespace onlinefood.Dto.OrderDtos;

public class PlaceOrderDto
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

    [Required]
    public List<int> CartItemIds { get; set; }

    [Required(ErrorMessage = "Select a payment method")]
    public PaymentMethod PaymentMethod { get; set; } // Assuming PaymentMethod is an enum, use the appropriate type here
}
