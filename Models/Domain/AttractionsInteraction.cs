using NzWalks.Models.Domain;

public class AttractionInteractionDto
{
    public string Name { get; set; }
    public AttractionType Type { get; set; }
    public string Description { get; set; }
    public double? LengthInKm { get; set; }
    public double? LengthIfTrek{ get; set; }
    public string CoverImageUrl { get; set; }
    public int AdminRating { get; set; }
    public string NotToMiss { get; set; }
    public string Suggestions { get; set; }
    
    // Location data
    public string State { get; set; }
    public string City { get; set; }
    public string Region { get; set; }
    public string InstituteName{ get; set; }
    // Images data
    public List<ImageDto> Images { get; set; }
}

public class ImageDto
{
    public string Url { get; set; }
    public string Caption { get; set; }
}