using System;
using onlinefood.Services.Interfaces;
using onlinefood.Data;
using onlinefood.ViewModels.CartItemVms;
using onlinefood.Entity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace onlinefood.Services;

public class CartService : ICartService
{
    private readonly FirstRunDbContext dbContext;
    private readonly IUserService userService;

    public CartService(FirstRunDbContext dbContext, IUserService userService)
    {
        this.userService = userService;
        this.dbContext = dbContext;
    }

    public async Task AddToCart(int userId, int foodItemId, int quantity)
    {
        try
        {
            var foodItem = await dbContext.FoodItems.FindAsync(foodItemId);
            if (foodItem == null)
            {
                throw new Exception("Food item not found");
            }

            var cartItem = await dbContext.CartItems
                .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.FoodItemId == foodItemId);

            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than 0.");
            }

            if (cartItem == null)
            {
                var newcartItem = new CartItems
                {
                    UserId = userId,
                    FoodItemId = foodItemId,
                    Quantity = quantity
                };
                dbContext.CartItems.Add(newcartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
                dbContext.CartItems.Update(cartItem);
            }

            await dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
            throw;
        }
    }

    public async Task RemoveFromCart(int userId, int foodItemId)
    {
        var cartItem = await dbContext.CartItems
            .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.FoodItemId == foodItemId);

        if (cartItem != null)
        {
            dbContext.CartItems.Remove(cartItem);
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task ClearCart(int userId)
    {
        var cartItems = await dbContext.CartItems
            .Where(ci => ci.UserId == userId)
            .ToListAsync();

        if (cartItems.Count > 0)
        {
            dbContext.CartItems.RemoveRange(cartItems);
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task<List<CartItemVm>> GetCartItems(int userId)
    {
        var cartItems = await dbContext.CartItems
            .Include(ci => ci.FoodItem)
            .Where(ci => ci.UserId == userId)
            .Select(ci => new CartItemVm
            {
                FoodItemId = ci.FoodItemId,
                FoodName = ci.FoodItem.Name,
                Quantity = ci.Quantity,
                Price = ci.FoodItem.Price
            })
            .ToListAsync();

        return cartItems;
    }

    public async Task<decimal> GetTotalPrice(int userId)
    {
        var total = await dbContext.CartItems
            .Include(ci => ci.FoodItem)
            .Where(ci => ci.UserId == userId)
            .SumAsync(ci => ci.Quantity * ci.FoodItem.Price);

        return total;
    }

    public async Task PlaceOrder(int userId)
    {
        var cartItems = await dbContext.CartItems
            .Include(ci => ci.FoodItem)
            .Where(ci => ci.UserId == userId)
            .ToListAsync();

        if (cartItems.Count == 0)
        {
            throw new Exception("Cart is empty");
        }

        var order = new Orders
        {
            UserId = userId,
            OrderDate = DateTime.Now,
            TotalAmount = cartItems.Sum(ci => ci.Quantity * ci.FoodItem.Price),
            OrderDetails = cartItems.Select(ci => new OrderDetails
            {
                FoodItemId = ci.FoodItemId,
                Quantity = ci.Quantity,
                PriceAtOrderTimex = ci.FoodItem.Price
            }).ToList(),
        };

        dbContext.Orders.Add(order);
        dbContext.CartItems.RemoveRange(cartItems);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateQuantity(int userId, int foodItemId, int quantity)
    {
        var cartItem = await dbContext.CartItems
            .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.FoodItemId == foodItemId);

        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than 0.");
        }
        if (cartItem != null)
        {
            cartItem.Quantity = quantity;
            dbContext.CartItems.Update(cartItem);
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task<int> GetCartItemCount(int userId)
    {
        var count = await dbContext.CartItems
            .Where(ci => ci.UserId == userId)
            .SumAsync(ci => ci.Quantity);

        return count;
    }
}
