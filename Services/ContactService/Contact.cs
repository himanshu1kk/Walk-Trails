using Microsoft.EntityFrameworkCore;
using NzWalks.Data;
using NzWalks.Models.Domain;
using NzWalks.Models.DTO;
namespace NzWalks.Services.ContactService;
public class ContactService : IContactService
{
    private readonly NzWalksDbContext _dbContext;

    public ContactService(NzWalksDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Contact> AddAsync(ContactDto contactDto)
    {
        var contact = new Contact
        {
            FullName = contactDto.FullName,
            Email = contactDto.Email,
            Subject = contactDto.Subject,
            Message = contactDto.Message
        };

        // Add to database
        await _dbContext.Contact.AddAsync(contact);
        await _dbContext.SaveChangesAsync();

        return contact;
    }


}