using System;
using System.ComponentModel.DataAnnotations;

namespace onlinefood.ViewModels.OrderVms;

public class UpdateOrderStatusVm
{
    [Required]
    public int OrderId { get; set; }

    [Required(ErrorMessage = "Please select a status.")]
    public string Status { get; set; }

    // Optional: You can use this to populate a dropdown of available statuses
    public List<string> AvailableStatuses { get; set; } = new()
    {
        "Pending", "Processing", "Delivered", "Cancelled"
    };
}
