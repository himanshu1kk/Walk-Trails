public class UpdateLocationDto
{
    public string LocationId { get; set; } // The ID of the location to update
    public string? State { get; set; }      // State (nullable)
    public string? City { get; set; }       // City (nullable)
    public string? Region { get; set; }     // Region (nullable)
}
