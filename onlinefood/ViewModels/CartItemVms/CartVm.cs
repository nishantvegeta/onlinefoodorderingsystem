using System;

namespace onlinefood.ViewModels.CartItemVms;

public class CartVm
{
    public List<CartItemVm> CartItems { get; set; } // List of CartItemVm
    public decimal TotalPrice { get; set; }
}
