using System;
using System.Collections.Generic;
using NzWalks.Models.Domain;

namespace NzWalks.Models.DTO
{
    /// <summary>
    /// Complete attraction data transfer object
    /// </summary>
    public class AttractionDto
    {
        public string Id { get; set; }


        public string Name { get; set; }


        public string UserId { get; set; }

        public AttractionType Type { get; set; }


        public string Description { get; set; }


        public double? LengthInKm { get; set; }

        public double? LengthIfTrek{ get; set; }


        public string CoverImageUrl { get; set; }


        public int AdminRating { get; set; }

        public DateTime CreatedAt { get; set; }

        public string NotToMiss { get; set; }
        public string Suggestions { get; set; }


        public LocationDto Location { get; set; }

        public List<ImageDtos> Images { get; set; } = new();

        public List<string> UpvotedByUsers { get; set; } = new();
        
     public string ?StudentTip { get; set; }
    public int? Upvotes { get; set; } = 0;
    public int? Favorites { get; set; } = 0;

    public int? Disliked { get; set; } = 0;
    }

    /// <summary>
    /// Location data for an attraction
    /// </summary>
    public class LocationDto
    {
        public string LocationId { get; set; }
        public string State { get; set; }


        public string City { get; set; }

        public string Region { get; set; }
        
        public string InstituteName { get; set; }
        

    }

    /// <summary>
    /// Image data for an attraction
    /// </summary>
    public class ImageDtos
    {

        public string ImageId { get; set; }
        public string Url { get; set; }
        
        public string Caption { get; set; }
    }

    
}