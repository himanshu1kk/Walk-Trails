using System.Linq.Expressions;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NzWalks.Data;
using NzWalks.Extensions;
using NzWalks.Models.Domain;
using NzWalks.Models.DTO;
using NzWalks.Service.Attract;
using NzWalks.Services.Registration;

namespace NzWalks.Controllers;

[ApiController]
[Route("api/v1")]
public class AttractionController(
    IAttractionService attractionService,
    NzWalksDbContext nzWalksDbContext
) : ControllerBase
{
    private readonly IAttractionService _attractionService = attractionService;
    private readonly NzWalksDbContext _nzWalksDbContext = nzWalksDbContext;

    [Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN,USER")]
    [HttpPost("create-attraction")]
    public async Task<IActionResult> CreateAttraction([FromBody] AttractionInteractionDto attractionDto)
    {
        try
        {
            var userId = User.FindFirstValue("UserId");

            if (attractionDto == null)
            {
                return BadRequest("Attraction data cannot be null");
            }

            var addAddtraction = await _attractionService.CreateAttractionAsync(attractionDto, userId);
            return Ok(new
            {
                message = "attracted added successfully",
                attractionId = addAddtraction.Id
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("get-attraction")]
    public async Task<IActionResult> GetAllAttractions(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 12)
    {
        var attractions = await _attractionService.GetAllAttractionsAsync(pageNumber, pageSize);
        return Ok(attractions);
    }

    [Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN, USER")]
    [HttpGet("get-attraction-by-id")]
    public async Task<IActionResult> GetAttraction([FromQuery] string id)
    {
        try
        {
            var userId = User.FindFirstValue("UserId");
            if (id == null)
            {
                throw new Exception("attraction id cannot be null");
            }
            var attraction = await _attractionService.GetAttractionByIdAsync(id);
            return Ok(attraction);
        }
        catch (Exception ex)
        {
            throw new Exception("their is some error in getting the attraction", ex);
        }
    }

    [HttpGet("search-attraction")]
    public async Task<IActionResult> SearchAttractions(
        [FromQuery] string? searchTerm,
        [FromQuery] string? searchBy = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 12
        )
    {
        try
        {
            var attractions = await _attractionService.SearchAttractionsAsync(searchTerm, searchBy, pageNumber, pageSize);
            return Ok(attractions);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN,USER")]
    [HttpPut("update-attraction")]
    public async Task<IActionResult> UpdateAttraction([FromBody] UpdateAttractionDto attractionDto)
    {
        try
        {
            var userId = User.FindFirstValue("UserId");

            if (attractionDto == null)
            {
                return BadRequest("Attraction data cannot be null");
            }

            var updatedAttraction = await _attractionService.UpdateAttractionAsync(attractionDto, userId);

            if (updatedAttraction == null)
            {
                return NotFound($"Attraction with ID {attractionDto.AttractionId} not found.");
            }

            return Ok(new
            {
                message = "Attraction updated successfully",
                attractionId = updatedAttraction.Id
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN,USER")]
    [HttpPost("upvote-attraction")]
    public async Task<IActionResult> UpvoteAttraction([FromQuery] string attractionId)
    {
        try
        {
            var userId = User.FindFirstValue("UserId");

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token");

            if (string.IsNullOrEmpty(attractionId))
                return BadRequest("Attraction ID is required");

            var attraction = await _attractionService.UpvoteAttractionAsync(attractionId, userId);

            if (attraction == null)
                return NotFound("Attraction not found");

            return Ok(new
            {
                Message = attraction.Upvotes > 0 ? "Upvote added successfully" : "Upvote removed successfully",
                Upvotes = attraction.Upvotes,
                HasUpvoted = attraction.UpvotedByUsers.Contains(userId)
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("add-student-tip")]
    public async Task<IActionResult> AddStudentTip([FromQuery] string attractionId, [FromBody] string studentTip)
    {
        try
        {
            if (string.IsNullOrEmpty(attractionId))
                return BadRequest("Attraction ID is required");

            if (string.IsNullOrEmpty(studentTip))
                return BadRequest("Student Tip is required");

            var updatedAttraction = await _attractionService.AddOrUpdateStudentTipAsync(attractionId, studentTip);

            if (updatedAttraction == null)
                return NotFound("Attraction not found");

            return Ok(new
            {
                Message = "Student Tip added/updated successfully",
                StudentTip = updatedAttraction.StudentTip
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("add-favorites")]
    public async Task<IActionResult> AddFavorites([FromQuery] string attractionId, [FromBody] int favorites)
    {
        try
        {
            if (string.IsNullOrEmpty(attractionId))
                return BadRequest("Attraction ID is required");

            if (favorites < 0)
                return BadRequest("Favorites cannot be negative");

            var updatedAttraction = await _attractionService.AddOrUpdateFavoritesAsync(attractionId, favorites);

            if (updatedAttraction == null)
                return NotFound("Attraction not found");

            return Ok(new
            {
                Message = "Favorites count added/updated successfully",
                Favorites = updatedAttraction.Favorites
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("update-location")]
    public async Task<IActionResult> UpdateAttractionLocation([FromBody] UpdateLocationDto locationDto)
    {
        try
        {
            if (string.IsNullOrEmpty(locationDto.LocationId))
                return BadRequest("Location ID is required.");

            var location = await _attractionService.UpdateAttractionLocationAsync(locationDto);

            if (location == null)
                return NotFound("Attraction not found.");

            return Ok(new
            {
                Message = " location updated successfully",
                Location = location
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN,USER")]
    [HttpGet("my-attractions")]
    public async Task<IActionResult> GetMyAttractions()
    {
        try
        {
            var userId = User.FindFirstValue("UserId");

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found or unauthorized.");

            var myAttractions = await _attractionService.GetAttractionsByUserIdAsync(userId);

            return Ok(new
            {
                Message = "Attractions fetched successfully.",
                Count = myAttractions.Count,
                Data = myAttractions
            });
        }
        catch (ArgumentException argEx)
        {
            return BadRequest(new
            {
                Message = "Bad request.",
                Details = argEx.Message
            });
        }
        catch (Exception ex)
        {
            return NotFound(new
            {
                Message = ex.InnerException?.Message ?? ex.Message
            });
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN,USER")]
    [HttpPost("add-review")]
    public async Task<IActionResult> AddReview([FromBody] ReviewDto reviewDto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(reviewDto.Reviews))
            {
                return BadRequest("Review content is required.");
            }

            var firstName = User.FindFirstValue(ClaimTypes.GivenName) ?? "Anonymous";
            var lastName = User.FindFirstValue(ClaimTypes.Surname) ?? string.Empty;
            var email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;

            var review = new Review
            {
                FirstName = firstName,
                LastName = lastName,
                Reviews = reviewDto.Reviews
            };

            await _nzWalksDbContext.Reviews.AddAsync(review);
            await _nzWalksDbContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "Review submitted successfully.",
                ReviewId = review.ReviewId
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Error while adding review", Details = ex.Message });
        }
    }

    [HttpGet("get-user-by-id")]
    public async Task<IActionResult> GetUserById([FromQuery] string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("User ID is required.");

            var user = await _nzWalksDbContext.Users.FindAsync(id);

            if (user == null)
                return NotFound("User not found.");

            var userDto = new UserDto
            {
                Id = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };

            return Ok(userDto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN,USER")]
    [HttpPost("report-attraction")]
    public async Task<IActionResult> ReportAttraction([FromBody] ReportAttractionDto reportDto)
    {
        try
        {
            var userId = User.FindFirstValue("UserId");

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found.");

            if (reportDto == null || string.IsNullOrEmpty(reportDto.AttractionId) || !Enum.IsDefined(typeof(Reason), reportDto.Reason))
                return BadRequest("Invalid report details.");

            var result = await _attractionService.ReportAttractionAsync(reportDto, userId);
            if (!result.Success)
                return Conflict(new { Message = result.Message });

            return Ok(new { Message = "Attraction reported successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN,USER")]
    [HttpPost("unreport-attraction")]
    public async Task<IActionResult> UnreportAttraction([FromBody] UnreportAttractionDto unreportDto)
    {
        try
        {
            var userId = User.FindFirstValue("UserId");

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found.");

            if (string.IsNullOrEmpty(unreportDto.AttractionId))
                return BadRequest("Invalid attraction ID.");

            var result = await _attractionService.UnreportAttractionAsync(unreportDto.AttractionId, userId);

            if (!result.Success)
                return NotFound(new { Message = result.Message });

            return Ok(new { Message = "Attraction unreported successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN,USER")]
    [HttpGet("my-reported-attractions")]
    public async Task<IActionResult> GetMyReportedAttractions()
    {
        try
        {
            var userId = User.FindFirstValue("UserId");

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found or unauthorized.");

            var reportedAttractions = await _attractionService.GetReportedAttractionsByUserAsync(userId);

            return Ok(new
            {
                Message = "Reported attractions fetched successfully.",
                Count = reportedAttractions.Count,
                Data = reportedAttractions
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
