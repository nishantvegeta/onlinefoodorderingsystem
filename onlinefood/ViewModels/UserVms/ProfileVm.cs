using System;
using System.ComponentModel.DataAnnotations;

namespace onlinefood.ViewModels.UserVms;

public class ProfileVm
{
    public int UserId { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters")]
    public string Email { get; set; } = string.Empty;

    // We don't expose the password directly in ProfileVm, but we could provide a way to change it separately.
    public string Password { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public string Role { get; set; } = "User"; // Default role

    public bool IsVerified { get; set; } = false;

    // Optional: Add a property for the "current" password if you plan to let the user change it.
    [Compare("Password", ErrorMessage = "Passwords must match.")]
    public string ConfirmPassword { get; set; } = string.Empty; // For changing password
    public string CurrentPassword { get; set; } = string.Empty; // For changing password
}
