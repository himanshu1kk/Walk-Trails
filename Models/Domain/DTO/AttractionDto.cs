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
        

        public string CoverImageUrl { get; set; }
        
    
        public int AdminRating { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public string NotToMiss { get; set; } 
        public string Suggestions { get; set; }
        
        
        public LocationDto Location { get; set; }
        
        public List<ImageDtos> Images { get; set; } = new();
    }

    /// <summary>
    /// Location data for an attraction
    /// </summary>
    public class LocationDto
    {
    
        public string State { get; set; }
        
        
        public string City { get; set; }
        
        public string Region { get; set; }
        
      
        public double Latitude { get; set; }
        
       
        public double Longitude { get; set; }
    }

    /// <summary>
    /// Image data for an attraction
    /// </summary>
    public class ImageDtos
    {
        public string Url { get; set; }
        
        public string Caption { get; set; }
    }

    
}