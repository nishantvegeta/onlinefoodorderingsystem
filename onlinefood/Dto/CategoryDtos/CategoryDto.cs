using System;
using System.ComponentModel.DataAnnotations;

namespace onlinefood.Dto.CategoryDtos;

public class CategoryDto
{
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Category name is required")]
    public string Name { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }

    public bool IsActive { get; set; }
}
