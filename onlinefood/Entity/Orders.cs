using System;
using System.ComponentModel.DataAnnotations;

namespace onlinefood.Entity;

public class Orders
{
    [Key]
    public int OrderId { get; set; }
    public int UserId { get; set; }
    public Users User { get; set; }

    [Required(ErrorMessage = "name is required")]
    public string CustomerName { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Pending"; // Default status is Pending

    [Required(ErrorMessage = "Delivery address is required")]
    public string DeliveryAddress { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Phone number is required")]
    public string Phone { get; set; }
}
