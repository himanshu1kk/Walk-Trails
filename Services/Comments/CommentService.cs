using Microsoft.EntityFrameworkCore;
using NzWalks.Data;
using NzWalks.Models.Domain;
using NzWalks.Models.DTO.Comments;

namespace NzWalks.Services.Comments
{
    public class CommentService : ICommentService
    {
        private readonly NzWalksDbContext _dbContext;

        public CommentService(NzWalksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddCommentAsync(string userId, CreateCommentDto dto)
        {
            try
            {
                var attraction = await _dbContext.Attractions
                    .FirstOrDefaultAsync(a => a.Id == dto.AttractionId && a.IsActive);

                if (attraction == null)
                    throw new ArgumentException("Attraction not found or inactive.");

                var comment = new Comment
                {
                    AttractionId = dto.AttractionId,
                    UserId = userId,
                    Message = dto.Message
                };

                await _dbContext.Comment.AddAsync(comment);
                await _dbContext.SaveChangesAsync();
            }
           
            catch (Exception ex)
            {
                throw new Exception("Unexpected error while adding comment.", ex);
            }
        }

        public async Task<List<Comment>> GetCommentsByAttractionIdAsync(string attractionId)
        {
            try
            {
                return await _dbContext.Comment
                    .Where(c => c.AttractionId == attractionId)
                    .OrderByDescending(c => c.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching comments for attraction.", ex);
            }
        }
    }
}
