using System;
using System.ComponentModel.DataAnnotations;

namespace onlinefood.Entity;

public class Users
{
    [Key]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string Role { get; set; } = "User"; // Default role is User

    public bool IsVerified { get; set; } = false;
}
