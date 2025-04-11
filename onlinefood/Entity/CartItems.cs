using System;
using System.ComponentModel.DataAnnotations;

namespace onlinefood.Entity;

public class CartItems
{
    [Key]
    public int CartItemId { get; set; }

    public int FoodItemId { get; set; }
    public FoodItems FoodItem { get; set; }

    public int Quantity { get; set; }

    public int UserId { get; set; }
    public Users User { get; set; }
}
