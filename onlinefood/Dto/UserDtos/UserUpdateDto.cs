using System;
using System.ComponentModel.DataAnnotations;

namespace onlinefood.Dto.UserDtos;

public class UserUpdateDto
{
    public int UserId { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public string Password { get; set; }

    [Phone]
    public string Phone { get; set; }

    public bool IsVerified { get; set; }
}
