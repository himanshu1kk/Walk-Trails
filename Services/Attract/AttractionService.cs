

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
              var user = await _dbContext.Users.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            if(user==null){
                throw new Exception("User Not Found");
            }
            // Create main attraction
            var attraction = new Attraction
            {
                Name = attractionDto.Name,
                UserId = userId,
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

            user.PointsAdded = user.PointsAdded + 100;
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
                    UpvotedByUsers = combined.Attraction.UpvotedByUsers,
                    Upvotes = combined.Attraction.Upvotes,
                    Disliked = combined.Attraction.Disliked,

                    Location = new LocationDto
                    {
                        LocationId = combined.Location.Id,
                        State = combined.Location.State,
                        City = combined.Location.City,
                        Region = combined.Location.Region,
                    },
                    Images = images.Select(i => new ImageDtos
                    {
                        ImageId = i.Id,
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

   public async Task<Attraction> UpdateAttractionAsync(UpdateAttractionDto attractionDto, string userId)
{
    using var transaction = await _dbContext.Database.BeginTransactionAsync();

    try
    {
        var existingAttraction = await _dbContext.Attractions
            .FirstOrDefaultAsync(a => a.Id == attractionDto.AttractionId);

        if (existingAttraction == null || existingAttraction.UserId != userId)
            return null;

        // Basic fields
        if (!string.IsNullOrEmpty(attractionDto.Name))
            existingAttraction.Name = attractionDto.Name;

        if (attractionDto.Type.HasValue)
            existingAttraction.Type = attractionDto.Type.Value;

        if (!string.IsNullOrEmpty(attractionDto.Description))
            existingAttraction.Description = attractionDto.Description;

        if (attractionDto.LengthInKm.HasValue)
            existingAttraction.LengthInKm = attractionDto.LengthInKm.Value;

        if (!string.IsNullOrEmpty(attractionDto.CoverImageUrl))
            existingAttraction.CoverImageUrl = attractionDto.CoverImageUrl;

        if (attractionDto.AdminRating.HasValue)
            existingAttraction.AdminRating = attractionDto.AdminRating.Value;

        if (!string.IsNullOrEmpty(attractionDto.NotToMiss))
            existingAttraction.NotToMiss = attractionDto.NotToMiss;

        if (!string.IsNullOrEmpty(attractionDto.Suggestions))
            existingAttraction.Suggestions = attractionDto.Suggestions;

        if (!string.IsNullOrEmpty(attractionDto.StudentTip))
            existingAttraction.StudentTip = attractionDto.StudentTip;

        if (attractionDto.Upvotes.HasValue)
            existingAttraction.Upvotes = attractionDto.Upvotes.Value;

        if (attractionDto.Favorites.HasValue)
            existingAttraction.Favorites = attractionDto.Favorites.Value;

        if (attractionDto.Disliked.HasValue)
            existingAttraction.Disliked = attractionDto.Disliked.Value;

        if (attractionDto.UpvotedByUsers != null)
            existingAttraction.UpvotedByUsers = attractionDto.UpvotedByUsers;

        _dbContext.Attractions.Update(existingAttraction);

        var existingLocation = await _dbContext.LocationInfos
            .FirstOrDefaultAsync(l => l.AttractionId == attractionDto.AttractionId);

        if (existingLocation != null)
        {
            if (!string.IsNullOrEmpty(attractionDto.State))
                existingLocation.State = attractionDto.State;
            if (!string.IsNullOrEmpty(attractionDto.City))
                existingLocation.City = attractionDto.City;
            if (!string.IsNullOrEmpty(attractionDto.Region))
                existingLocation.Region = attractionDto.Region;

            _dbContext.LocationInfos.Update(existingLocation);
        }

        // 🚩 Update Images (replace all)
      if (attractionDto.Images != null)
        {
            var existingImages = _dbContext.AttractionImages
                .Where(img => img.AttractionId == attractionDto.AttractionId);
            _dbContext.AttractionImages.RemoveRange(existingImages);

            var newImages = attractionDto.Images.Select(img => new AttractionImage
            {
                AttractionId = attractionDto.AttractionId,
                Url = img.Url,
                Caption = img.Caption
            }).ToList();

            await _dbContext.AttractionImages.AddRangeAsync(newImages);
        }
        
        await _dbContext.SaveChangesAsync();
        await transaction.CommitAsync();

        return existingAttraction;
    }
    catch
    {
        await transaction.RollbackAsync();
        throw;
    }
}


    public async Task<Attraction> UpvoteAttractionAsync(string attractionId, string userId)
    {
        var attraction = await _dbContext.Attractions
            .FirstOrDefaultAsync(a => a.Id == attractionId);

        if (attraction == null)
        {
            return null; // Attraction not found
        }

        // Check if user has already upvoted
        bool alreadyUpvoted = attraction.UpvotedByUsers?.Contains(userId) ?? false;

        if (alreadyUpvoted)
        {
            // Remove upvote
            attraction.UpvotedByUsers?.Remove(userId);
            attraction.Upvotes--;
        }
        else
        {
            // Add upvote
            attraction.UpvotedByUsers ??= new List<string>();
            attraction.UpvotedByUsers.Add(userId);
            attraction.Upvotes++;
        }

        // Convert the attraction to DTO for update
        var attractionDto = MapToUpdateAttractionDto(attraction);

        // Update the attraction
        // await UpdateAttractionAsync(attractionDto);

        return attraction; // Return the updated attraction directly
    }

    private UpdateAttractionDto MapToUpdateAttractionDto(Attraction attraction)
    {
        return new UpdateAttractionDto
        {
            AttractionId = attraction.Id,
            Name = attraction.Name,
            Type = attraction.Type,
            Description = attraction.Description,
            LengthInKm = attraction.LengthInKm,
            CoverImageUrl = attraction.CoverImageUrl,
            AdminRating = attraction.AdminRating,
            NotToMiss = attraction.NotToMiss,
            Suggestions = attraction.Suggestions,
            StudentTip = attraction.StudentTip,
            Upvotes = attraction.Upvotes,
            Favorites = attraction.Favorites,
            Disliked = attraction.Disliked,
            UpvotedByUsers = attraction.UpvotedByUsers
        };
    }

    // Add or Update Student Tip for an attraction
    public async Task<Attraction> AddOrUpdateStudentTipAsync(string attractionId, string studentTip)
    {
        var attraction = await _dbContext.Attractions
            .FirstOrDefaultAsync(a => a.Id == attractionId);

        if (attraction == null)
        {
            return null; // Attraction not found
        }

        // Update the StudentTip field
        attraction.StudentTip = studentTip;

        // Convert the attraction to DTO and update
        var attractionDto = MapToUpdateAttractionDto(attraction);
        // await UpdateAttractionAsync(attractionDto);

        return attraction; // Return the updated attraction
    }

    // Add or Update Favorites for an attraction
    public async Task<Attraction> AddOrUpdateFavoritesAsync(string attractionId, int favorites)
    {
        var attraction = await _dbContext.Attractions
            .FirstOrDefaultAsync(a => a.Id == attractionId);

        if (attraction == null)
        {
            return null; // Attraction not found
        }

        // Update the Favorites field
        attraction.Favorites = favorites;

        // Convert the attraction to DTO and update
        var attractionDto = MapToUpdateAttractionDto(attraction);
        // await UpdateAttractionAsync(attractionDto);

        return attraction; // Return the updated attraction
    }

    public async Task<Location> UpdateAttractionLocationAsync(UpdateLocationDto locationDto)
{
    try
    {
        // Fetch the attraction along with its associated location
        var location = await _dbContext.LocationInfos
            .FirstOrDefaultAsync(a => a.Id == locationDto.LocationId);

        if (location == null)
        {
            // Attraction not found, return null
            return null; 
        }

            // Update only the fields that are not null in the DTO
            if (!string.IsNullOrEmpty(locationDto.State))
                location.State = locationDto.State;

            if (!string.IsNullOrEmpty(locationDto.City))
                location.City = locationDto.City;

            if (!string.IsNullOrEmpty(locationDto.Region))
                location.Region = locationDto.Region;

            // Save changes to the database
            _dbContext.LocationInfos.Update(location);
            await _dbContext.SaveChangesAsync();
        


        // Return the updated attraction
        return location;
    }
    catch (DbUpdateException dbEx)
    {
        // Handle database-specific exceptions (e.g., constraint violations)
        // Log the exception (you can use a logger here)
        throw new Exception("An error occurred while updating the attraction's location.", dbEx);
    }
    catch (Exception ex)
    {
        // Handle general exceptions
        // Log the exception (you can use a logger here)
        throw new Exception("An unexpected error occurred while updating the attraction's location.", ex);
    }
}

public async Task<List<AttractionDto>> GetAttractionsByUserIdAsync(string userId)
{
    try
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("UserId cannot be null or empty.");

        var attractions = await _dbContext.Attractions
            .Where(a => a.UserId == userId)
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

        // if (attractions == null || attractions.Count == 0)
        //     throw new Exception("No attractions found for the given user.");

        return attractions;
    }
    catch (ArgumentException argEx)
    {
        // Handle known validation error
        throw new Exception($"Invalid input: {argEx.Message}", argEx);
    }
    catch (Exception ex)
    {
        // Log exception here if needed
        throw new Exception("Failed to retrieve attractions for the user.", ex);
    }
}

}