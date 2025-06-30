using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NzWalks.Models.Domain;

public class AttractionImage
{
    [Key]
    public string Id { get; set; }


    public string Url { get; set; }

  
    public string Caption { get; set; }

  
    public string AttractionId { get; set; }

    public AttractionImage()
    {
        this.Id = Guid.NewGuid().ToString();
    }
}