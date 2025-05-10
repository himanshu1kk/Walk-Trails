using System.ComponentModel.DataAnnotations.Schema;

namespace NzWalks{
    public class Image{
        //creating this model to add image in the 

        public Guid Id{get; set;}
        [NotMapped]//This attribute tells Entity Framework (EF) Core not to map this property to a database column. In other words, the File property is part of the Image class for handling the file upload process but is not stored in the database.
        public IFormFile File{get; set;}

        public string FileName{get;set;}

        public string? FileDescription{get;set;}

        public string FileExtension{get;set;}

        public long FileSizeInBytes{get;set;}

        public string FilePath{get;set;}

    }
}