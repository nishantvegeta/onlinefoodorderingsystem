using System;
using System.ComponentModel.DataAnnotations;

namespace onlinefood.Entity;

public class Payments
{   
    [Key]
    public int PaymentId { get; set; }
    public int OrderId { get; set; }
    public Orders Order { get; set; }
    public string Provider { get; set; } = string.Empty;
    public string PaymentToken { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Status { get; set; } = "Failed";
    public DateTime PaidAt { get; set; } = DateTime.UtcNow;
}
