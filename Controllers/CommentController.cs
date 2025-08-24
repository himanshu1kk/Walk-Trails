using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NzWalks.Models.DTO.Comments;
using NzWalks.Services.Comments;
using System.Security.Claims;
using NzWalks.Models.Domain;

namespace NzWalks.Controllers
{
    [ApiController]
    [Route("api/v1/comments")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        // ✅ Add a comment
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN,USER")]
        [HttpPost("add")]
        public async Task<IActionResult> AddComment([FromBody] CreateCommentDto dto)
        {
            try
            {
                var userId = User.FindFirstValue("UserId");
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("User ID not found in token");

                await _commentService.AddCommentAsync(userId, dto);
                return Ok(new { Message = "Comment added successfully"});
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // ✅ Get all comments for an attraction
        [HttpGet("by-attraction")]
        public async Task<IActionResult> GetComments([FromQuery] string attractionId)
        {
            try
            {
                List<Comment> comments = await _commentService.GetCommentsByAttractionIdAsync(attractionId);
                return Ok(new { Count = comments.Count, Data = comments });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
