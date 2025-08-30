using NzWalks.Models.DTO;
namespace NzWalks.Models.Domain;

public class AttractionReport
{
    public string Id { get; set; }
    public string AttractionId { get; set; }
    public string UserId { get; set; }
    public Reason Reason { get; set; }
    public DateTime ReportedAt { get; set; }

    public AttractionReport()
    {
        this.Id = Guid.NewGuid().ToString();
    }
}
