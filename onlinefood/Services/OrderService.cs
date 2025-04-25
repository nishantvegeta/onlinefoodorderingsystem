using System;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using onlinefood.Data;
using onlinefood.Dto.OrderDtos;
using onlinefood.Entity;
using onlinefood.Services.Interfaces;
using onlinefood.ViewModels.OrderVms;

namespace onlinefood.Services.Interfaces
{
    public class OrderService : IOrderService
    {
        private readonly FirstRunDbContext dbContext;

        public OrderService(FirstRunDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateOrder(PlaceOrderDto dto, int userId)
        {
            using var txn = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var user = await dbContext.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
                throw new Exception("User not found");


            // Get user cart items
            var cartItems = await dbContext.CartItems
                .Include(ci => ci.FoodItem)
                .Where(ci => ci.UserId == userId)
                .ToListAsync();

            if (!cartItems.Any())
                throw new Exception("Cart is empty");

            var order = new Orders
            {
                UserId = userId,
                CustomerName = user.Name,
                DeliveryAddress = dto.DeliveryAddress,
                TotalAmount = dto.TotalAmount,
                Email = dto.Email,
                Phone = dto.Phone,
                OrderDate = DateTime.UtcNow,
                Status = "Pending"
            };

            await dbContext.Orders.AddAsync(order);
            await dbContext.SaveChangesAsync();

            // Create order items from cart items
            var orderItems = cartItems.Select(ci => new OrderDetails
            {
                OrderId = order.OrderId,
                FoodItemId = ci.FoodItemId,
                Quantity = ci.Quantity,
                PriceAtOrderTimex = ci.FoodItem.Price,
            }).ToList();

            await dbContext.OrderDetails.AddRangeAsync(orderItems);

            // Clear the cart
            dbContext.CartItems.RemoveRange(cartItems);

            await dbContext.SaveChangesAsync();
            txn.Complete();
        }

        public async Task<OrderVm> GetOrderById(int userId, int orderId)
        {
            var order = await dbContext.Orders
                .Where(x => x.UserId == userId && x.OrderId == orderId)
                .Select(x => new OrderVm
                {
                    OrderId = x.OrderId,
                    CustomerName = x.CustomerName,
                    DeliveryAddress = x.DeliveryAddress,
                    TotalAmount = x.TotalAmount,
                    OrderDate = x.OrderDate,
                    Status = x.Status,
                })
                .FirstOrDefaultAsync();

            if (order == null)
                throw new Exception("Order not found");

            return order;
        }

        public async Task UpdateOrderStatus(UpdateOrderStatusDto dto)
        {
            using var txn = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var order = await dbContext.Orders
                .FirstOrDefaultAsync(x => x.OrderId == dto.OrderId);

            if (order == null)
                throw new Exception("Order not found");

            order.Status = dto.Status;

            await dbContext.SaveChangesAsync();
            txn.Complete();
        }

        public async Task<List<OrderVm>> GetAllOrders()
        {
            var orders = await dbContext.Orders
                .Include(o => o.User)
                .Select(x => new OrderVm
                {
                    OrderId = x.OrderId,
                    CustomerName = x.User.Name,
                    TotalAmount = x.TotalAmount,
                    OrderDate = x.OrderDate,
                    Status = x.Status,
                })
                .ToListAsync();

            return orders;
        }
        
        public async Task<List<OrderVm>> GetOrdersByUserId(int userId)
        {
            var orders = await dbContext.Orders
                .Where(x => x.UserId == userId)
                .Select(x => new OrderVm
                {
                    OrderId = x.OrderId,
                    CustomerName = x.CustomerName,
                    TotalAmount = x.TotalAmount,
                    OrderDate = x.OrderDate,
                    Status = x.Status,
                })
                .ToListAsync();

            return orders;
        }
    }
}
