using System;

namespace onlinefood.ViewModels.AdminVms;

public class AdminProfileVm
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public bool IsVerified { get; set; }
    public DateTime CreatedAt { get; set; }
}
