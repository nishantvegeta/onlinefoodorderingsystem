using System;

namespace onlinefood.ViewModels.CartItemVms;

public class CartItemVm
{
    public int CartItemId { get; set; }
    public int FoodItemId { get; set; }
    public string FoodName { get; set; }
    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public decimal Total => Price * Quantity;

    public int UserId { get; set; }
    public string UserName { get; set; }
}
