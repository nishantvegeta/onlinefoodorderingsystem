using System;

namespace onlinefood.ViewModels.UserVms;

public class UpdateUserVm
{
    public string Name { get; set; }
    public string Email { get; set; }

    public string Password { get; set; }
    public string Phone { get; set; }

    public bool IsVerified { get; set; } 
}
