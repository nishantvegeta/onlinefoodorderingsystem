using System;
using System.ComponentModel.DataAnnotations;

namespace onlinefood.ViewModels.AccountVms;

public class LoginVm
{
    [Required]
    public string Username { get; set; }
    public string Email { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
