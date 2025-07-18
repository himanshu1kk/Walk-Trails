namespace NzWalks.Models.Domain{
    public class Review
    {
        public string ReviewId { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string? Reviews { get; set; }

        public Review(){
            this.ReviewId = Guid.NewGuid().ToString();
        }
 

    }
}