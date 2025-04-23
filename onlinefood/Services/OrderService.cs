using System;
using onlinefood.Dto.OrderDtos;
using onlinefood.Services.Interfaces;
using onlinefood.Data;
using System.Transactions;
using onlinefood.Entity;
using Microsoft.EntityFrameworkCore;
using onlinefood.ViewModels.OrderVms;

namespace onlinefood.Services.Interfaces;

public class OrderService : IOrderService
{
    private readonly FirstRunDbContext dbContext;

    public OrderService(FirstRunDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task CreateOrder(CreateOrderDto dto)
    {
        using var txn = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var order = new Orders();
        order.UserId = dto.UserId;
        order.CustomerName = dto.UserName;
        order.DeliveryAddress = dto.DeliveryAddress;
        order.TotalAmount = dto.TotalAmount;
        order.OrderDate = DateTime.UtcNow;
        order.Status = "Pending";

        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync();
        txn.Complete();
    }

    public async Task<OrderDto> GetOrderById(int orderId)
    {
        var order = await dbContext.Orders
            .Where(x => x.OrderId == orderId)
            .Select(x => new OrderDto
            {
                OrderId = x.OrderId,
                UserFullName = x.User.Name,
                TotalAmount = x.TotalAmount,
                DeliveryAddress = x.DeliveryAddress,
                OrderDate = x.OrderDate,
                Status = x.Status,
                Email = x.Email,
                Phone = x.Phone
            })
            .FirstOrDefaultAsync();

        if (order == null)
        {
            throw new Exception("Order not found");
        }

        return order;
    }

    public async Task UpdateOrderStatus(UpdateOrderStatusDto dto)
    {
        using var txn = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var order = await dbContext.Orders
            .Where(x => x.OrderId == dto.OrderId)
            .FirstOrDefaultAsync();

        if (order == null)
        {
            throw new Exception("Order not found");
        }

        order.Status = dto.Status;
        await dbContext.SaveChangesAsync();
        txn.Complete();
    }

    public async Task<List<OrderVm>> GetAllOrders()
    {
        var order = await dbContext.Orders
            .Select(x => new OrderVm
            {
                OrderId = x.OrderId,
                CustomerName = x.User.Name,
                TotalAmount = x.TotalAmount,
                OrderDate = x.OrderDate,
                Status = x.Status,
            })
            .ToListAsync();

        return order;
    }
}
