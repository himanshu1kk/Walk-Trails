
using System.Linq.Expressions;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NzWalks.Data;
using NzWalks.Extensions;
using NzWalks.Models.Domain;
using NzWalks.Models.DTO;
using NzWalks.Service.Attract;

// using NzWalks.Service.Attract;
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
    public async Task<IActionResult> GetAllAttractions()
    {
        var attractions = await _attractionService.GetAllAttractionsAsync();
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
        [FromQuery] string? searchBy = null)
    {
        try
        {
            var attractions = await _attractionService.SearchAttractionsAsync(searchTerm, searchBy);
            return Ok(attractions);
        }
        catch (Exception ex)
        {
            // Log the exception here
            return BadRequest(ex.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN,USER")]
    [HttpPut("update-attraction")]
    public async Task<IActionResult> UpdateAttraction([FromBody] UpdateAttractionDto attractionDto)
    {
        try
        {
            var userId = User.FindFirstValue("UserId"); // Assuming user is authenticated
            // userId = "456"

            if (attractionDto == null)
            {
                return BadRequest("Attraction data cannot be null");
            }

            // Call service to update the attraction
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


    [HttpPost("upvote-attraction")]
    public async Task<IActionResult> UpvoteAttraction([FromQuery] string attractionId)
    {
        try
        {
            // Get user ID from JWT claims (simulated here as a string for example)
            var userId = "123"; // Get this from JWT or other user context

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token");

            if (string.IsNullOrEmpty(attractionId))
                return BadRequest("Attraction ID is required");

            // Delegate the actual upvoting logic to the service
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

    // API to add or update the Student Tip
    [HttpPost("add-student-tip")]
    public async Task<IActionResult> AddStudentTip([FromQuery] string attractionId, [FromBody] string studentTip)
    {
        try
        {
            if (string.IsNullOrEmpty(attractionId))
                return BadRequest("Attraction ID is required");

            if (string.IsNullOrEmpty(studentTip))
                return BadRequest("Student Tip is required");

            // Call the service to add/update the Student Tip
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

    // API to add or update the Favorites count
    [HttpPost("add-favorites")]
    public async Task<IActionResult> AddFavorites([FromQuery] string attractionId, [FromBody] int favorites)
    {
        try
        {
            if (string.IsNullOrEmpty(attractionId))
                return BadRequest("Attraction ID is required");

            if (favorites < 0)
                return BadRequest("Favorites cannot be negative");

            // Call the service to add/update the Favorites count
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

            await  _nzWalksDbContext.Reviews.AddAsync(review);
            await  _nzWalksDbContext.SaveChangesAsync();

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

    // [Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN,USER")]
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

}

    
