using System;
using System.ComponentModel.DataAnnotations;

namespace onlinefood.Dto.FoodItemDtos;

public class UpdateFoodItemDto
{
    [Required]
    public int FoodItemId { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
    public decimal Price { get; set; }

    public string? ImageUrl { get; set; }

    public IFormFile? ImageFile { get; set; }

    public bool IsActive { get; set; } = true; // Default value

    [Required(ErrorMessage = "Category is required")]
    public int CategoryId { get; set; }

    public bool IsFeatured { get; set; } = false;

}
