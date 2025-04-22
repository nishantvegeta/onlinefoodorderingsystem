using System;
using onlinefood.Entity;
using onlinefood.Dto.FoodItemDtos;

namespace onlinefood.Services.Interfaces;

public interface IFoodItemService
{
    Task CreateFoodItem(CreateFoodItemDto foodItemDto);
    Task UpdateFoodItem(int id, UpdateFoodItemDto foodItemDto);
    Task DeleteFoodItem(int id);
    Task<List<FoodItemDto>> GetAllFoodItems();
    Task<List<FoodItemDto>> SearchFoodItems(string searchTerm);

}
