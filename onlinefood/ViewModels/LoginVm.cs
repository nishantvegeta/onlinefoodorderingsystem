using System;
using System.ComponentModel.DataAnnotations;

namespace onlinefood.ViewModels;

public class LoginVm
{
    [Required]
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
