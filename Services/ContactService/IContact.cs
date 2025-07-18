using NzWalks.Models.Domain;
using NzWalks.Models.DTO;

namespace NzWalks.Services.ContactService
{
    public interface IContactService
    {
        Task<Contact> AddAsync(ContactDto contactDto);
    }
}