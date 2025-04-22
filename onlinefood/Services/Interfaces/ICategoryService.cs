using System;
using onlinefood.Entity;
using onlinefood.Dto.CategoryDtos;

namespace onlinefood.Services.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllCategories();
    Task<CategoryDto> GetCategoryById(int id);
    Task<List<CategoryDto>> SearchCategories(string searchTerm);
    Task Create(CreateCategoryDto categoryDto);
    Task Update(int id, UpdateCategoryDto categoryDto);
    Task Delete(int id);
}
