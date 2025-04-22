using System;
using System.ComponentModel.DataAnnotations;

namespace onlinefood.Dto.CategoryDtos;

public class UpdateCategoryDto
{
    [Required]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Category name is required")]
    public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}
