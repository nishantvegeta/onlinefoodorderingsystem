using System;

namespace onlinefood.Dto.ContactDtos;

public class ContactDto
{
    public int ContactId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
    public DateTime SubmittedAt { get; set; }
}
