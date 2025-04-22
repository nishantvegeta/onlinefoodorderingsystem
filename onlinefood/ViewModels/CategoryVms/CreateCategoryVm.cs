using System;
using System.ComponentModel.DataAnnotations;

namespace onlinefood.ViewModels.CategoryVms;

public class CreateCategoryVm
{
    [Required(ErrorMessage = "Category name is required")]
    public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}
