using System;
using System.ComponentModel.DataAnnotations;

namespace onlinefood.ViewModels.FoodItemVms;

public class UpdateFoodItemVm
{
    [Required]
    public int FoodItemId { get; set; } // this is needed to identify the item to update

    [Required]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }

    public string? ImageUrl { get; set; } // this is optional, if not provided, the existing image will be retained

    public IFormFile? ImageFile { get; set; } // this is optional, if not provided, the existing image will be retained

    [Required]
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public bool IsFeatured { get; set; } = false;

}
