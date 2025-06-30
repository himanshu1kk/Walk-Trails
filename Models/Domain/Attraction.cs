using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NzWalks.Models.Domain;

public class Attraction
{
    [Key]
    public string Id { get; set; }

   
    public string Name { get; set; }

   
    public string UserId { get; set; }

    public AttractionType Type { get; set; }

    public string Description { get; set; }

    public double? LengthInKm { get; set; }

    public string CoverImageUrl { get; set; }

    public int AdminRating { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string NotToMiss { get; set; }

    public string Suggestions { get; set; }

    public string ?StudentTip { get; set; }
    public int? Upvotes { get; set; } = 0;
    public int? Favorites { get; set; } = 0;

    public int? Disliked { get; set; } = 0;


    public Attraction()
    {
        this.Id = Guid.NewGuid().ToString();
    }
}

public enum AttractionType
{
    Cafe = 100,
    SeaShore = 200,
    Temple = 300,
    Trek = 400,
    Park = 500,
    ChaiTapri = 600,
    HostelMess = 700,
    Dhaba = 800,
    Library = 900,
    SportsGround = 1000,
    Other = 1100
}