using System;
using System.ComponentModel.DataAnnotations;

namespace onlinefood.Dto.FoodItemDtos;

public class FoodItemDto
{
   public int FoodItemId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public decimal Price { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }

        public bool IsActive { get; set; } = true; // Default value can be set here if desired

        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;
}

