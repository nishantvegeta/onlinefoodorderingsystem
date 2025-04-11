using System;
using System.ComponentModel.DataAnnotations;

namespace onlinefood.Entity;

public class FoodItems
{
    [Key]
    public int FoodItemId { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Price is required")]
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; }
    public int CategoryId { get; set; }
    public Categories Category { get; set; }

}
