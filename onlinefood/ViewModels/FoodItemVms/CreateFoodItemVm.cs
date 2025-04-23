using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using onlinefood.Entity;

namespace onlinefood.ViewModels.FoodItemVms;

public class CreateFoodItemVm
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Required]
    public decimal Price { get; set; }

    public IFormFile? ImageFile { get; set; }

    [Required]
    public int CategoryId { get; set; }

    public bool IsActive { get; set; } = true;

    public SelectList CategoriesSelectList()
        => new SelectList(
            Categorie,
            nameof(Categories.CategoryId),
            nameof(Categories.Name),
            CategoryId   
        );

    public List<Categories> Categorie;

}
