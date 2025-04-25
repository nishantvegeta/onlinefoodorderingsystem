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
    public bool IsVerified { get; set; }
    public string Password { get; set; } = string.Empty; // For changing password

}
