using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NzWalks.Models.Domain;

public class Location
{
    [Key]
    public string Id { get; set; }

 
    public string State { get; set; }

  
    public string City { get; set; }

   
    public string Region { get; set; }

  
    public string AttractionId { get; set; }

    public string InstituteName{ get; set; }

    public Location()
    {
        this.Id = Guid.NewGuid().ToString();
    }
}