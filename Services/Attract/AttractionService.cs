

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
            if (user == null)
            {
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
                Suggestions = attractionDto.Suggestions,
                LengthIfTrek = attractionDto.LengthIfTrek,
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
                InstituteName = attractionDto.InstituteName,

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

    public async Task<PaginatedDto> GetAllAttractionsAsync(int pageNumber, int pageSize)
    {
        var attractions = await _dbContext.Attractions
            .Where(a => a.IsActive)
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
                    LengthIfTrek = combined.Attraction.LengthIfTrek,
                    Location = new LocationDto
                    {
                        State = combined.Location.State,
                        City = combined.Location.City,
                        Region = combined.Location.Region,
                        InstituteName = combined.Location.InstituteName

                    },
                    Images = images.Select(i => new ImageDtos
                    {
                        Url = i.Url,
                        Caption = i.Caption
                    }).ToList()
                }
            )
            .AsNoTracking()
             .Skip((pageNumber - 1) * pageSize)
             .Take(pageSize)
            .ToListAsync();
        var totalCount = await _dbContext.Attractions.CountAsync();

        return new PaginatedDto
        {
            Count = totalCount,
            data = attractions,
        };
    }

    // Get base query with joins
    public async Task<PaginatedDto> SearchAttractionsAsync(string? searchTerm, string? searchBy, int pageNumber, int pageSize)
    {
        // Normalize search term
        searchTerm = searchTerm?.Trim();
        bool hasSearch = !string.IsNullOrEmpty(searchTerm);

        var query = from a in _dbContext.Attractions
                    where a.IsActive // Filter only active attractions
                    join l in _dbContext.LocationInfos on a.Id equals l.AttractionId
                    select new { Attraction = a, Location = l };

        // Apply filtering (only if search term provided)
        if (hasSearch)
        {
            switch (searchBy?.ToLower())
            {
                case "name":
                    query = query.Where(x => x.Attraction.Name.Contains(searchTerm));
                    break;

                case "state":
                    query = query.Where(x => x.Location.State.Contains(searchTerm));
                    break;

                case "city":
                    query = query.Where(x => x.Location.City.Contains(searchTerm));
                    break;

                case "region":
                    query = query.Where(x => x.Location.Region.Contains(searchTerm));
                    break;

                case "location": // all location fields
                    query = query.Where(x =>
                        x.Location.State.Contains(searchTerm) ||
                        x.Location.City.Contains(searchTerm) ||
                        x.Location.Region.Contains(searchTerm));
                    break;

                default: // All fields
                    query = query.Where(x =>
                        x.Attraction.Name.Contains(searchTerm) ||
                        x.Location.State.Contains(searchTerm) ||
                        x.Location.City.Contains(searchTerm) ||
                        x.Location.Region.Contains(searchTerm));
                    break;
            }
        }

        int totalCount = await query.CountAsync();

        // Apply pagination (Skip and Take)
        var pagedQuery = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        // Fetch paged result
        var result = await pagedQuery
            .Select(x => new AttractionDto
            {
                Id = x.Attraction.Id,
                Name = x.Attraction.Name,
                UserId = x.Attraction.UserId,
                Type = x.Attraction.Type,
                Description = x.Attraction.Description,
                LengthInKm = x.Attraction.LengthInKm,
                CoverImageUrl = x.Attraction.CoverImageUrl,
                AdminRating = x.Attraction.AdminRating,
                CreatedAt = x.Attraction.CreatedAt,
                NotToMiss = x.Attraction.NotToMiss,
                Suggestions = x.Attraction.Suggestions,
                LengthIfTrek = x.Attraction.LengthIfTrek,

                Location = new LocationDto
                {
                    State = x.Location.State,
                    City = x.Location.City,
                    Region = x.Location.Region,
                    InstituteName = x.Location.InstituteName
                },

                Images = _dbContext.AttractionImages
                    .Where(img => img.AttractionId == x.Attraction.Id)
                    .Select(img => new ImageDtos
                    {
                        Url = img.Url,
                        Caption = img.Caption
                    }).ToList()
            })
            .AsNoTracking()
            .ToListAsync();

        return new PaginatedDto
        {
            Count = totalCount,
            data = result,

        };


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
                var newImages = attractionDto.Images
                    .Where(img => !string.IsNullOrEmpty(img.Url))  // Filter out null/empty URLs
                    .Select(img => new AttractionImage
                    {
                        AttractionId = attractionDto.AttractionId,
                        Url = img.Url,
                        Caption = img.Caption
                    })
                    .ToList();
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


        _dbContext.Attractions.Update(attraction);
        await _dbContext.SaveChangesAsync();
        return attraction;


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
            Description = attraction.Description,
            Type = attraction.Type,
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
                   .Where(a => a.IsActive)
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
                        LengthIfTrek = combined.Attraction.LengthIfTrek,
                        Location = new LocationDto
                        {
                            State = combined.Location.State,
                            City = combined.Location.City,
                            Region = combined.Location.Region,
                            InstituteName = combined.Location.InstituteName,
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
    public async Task<ReportResponse> ReportAttractionAsync(ReportAttractionDto reportDto, string userId)
    {
        var attraction = await _dbContext.Attractions
            .FirstOrDefaultAsync(a => a.Id == reportDto.AttractionId);

        if (attraction == null)
        {
            return new ReportResponse { Success = false, Message = "Attraction not found." };
        }

        if (attraction.UserId == userId)
        {
            return new ReportResponse { Success = false, Message = "User cannot report their own attraction." };
        }

        var alreadyReported = await _dbContext.AttractionReports
            .AnyAsync(r => r.AttractionId == reportDto.AttractionId && r.UserId == userId);

        if (alreadyReported)
        {
            return new ReportResponse { Success = false, Message = "You have already reported this attraction." };
        }

        // Create and add report
        var report = new AttractionReport
        {
            AttractionId = reportDto.AttractionId,
            UserId = userId,
            Reason = reportDto.Reason,
            ReportedAt = DateTime.UtcNow
        };

        await _dbContext.AttractionReports.AddAsync(report);

        // Update attraction report count
        attraction.ReportCount++;

        // Count same-reason reports
        var similarReportCount = await _dbContext.AttractionReports
            .CountAsync(r => r.AttractionId == reportDto.AttractionId && r.Reason == reportDto.Reason);

        if (similarReportCount >= 10)
        {
            attraction.IsActive = false;
        }

        _dbContext.Attractions.Update(attraction);

        await _dbContext.SaveChangesAsync();

        return new ReportResponse { Success = true, Message = "Report submitted successfully." };
    }

    public async Task<ReportResponse> UnreportAttractionAsync(string attractionId, string userId)
    {
        var attraction = await _dbContext.Attractions
            .FirstOrDefaultAsync(a => a.Id == attractionId);

        if (attraction == null)
            return new ReportResponse { Success = false, Message = "Attraction not found." };

        var report = await _dbContext.AttractionReports
            .FirstOrDefaultAsync(r => r.AttractionId == attractionId && r.UserId == userId);

        if (report == null)
            return new ReportResponse { Success = false, Message = "Report not found for this user." };

        // Delete report
        _dbContext.AttractionReports.Remove(report);
        attraction.ReportCount--;

        // If reports for that reason drop below 10, re-enable attraction
        var remainingSameReason = await _dbContext.AttractionReports
            .CountAsync(r => r.AttractionId == attractionId && r.Reason == report.Reason);

        if (remainingSameReason < 10 && !attraction.IsActive)
            attraction.IsActive = true;

        await _dbContext.SaveChangesAsync();

        return new ReportResponse { Success = true, Message = "Unreported successfully." };
    }

public async Task<List<AttractionDto>> GetReportedAttractionsByUserAsync(string userId)
{
    var reportedAttractionIds = await _dbContext.AttractionReports
        .Where(r => r.UserId == userId)
        .Select(r => r.AttractionId)
        .Distinct()
        .ToListAsync();

    if (!reportedAttractionIds.Any())
        return new List<AttractionDto>();

    var attractions = await _dbContext.Attractions
           .Where(a => reportedAttractionIds.Contains(a.Id) && a.IsActive)
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
                    State = combined.Location.State,
                    City = combined.Location.City,
                    Region = combined.Location.Region
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






}