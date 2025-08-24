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



// {
//   "Logging": {
//     "LogLevel": {
//       "Default": "Information",
//       "Microsoft.AspNetCore": "Warning"
//     }
//   },
//   "AllowedHosts": "*",
//   "ConnectionStrings": {
//     "NzWalksConnectionString": "Server=tcp:sqlserver-walks-northindia-dev-0001.database.windows.net,1433;Initial Catalog=sqldb-walks-northindia-dev-0001;Persist Security Info=False;User ID=himanshuwalksdb;Password=Himanshu_1k;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;",
    
//     "NzWalksAuthConnectionString": "Server=localhost;Database=NzWalksAuthDb;Trusted_Connection=True;TrustServerCertificate=True;"
//   },
//   "Jwt": {
//     "Key": "bbuosyJSSGPOosflusJ75JJHst6yjjjST5rt65SY77uhSYSjo098HHhgst",
//     "Issuer": "http://localhost:5190/",
//     "Audience": "http://localhost:5190/"
//   },
//   "AzureBlobStorage": {
//     "ConnectionString": "DefaultEndpointsProtocol=https;AccountName=nzwalksimagesaccount123;AccountKey=+6eBgGIUNU4U52Mp+AqB/YrrB+opd88SazJXcSGjnJRVrY59Vi99578ph/43L4BYqwMitGVQtWhg+AStY2xGDA==;EndpointSuffix=core.windows.net",
//     "ContainerName": "images"
//   }
// }