using System;
using onlinefood.Entity;
using onlinefood.Dto.CategoryDtos;
using onlinefood.ViewModels.CategoryVms;

namespace onlinefood.Services.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryVm>> GetAllCategories();
    Task<CategoryDto> GetCategoryById(int id);
    Task<List<CategoryDto>> SearchCategories(string searchTerm);
    Task Create(CreateCategoryDto categoryDto);
    Task Update(int id, UpdateCategoryDto categoryDto);
    Task Delete(int id);
}
