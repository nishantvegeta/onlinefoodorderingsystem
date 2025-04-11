using System;
using System.ComponentModel.DataAnnotations;

namespace onlinefood.Entity;

public class Contacts
{
    [Key]
    public int ContactId { get; set; }
    
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Subject is required")]
    public string Subject { get; set; } = string.Empty;

    [Required(ErrorMessage = "Message is required")]
    public string Message { get; set; } = string.Empty;
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    public int UserId { get; set; }
    public Users User { get; set; } = new Users();
}
