namespace NzWalks.Models.Domain
{
    public class Contact
    {
        public string ContactId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public Subject Subject { get; set; }
        public string Message { get; set; }

        public Contact()
        {
            this.ContactId = Guid.NewGuid().ToString();
        }
    }

    public enum Subject
    {
        GENERAL_INQUIRY = 1,
        PROJECT_COLLABORATION = 2,
        INTERNSHIP_OPPORTUNITY = 3,
        FEEDBACK = 4
    }
}