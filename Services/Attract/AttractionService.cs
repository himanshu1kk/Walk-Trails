// using Microsoft.EntityFrameworkCore;
// using NzWalks.Data;
// using NzWalks.Models.Domain;

// namespace NzWalks.Service.Attract;

// public class AttractionService(
//     NzWalksDbContext nzWalksDbContext
// ) : IAttractionService
// {
//     private readonly NzWalksDbContext _nzWalksDbContext = nzWalksDbContext;

//     public async Task CreateWalkAsync(AttractionsModel attraction)
//     {

//         try
//         {

//             //now we need to upsert this into the db only
//             if (attraction == null)
//             {
//                 throw new Exception("cannot create a null inteaction u need to populate some fields of it.");
//             }

//             if (string.IsNullOrWhiteSpace(attraction.AttractionName))
//                 throw new ArgumentException("Attraction name is required");

//             if (string.IsNullOrWhiteSpace(attraction.Description))
//                 throw new ArgumentException("Description is required");

//             if (attraction.LocationInfo == null)
//                 throw new ArgumentException("Location info is required");

//             // Validate LocationInfo
//             ValidateLocation(attraction.LocationInfo);

//             // Validate LengthInKm (if provided)
//             if (attraction.LengthInKm.HasValue && attraction.LengthInKm < 0)
//                 throw new ArgumentException("Length must be positive");

//             // Validate RatingsByAdmin
//             if (attraction.RatingsByAdmin < 0 || attraction.RatingsByAdmin > 5)
//                 throw new ArgumentException("Admin rating must be between 0 and 5");


//             await nzWalksDbContext.Attractions.AddAsync(attraction);
//             Console.WriteLine("helloq");

//             var user = await nzWalksDbContext.Users.FirstOrDefaultAsync(x => x.UserId == attraction.UserId);
//             user.PointsAdded = user.PointsAdded + 100;

//             nzWalksDbContext.Users.Update(user);

//             await nzWalksDbContext.SaveChangesAsync();




//         }
//         catch (Exception ex)
//         {
//             throw new Exception("An error occurred while creating the attraction.", ex);
//         }
//     }

//     private static void ValidateLocation(LocationInfo location)
//     {
//         if (location == null)
//             throw new ArgumentNullException(nameof(location), "Location cannot be null");

//         // Validate coordinates (if provided)
//         if (location.Latitude.HasValue && (location.Latitude < -90 || location.Latitude > 90))
//             throw new ArgumentException("Latitude must be between -90 and 90");

//         if (location.Longitude.HasValue && (location.Longitude < -180 || location.Longitude > 180))
//             throw new ArgumentException("Longitude must be between -180 and 180");
//     }
    

//    public async Task<List<AttractionsModel>> GetAllWalks()
//     {
//         try
//         {
//             var attractions = await nzWalksDbContext.Attractions.ToListAsync();
//             return attractions;
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine($"An error occurred while retrieving walks: {ex.Message}");
//             throw new Exception("An error occurred while retrieving walks.", ex);
//         }
//     }

//     public async Task<AttractionsModel> GetWalkByIdAsync(string Id)
//     {



//         try
//         {
//             var attraction = await nzWalksDbContext.Attractions.FirstOrDefaultAsync(x => x.AttractionId == Id);
//             if (attraction == null)
//             {
//                 Console.WriteLine("no attraction found with this id ");
//                 throw new Exception("attraction does not exist with this id");
//             }

//             return attraction;
//         }

//         catch (Exception ex)
//         {
//             // Log the exception (you can replace this with a logger in a real application)
//             Console.WriteLine($"An error occurred: {ex.Message}");

//             // You can rethrow the exception if needed, or return a custom error response
//             throw new Exception("An error occurred while fetching the attraction.", ex);
//         }

//     }
// }

using Microsoft.EntityFrameworkCore;
using NzWalks.Data;
using NzWalks.Models.Domain;
using NzWalks.Models.DTO;
using NzWalks.Service.Attract;

namespace NzWalks.Services.Attractions;

public class AttractionService : IAttractionService
{
    private readonly NzWalksDbContext _dbContext;

