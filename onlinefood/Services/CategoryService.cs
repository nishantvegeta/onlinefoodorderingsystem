using System;
using onlinefood.Services.Interfaces;
using onlinefood.Entity;
using onlinefood.Dto.CategoryDtos;
using Microsoft.EntityFrameworkCore;
using onlinefood.Data;
using System.Transactions;

namespace onlinefood.Services;

public class CategoryService : ICategoryService
{
    private readonly FirstRunDbContext dbContext;
    public CategoryService(FirstRunDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<List<CategoryDto>> GetAllCategories()
    {
        var categories = await dbContext.Categories
            .AsNoTracking()
            .ToListAsync();

        var dto = categories.Select(c => new CategoryDto
        {
            CategoryId = c.CategoryId,
            Name = c.Name,
            IsActive = c.IsActive
        }).ToList();
        return dto;
    }

    public async Task<CategoryDto> GetCategoryById(int id)
    {
        var category = await dbContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.CategoryId == id);

        if (category == null)
        {
            throw new Exception("Category not found");
        }

        var dto = new CategoryDto
        {
            CategoryId = category.CategoryId,
            Name = category.Name,
            IsActive = category.IsActive
        };
        return dto;
    }

    public async Task<List<CategoryDto>> SearchCategories(string searchTerm)
    {
        var categories = await dbContext.Categories
            .AsNoTracking()
            .Where(c => c.Name.Contains(searchTerm))
            .ToListAsync();

        var dto = categories.Select(c => new CategoryDto
        {
            CategoryId = c.CategoryId,
            Name = c.Name,
            IsActive = c.IsActive
        }).ToList();
        return dto;
    }

    public async Task Create(CreateCategoryDto categoryDto)
    {
        var txn = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var exists = await dbContext.Categories
            .AsNoTracking()
            .AnyAsync(c => c.Name == categoryDto.Name);

        if (exists)
        {
            throw new Exception("Category already exists");
        }

        var category = new Categories();
        category.Name = categoryDto.Name;
        category.CreatedDate = DateTime.UtcNow;
        category.IsActive = true;

        dbContext.Categories.Add(category);
        await dbContext.SaveChangesAsync();
        txn.Complete();
    }

    public async Task Update(int id, UpdateCategoryDto categoryDto)
    {
        var txn = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var category = await dbContext.Categories
            .Where(c => c.CategoryId == id)
            .FirstOrDefaultAsync();

        if (category == null)
        {
            throw new Exception("Category not found");
        }

        category.Name = categoryDto.Name;
        category.IsActive = categoryDto.IsActive;
        dbContext.Categories.Update(category);
        await dbContext.SaveChangesAsync();
        txn.Complete();
    }

    public async Task Delete(int id)
    {
        var txn = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var category = await dbContext.Categories
            .Where(c => c.CategoryId == id)
            .FirstOrDefaultAsync();

        if (category == null)
        {
            throw new Exception("Category not found");
        }

        dbContext.Categories.Remove(category);
        await dbContext.SaveChangesAsync();
        txn.Complete();
    }

    
}
