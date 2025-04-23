using System;
using onlinefood.Dto.ContactDtos;
using onlinefood.Services.Interfaces;
using onlinefood.Entity;
using onlinefood.Data;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using onlinefood.ViewModels.ContactVms;

namespace onlinefood.Services;

public class ContactService : IContactService
{
    private readonly FirstRunDbContext dbContext;

    public ContactService(FirstRunDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task CreateContact(CreateContactDto dto)
    {
        using var txn = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var contact = new Contacts();
        contact.Name = dto.Name;
        contact.Email = dto.Email;
        contact.Phone = dto.Phone;
        contact.Subject = dto.Subject;
        contact.Message = dto.Message;
        contact.SubmittedAt = DateTime.UtcNow;

        dbContext.Contacts.Add(contact);
        await dbContext.SaveChangesAsync();
        txn.Complete();
    }

    public async Task<List<ContactVm>> GetAllContacts()
    {
        var contacts = await dbContext.Contacts.Include(x => x.User).ToListAsync();

        var contactDtos = contacts.Select(x => new ContactVm
        {
            ContactId = x.ContactId,
            Name = x.Name,
            Email = x.Email,
            Phone = x.Phone,
            Subject = x.Subject,
            Message = x.Message,
            SubmittedAt = x.SubmittedAt,
        }).ToList();
        return contactDtos;
    }

    public async Task UpdateContactStatus(UpdateContactDto dto)
    {
        using var txn = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var contact = await dbContext.Contacts.FindAsync(dto.ContactId);
        if (contact == null)
        {
            throw new Exception("Contact not found");
        }

        contact.IsResolved = dto.IsResolved;
        await dbContext.SaveChangesAsync();
        txn.Complete();
    }

    public async Task DeleteContact(int id)
    {
        using var txn = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var contact = await dbContext.Contacts.FindAsync(id);
        if (contact == null)
        {
            throw new Exception("Contact not found");
        }

        dbContext.Contacts.Remove(contact);
        await dbContext.SaveChangesAsync();
        txn.Complete();
    }
}
