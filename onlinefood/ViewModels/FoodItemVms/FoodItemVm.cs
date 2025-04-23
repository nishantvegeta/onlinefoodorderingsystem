using System;
using System.ComponentModel.DataAnnotations;

namespace onlinefood.ViewModels.FoodItemVms;

public class FoodItemVm
{
    public int FoodItemId { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }

    public string ImageUrl { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; }

    [Required(ErrorMessage = "Category is required")]
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;
}
