using System;
using onlinefood.Dto.ContactDtos;

namespace onlinefood.Services.Interfaces;

public interface IContactService
{
    Task CreateContact(CreateContactDto dto);
    Task<List<ContactDto>> GetAllContacts(); // For admin

    Task UpdateContactStatus(UpdateContactDto dto); // For admin

    Task DeleteContact(int id); // For admin
}
