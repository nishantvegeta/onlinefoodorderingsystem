using System;
using onlinefood.Enums;
using onlinefood.ViewModels.CartItemVms;

namespace onlinefood.Services.Interfaces;

public interface ICartService
{
    Task AddToCart(int userId, int foodItemId, int quantity);
    Task RemoveFromCart(int userId, int foodItemId);
    Task ClearCart(int userId);
    Task<List<CartItemVm>> GetCartItems(int userId);
    Task<decimal> GetTotalPrice(int userId);
    Task PlaceOrder(int userId, PaymentMethod paymentMethod);
    Task UpdateQuantity(int userId, int foodItemId, int quantity);
    Task<int> GetCartItemCount(int userId);
}
