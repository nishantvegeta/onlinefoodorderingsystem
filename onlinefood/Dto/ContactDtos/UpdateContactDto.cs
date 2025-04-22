using System;

namespace onlinefood.Dto.ContactDtos;

public class UpdateContactDto
{
    public int ContactId { get; set; }
    public bool IsResolved { get; set; }
}
