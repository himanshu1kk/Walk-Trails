using System.ComponentModel.DataAnnotations;

namespace NzWalks.Models.Dto{
    public class AddRegionRequestDto
    {
        public string Code { get; set; }

        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }

         public string? CountryName { get; set; }
        public string? StateName { get; set; }
        public string? DistrictName { get; set; }

        public string? PinCode { get; set; }

        public string? LocalRegionName { get; set; }

        

    }
}