    public AttractionService(NzWalksDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Attraction> CreateAttractionAsync(AttractionInteractionDto attractionDto, string userId)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            // Create main attraction
            var attraction = new Attraction
            {
                Name = attractionDto.Name,
                UserId = "456",
                Type = attractionDto.Type,
                Description = attractionDto.Description,
                LengthInKm = attractionDto.LengthInKm,
                CoverImageUrl = attractionDto.CoverImageUrl,
                AdminRating = attractionDto.AdminRating,
                NotToMiss = attractionDto.NotToMiss,
                Suggestions = attractionDto.Suggestions
            };

            await _dbContext.Attractions.AddAsync(attraction);
            await _dbContext.SaveChangesAsync();

            // Create location
            var location = new Location
            {
                AttractionId = attraction.Id,
                State = attractionDto.State,
                City = attractionDto.City,
                Region = attractionDto.Region,
                Latitude = attractionDto.Latitude,
                Longitude = attractionDto.Longitude
            };

            await _dbContext.LocationInfos.AddAsync(location);

            // Create images
            if (attractionDto.Images != null && attractionDto.Images.Any())
            {
                var images = attractionDto.Images.Select(img => new AttractionImage
                {
                    AttractionId = attraction.Id,
                    Url = img.Url,
                    Caption = img.Caption
                }).ToList();

                await _dbContext.AttractionImages.AddRangeAsync(images);
            }

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return attraction;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    public async Task<AttractionDto> GetAttractionByIdAsync(string id)
    {
        var result = await _dbContext.Attractions
            .Where(a => a.Id == id)
            .Join(
                _dbContext.LocationInfos,
                a => a.Id,
                l => l.AttractionId,
                (a, l) => new { Attraction = a, Location = l }
            )
            .GroupJoin(
                _dbContext.AttractionImages,
                combined => combined.Attraction.Id,
                img => img.AttractionId,
                (combined, images) => new AttractionDto
                {
                    Id = combined.Attraction.Id,
                    Name = combined.Attraction.Name,
                    UserId = combined.Attraction.UserId,
                    Type = combined.Attraction.Type,
                    Description = combined.Attraction.Description,
                    LengthInKm = combined.Attraction.LengthInKm,
                    CoverImageUrl = combined.Attraction.CoverImageUrl,
                    AdminRating = combined.Attraction.AdminRating,
                    CreatedAt = combined.Attraction.CreatedAt,
                    NotToMiss = combined.Attraction.NotToMiss,
                    Suggestions = combined.Attraction.Suggestions,
                    Location = new LocationDto
                    {
                        State = combined.Location.State,
                        City = combined.Location.City,
                        Region = combined.Location.Region,
                        Latitude = combined.Location.Latitude ?? 0,
                        Longitude = combined.Location.Longitude ?? 0
                    },
                    Images = images.Select(i => new ImageDtos
                    {
                        Url = i.Url,
                        Caption = i.Caption
                    }).ToList()
                }
            )
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return result;
    }

    public async Task<List<AttractionDto>> GetAllAttractionsAsync()
    {
        var attractions = await _dbContext.Attractions
            .Join(
                _dbContext.LocationInfos,
                a => a.Id,
                l => l.AttractionId,
                (a, l) => new { Attraction = a, Location = l }
            )
            .GroupJoin(
                _dbContext.AttractionImages,
                combined => combined.Attraction.Id,
                img => img.AttractionId,
                (combined, images) => new AttractionDto
                {
                    Id = combined.Attraction.Id,
                    Name = combined.Attraction.Name,
                    UserId = combined.Attraction.UserId,
                    Type = combined.Attraction.Type,
                    Description = combined.Attraction.Description,
                    LengthInKm = combined.Attraction.LengthInKm,
                    CoverImageUrl = combined.Attraction.CoverImageUrl,
                    AdminRating = combined.Attraction.AdminRating,
                    CreatedAt = combined.Attraction.CreatedAt,
                    NotToMiss = combined.Attraction.NotToMiss,
                    Suggestions = combined.Attraction.Suggestions,
                    Location = new LocationDto
                    {
                        State = combined.Location.State,
                        City = combined.Location.City,
                        Region = combined.Location.Region,
                        Latitude = combined.Location.Latitude ?? 0,
                        Longitude = combined.Location.Longitude ?? 0
                    },
                    Images = images.Select(i => new ImageDtos
                    {
                        Url = i.Url,
                        Caption = i.Caption
                    }).ToList()
                }
            )
            .AsNoTracking()
            .ToListAsync();

        return attractions;
    }
public async Task<List<AttractionDto>> SearchAttractionsAsync(string? searchTerm, string? searchBy)
{
    // Get base query with joins
    var query = _dbContext.Attractions
        .Join(
            _dbContext.LocationInfos,
            a => a.Id,
            l => l.AttractionId,
            (a, l) => new { Attraction = a, Location = l }
        )
        .GroupJoin(
            _dbContext.AttractionImages,
            combined => combined.Attraction.Id,
            img => img.AttractionId,
            (combined, images) => new AttractionDto
            {
                Id = combined.Attraction.Id,
                Name = combined.Attraction.Name,
                UserId = combined.Attraction.UserId,
                Type = combined.Attraction.Type,
                Description = combined.Attraction.Description,
                LengthInKm = combined.Attraction.LengthInKm,
                CoverImageUrl = combined.Attraction.CoverImageUrl,
                AdminRating = combined.Attraction.AdminRating,
                CreatedAt = combined.Attraction.CreatedAt,
                NotToMiss = combined.Attraction.NotToMiss,
                Suggestions = combined.Attraction.Suggestions,
                Location = new LocationDto
                {
                    State = combined.Location.State,
                    City = combined.Location.City,
                    Region = combined.Location.Region,
                    Latitude = combined.Location.Latitude ?? 0,
                    Longitude = combined.Location.Longitude ?? 0
                },
                Images = images.Select(i => new ImageDtos
                {
                    Url = i.Url,
                    Caption = i.Caption
                }).ToList()
            }
        );

    // Apply filters based on search parameters
    if (!string.IsNullOrEmpty(searchTerm))
    {
        query = searchBy?.ToLower() switch
        {
            "name" => query.Where(a => a.Name.Contains(searchTerm)),
            // "type" => query.Where(a => a.Type.ToString().Equals(searchTerm)),
            "location" => query.Where(a => 
                a.Location.State.Contains(searchTerm) ||
                a.Location.City.Contains(searchTerm) ||
                a.Location.Region.Contains(searchTerm)),
            _ => query.Where(a => 
                a.Name.Contains(searchTerm) ||
                a.Location.State.Contains(searchTerm) ||
                a.Location.City.Contains(searchTerm) ||
                a.Location.Region.Contains(searchTerm))
        };
    }

    return await query.AsNoTracking().ToListAsync();
}
}
