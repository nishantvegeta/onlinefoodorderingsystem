using System;
using System.ComponentModel.DataAnnotations;

namespace onlinefood.Entity;

public class OrderDetails
{
    [Key]
    public int OrderDetailId { get; set; }

    public int OrderId { get; set; }
    public Orders Order { get; set; }
    public int FoodItemId { get; set; }
    public FoodItems FoodItem { get; set; }

    [Required(ErrorMessage = "Quantity is required")]
    public int Quantity { get; set; }
    public decimal PriceAtOrderTimex { get; set; }
}
