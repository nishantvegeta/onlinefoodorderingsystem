using System;
using System.ComponentModel.DataAnnotations;

namespace onlinefood.Dto.ContactDtos;

public class CreateContactDto
{
    [Required]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public string Email { get; set; }

    public string Phone { get; set; }

    [Required(ErrorMessage = "Subject is required")]
    public string Subject { get; set; }

    [Required(ErrorMessage = "Message is required")]
    public string Message { get; set; }
}
