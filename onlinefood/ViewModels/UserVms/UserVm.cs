using System;
using System.ComponentModel.DataAnnotations;

namespace onlinefood.ViewModels.UserVms;

public class UserVm
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public string Email { get; set; }
    public string Role { get; set; } = "User"; // Default role is User

    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Invalid phone number format")]
    public string Phone { get; set; } = string.Empty;
    public bool IsVerified { get; set; }
    public string Password { get; set; } = string.Empty; // For changing password

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;// Default to current UTC time

}
