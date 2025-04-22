using System;
using System.ComponentModel.DataAnnotations;

namespace onlinefood.Dto.FoodItemDtos;

public class CreateFoodItemDto
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Price is required")]
    public decimal Price { get; set; }

    public string ImageUrl { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    [Required(ErrorMessage = "Category is required")]
    public int CategoryId { get; set; }
}
