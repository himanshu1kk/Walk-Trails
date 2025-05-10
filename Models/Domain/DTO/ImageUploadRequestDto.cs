using System.ComponentModel.DataAnnotations;

namespace NzWalks.Models.Dto{
    public class ImageUplaodRequestDto{
        [Required]
        public IFormFile File{ get; set; }

        [Required]
        public string FileNmae{get;set;}

        public string? FileDescription{get;set;}
        

    }
}