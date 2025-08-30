using System;
namespace NzWalks.Models.DTO.Comments
{
    
    public class CreateCommentDto
    {
        public string AttractionId { get; set; }
        public string Message { get; set; }
    }
}