using NzWalks.Models.Domain;
using NzWalks.Models.DTO.Comments;

namespace NzWalks.Services.Comments
{
    public interface ICommentService
    {
        Task  AddCommentAsync(string userId, CreateCommentDto dto);
        Task<List<Comment>> GetCommentsByAttractionIdAsync(string attractionId);
    }
}
