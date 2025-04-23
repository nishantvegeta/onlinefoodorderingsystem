using System;
using onlinefood.Dto.OrderDtos;
using onlinefood.ViewModels.OrderVms;

namespace onlinefood.Services.Interfaces;

public interface IOrderService
{
    Task CreateOrder(CreateOrderDto dto);
    Task<OrderDto> GetOrderById(int orderId);
    Task UpdateOrderStatus(UpdateOrderStatusDto dto);
    Task<List<OrderVm>> GetAllOrders();
}
