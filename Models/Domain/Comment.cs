using System;

namespace NzWalks.Models.Domain;

    public class Comment
    {
        public string CommentId { get; set; }

        public string AttractionId { get; set; }
        public string UserId { get; set; }

        public string Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public Comment()
        {
            this.CommentId = Guid.NewGuid().ToString();
        }

    }

