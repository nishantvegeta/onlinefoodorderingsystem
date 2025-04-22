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
    public decimal Price { get; set; }

    public string ImageUrl { get; set; } = string.Empty;

    [Required]
    public int CategoryId { get; set; }

    public bool IsActive { get; set; } = true;

}
