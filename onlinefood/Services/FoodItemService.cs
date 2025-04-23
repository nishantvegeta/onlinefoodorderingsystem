using System;
using onlinefood.Services.Interfaces;
using onlinefood.Data;
using onlinefood.Dto.FoodItemDtos;
using onlinefood.Entity;
using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using onlinefood.ViewModels.FoodItemVms;

namespace onlinefood.Services;

public class FoodItemService : IFoodItemService
{
    private readonly FirstRunDbContext dbContext;
    public FoodItemService(FirstRunDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task CreateFoodItem(CreateFoodItemDto foodItemDto)
    {
        using var txn = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var exists = await dbContext.FoodItems
            .AnyAsync(x => x.Name == foodItemDto.Name);
        if (exists)
        {
            throw new Exception("Food item already exists");
        }

        var foodItem = new FoodItems();
        foodItem.Name = foodItemDto.Name;
        foodItem.Description = foodItemDto.Description;
        foodItem.Price = foodItemDto.Price;
        foodItem.CategoryId = foodItemDto.CategoryId;
        foodItem.IsActive = foodItemDto.IsActive;

        if (foodItemDto.ImageFile != null && foodItemDto.ImageFile.Length > 0)
        {
            var uploadsFolder = Path.Combine("wwwroot", "images", "foods");
            Directory.CreateDirectory(uploadsFolder);
            var uniqueName = Guid.NewGuid().ToString() + Path.GetExtension(foodItemDto.ImageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await foodItemDto.ImageFile.CopyToAsync(stream);
            }

            foodItem.ImageUrl = $"/images/foods/{uniqueName}";
        }

        dbContext.FoodItems.Add(foodItem);
        await dbContext.SaveChangesAsync();
        txn.Complete();
    }

    public async Task UpdateFoodItem(int id, UpdateFoodItemDto foodItemDto)
    {
        using var txn = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var foodItem = await dbContext.FoodItems.Where(x => x.FoodItemId == id)
            .FirstOrDefaultAsync();
        if (foodItem == null)
        {
            throw new Exception("Food item not found");
        }

        foodItem.Name = foodItemDto.Name;
        foodItem.Description = foodItemDto.Description;
        foodItem.Price = foodItemDto.Price;
        foodItem.CategoryId = foodItemDto.CategoryId;
        foodItem.ImageUrl = foodItemDto.ImageUrl;
        foodItem.IsActive = foodItemDto.IsActive;

        dbContext.FoodItems.Update(foodItem);
        await dbContext.SaveChangesAsync();
        txn.Complete();
    }

    public async Task DeleteFoodItem(int id)
    {
        using var txn = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var foodItem = await dbContext.FoodItems.Where(x => x.FoodItemId == id)
            .FirstOrDefaultAsync();
        if (foodItem == null)
        {
            throw new Exception("Food item not found");
        }

        dbContext.FoodItems.Remove(foodItem);
        await dbContext.SaveChangesAsync();
        txn.Complete();
    }

    public async Task<List<FoodItemVm>> GetAllFoodItems()
    {
        var foodItems = await dbContext.FoodItems
            .Include(x => x.Category)
            .ToListAsync();

        var foodItemDtos = foodItems.Select(x => new FoodItemVm
        {
            FoodItemId = x.FoodItemId,
            Name = x.Name,
            Description = x.Description,
            Price = x.Price,
            ImageUrl = x.ImageUrl,
            IsActive = x.IsActive,
            CreatedDate = x.CreatedDate,
            CategoryId = x.CategoryId,
            CategoryName = x.Category.Name
        }).ToList();

        return foodItemDtos;
    }

    public async Task<List<FoodItemDto>> SearchFoodItems(string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
        {
            return new List<FoodItemDto>();
        }

        var foodItems = await dbContext.FoodItems
            .Include(x => x.Category)
            .Where(x => x.Name.Contains(searchTerm))
            .ToListAsync();

        var foodItemDtos = foodItems.Select(x => new FoodItemDto
        {
            FoodItemId = x.FoodItemId,
            Name = x.Name,
            Description = x.Description,
            Price = x.Price,
            ImageUrl = x.ImageUrl,
            IsActive = x.IsActive,
            CreatedDate = x.CreatedDate,
            CategoryId = x.CategoryId,
            CategoryName = x.Category.Name
        }).ToList();

        return foodItemDtos;
    }
}

