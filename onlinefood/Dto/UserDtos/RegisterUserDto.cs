using System;
using System.ComponentModel.DataAnnotations;

namespace onlinefood.Dto.UserDtos;

public class RegisterUserDto
{
    [Required]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "Password should be at least 6 characters long.")]
    public string Password { get; set; }

    [Required]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }

    [Required]
    [Phone]
    public string Phone { get; set; }

    public string Role { get; set; } = "User";

    public bool IsVerified { get; set; } = false;
}
