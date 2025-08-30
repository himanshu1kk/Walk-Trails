using NzWalks.Models.Domain;

namespace NzWalks.Models.DTO
{
    public class ContactDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public Subject Subject { get; set; } 
        public string Message { get; set; }
    }
}