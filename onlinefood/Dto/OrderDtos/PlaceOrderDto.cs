using System;
using System.ComponentModel.DataAnnotations;

namespace onlinefood.Dto.OrderDtos;

public class PlaceOrderDto
{
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
}
