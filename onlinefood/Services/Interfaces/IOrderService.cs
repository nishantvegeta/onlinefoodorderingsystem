using System;
using onlinefood.Dto.OrderDtos;
using onlinefood.ViewModels.OrderVms;

namespace onlinefood.Services.Interfaces;

public interface IOrderService
{
    Task CreateOrder(PlaceOrderDto dto, int userId);
    Task<OrderDetailsVm> GetOrderById(int userId, int orderId);
    Task UpdateOrderStatus(UpdateOrderStatusDto dto);
    Task<List<OrderVm>> GetAllOrders();
    // Get orders by user ID (for customer use)
    Task<List<OrderVm>> GetOrdersByUserId(int userId); // Fetch orders specific to the logged-in user
    Task CancelOrder(int orderId, int userID); // Delete an order by its ID
}
