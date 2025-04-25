using System;
using onlinefood.ViewModels.CartItemVms;

namespace onlinefood.Services.Interfaces;

public interface ICartService
{
    Task AddToCart(int UserId, int foodItemId, int quantity);
    Task RemoveFromCart(int foodItemId);
    Task ClearCart();
    Task<List<CartItemVm>> GetCartItems();
    Task<decimal> GetTotalPrice();
    Task PlaceOrder(int userId);
    Task UpdateQuantity(int foodItemId, int quantity);
    Task<decimal> GetCartTotal();
}
