namespace NzWalks.Models.Domain{
    public class Region
    {
        public Guid Id { get; set; }
        public string Code { get; set; } //why dont UI use this code to fetch the information about the regions

        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }


        //REGION NEEDS A LOT OF REFACTORING



        public string? CountryName { get; set; }
        public string ?StateName { get; set; }
        public string ?DistrictName { get; set; }

        public string? PinCode { get; set; }

        public string? LocalRegionName { get; set; }




        

        

    }
}