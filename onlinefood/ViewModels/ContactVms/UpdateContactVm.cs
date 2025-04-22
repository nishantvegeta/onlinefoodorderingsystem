using System;

namespace onlinefood.ViewModels.ContactVms;

public class UpdateContactVm
{
    public int ContactId { get; set; }
    public bool IsResolved { get; set; } = false;
}
