namespace NzWalks.Models.DTO;

public class ReportAttractionDto
{
    public string AttractionId { get; set; }
    public Reason Reason { get; set; }
}
public enum Reason
{
    UNKNOWN = 0,
    MISINFOMATION = 1,
    DONOTEXISTNOW = 2,
    IMPROPERLANGUAGE = 3,
    WRONGLOCATION = 4,
    



}
