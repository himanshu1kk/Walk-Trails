using System;

namespace NzWalks.Models.DTO.Comments
{
    // For creating a new comment (input only)
    public class CreateCommentDto
    {
        public string AttractionId { get; set; }
        public string Message { get; set; }
    }
}