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
    public string Password { get; set; }

    public string Role { get; set; } = "User";

    public bool IsVerified { get; set; } = false;

    [Required]
    [Phone]
    public string Phone { get; set; }
}
