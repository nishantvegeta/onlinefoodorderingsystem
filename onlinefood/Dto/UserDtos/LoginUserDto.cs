using System;
using System.ComponentModel.DataAnnotations;

namespace onlinefood.Dto.UserDtos;

public class LoginUserDto
{
    [Required]
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
