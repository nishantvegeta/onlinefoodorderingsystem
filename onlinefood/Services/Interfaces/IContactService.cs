using System;
using onlinefood.Dto.ContactDtos;
using onlinefood.ViewModels.ContactVms;

namespace onlinefood.Services.Interfaces;

public interface IContactService
{
    Task CreateContact(CreateContactDto dto);
    Task<List<ContactVm>> GetAllContacts(); // For admin

    Task UpdateContactStatus(UpdateContactDto dto); // For admin

    Task DeleteContact(int id); // For admin
}
