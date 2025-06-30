
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


    // [Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN,USER")]
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

    // [Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN, USER")]
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
}

    


//      [Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN, USER")]
//     [HttpGet("get-all-walks")]

//     public async Task<IActionResult> GetAllWalks()
//     {

//         try
//         {
           
//             var attraction  = await _attractionService. GetAllWalks();

//             return Ok(attraction);


        

//         }
//         catch (Exception ex)
//         {
//             throw new Exception("their is some error in getting the attraction", ex);

//         }
//     }
// }

