using NzWalks.Models.Domain;


public class UpdateAttractionDto
{
    public string AttractionId { get; set; }
    public string Name { get; set; }
    public AttractionType? Type { get; set; }
    public string Description { get; set; }
    public double? LengthInKm { get; set; }
    public string CoverImageUrl { get; set; }
    public int? AdminRating { get; set; }
    public string NotToMiss { get; set; }
    public string Suggestions { get; set; }

    public string? StudentTip { get; set; }
    public int? Upvotes { get; set; } = 0;
    public int? Favorites { get; set; } = 0;
    public int? Disliked { get; set; } = 0;
    public List<string>? UpvotedByUsers { get; set; } = new();

    // Location
    public string? State { get; set; }
    public string? City { get; set; }
    public string? Region { get; set; }

    // Images with captions
    public List<ImageDto>? Images { get; set; } = new();
}



