using System;
using System.ComponentModel.DataAnnotations;

namespace onlinefood.Entity;

public class EmailVerification
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    public Users User { get; set; }

    [Required]
    public string VerificationCode { get; set; } 

    public DateTime ExpiryDate {get; set; } = DateTime.UtcNow.AddHours(1); // Code is valid for 1 hour
    public bool IsVerified { get; set; } = false;
}
