using System;
using onlinefood.Entity;
using onlinefood.Dto.FoodItemDtos;
using onlinefood.ViewModels.FoodItemVms;

namespace onlinefood.Services.Interfaces;

public interface IFoodItemService
{
    Task CreateFoodItem(CreateFoodItemDto foodItemDto);
    Task UpdateFoodItem(int id, UpdateFoodItemDto foodItemDto);
    Task DeleteFoodItem(int id);
    Task<List<FoodItemVm>> GetAllFoodItems();
    Task<List<FoodItemVm>> SearchFoodItems(string searchTerm);
    Task<List<FoodItemVm>> GetFeaturedFoodItems();
    Task<List<FoodItemVm>> GetFoodItemsByCategory(int categoryId);
    Task<FoodItemVm> GetFoodItemById(int id);
    Task<List<FoodItemVm>> GetAllActiveFoodItems();
}
