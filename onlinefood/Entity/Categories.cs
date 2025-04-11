using System;
using System.ComponentModel.DataAnnotations;

namespace onlinefood.Entity;

public class Categories
{
    [Key]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
}
