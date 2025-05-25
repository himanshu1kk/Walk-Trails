using Microsoft.Net.Http.Headers;

namespace NzWalks.Models.Dto{
    public class WalkDto
    {
        public Guid Id { get; set; } //similarly we donot want someone to add the id here will remove it from her e
        public string Name { get; set; }

        public string Description { get; set; }
        public double LengthInKm { get; set; }

        public string? WalkImageUrl { get; set; }

        public string DifficultyId { get; set; }
        public string RegionId { get; set; }


        public RegionDto Region { get; set; }
        public DifficultyDto Difficulty { get; set; }
        
        public double? EstimatedDurationHours { get; set; }


    }
}
