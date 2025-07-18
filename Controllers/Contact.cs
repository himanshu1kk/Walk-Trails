using Microsoft.AspNetCore.Mvc;
using NzWalks.Models.DTO;
using NzWalks.Services.ContactService;

namespace NzWalks.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactsController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpPost("contact")]
        public async Task<IActionResult> CreateContact([FromBody] ContactDto contactDto)
        {
            try
            {
                if (contactDto==null)
                {
                    return BadRequest(ModelState);
                }

               if (string.IsNullOrWhiteSpace(contactDto.Email) ||
                string.IsNullOrWhiteSpace(contactDto.FullName) ||
                string.IsNullOrWhiteSpace(contactDto.Message))
            {
                return BadRequest("All fields are required");
            }
                var createdContact = await _contactService.AddAsync(contactDto);
                
                return Ok(new ContactDto
                {
                    FullName = createdContact.FullName,
                    Email = createdContact.Email,
                    Message = createdContact.Message 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error processing your request");
            }
        }
    }
}