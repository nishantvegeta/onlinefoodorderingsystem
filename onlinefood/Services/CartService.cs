using System;
using onlinefood.Services.Interfaces;
using onlinefood.Data;
using onlinefood.ViewModels.CartItemVms;
using onlinefood.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace onlinefood.Services;

public class CartService : ICartService
{
    private readonly FirstRunDbContext dbContext;

    public CartService(FirstRunDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task AddToCart(int UserId, int foodItemId, int quantity)
    {
        var foodItem = await dbContext.FoodItems.FindAsync(foodItemId);
        if (foodItem == null)
        {
            throw new Exception("Food item not found");
        }

        var cartItem = await dbContext.CartItems
            .FirstOrDefaultAsync(ci => ci.UserId == UserId && ci.FoodItemId == foodItemId);

        if (cartItem == null)
        {
            var newcartItem = new CartItems
            {
                UserId = UserId,
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

    public async Task RemoveFromCart(int foodItemId)
    {
        var cartItem = await dbContext.CartItems.FindAsync(foodItemId);
        if (cartItem != null)
        {
            dbContext.CartItems.Remove(cartItem);
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task ClearCart()
    {
        var cartItems = await dbContext.CartItems.ToListAsync();
        dbContext.CartItems.RemoveRange(cartItems);
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<CartItemVm>> GetCartItems()
    {
        var cartItems = await dbContext.CartItems
            .Include(ci => ci.FoodItem)
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

    public async Task<decimal> GetTotalPrice()
    {
        var total = await dbContext.CartItems
            .Include(ci => ci.FoodItem)
            .SumAsync(ci => ci.Quantity * ci.FoodItem.Price);

        return total;
    }

    public async Task PlaceOrder(int userId)
    {
        // Get the cart items for the user
    }

    public async Task UpdateQuantity(int foodItemId, int quantity)
    {
        var cartItem = await dbContext.CartItems.FindAsync(foodItemId);
        if (cartItem != null)
        {
            cartItem.Quantity = quantity;
            dbContext.CartItems.Update(cartItem);
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task<decimal> GetCartTotal()
    {
        var total = await dbContext.CartItems
            .Include(ci => ci.FoodItem)
            .SumAsync(ci => ci.Quantity * ci.FoodItem.Price);

        return total;
    }

    

}
