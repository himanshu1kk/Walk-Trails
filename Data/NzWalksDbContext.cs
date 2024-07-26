using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NzWalks.Models.Domain;
namespace NzWalks.Data{
    public class NzWalksDbContext:DbContext{
        public NzWalksDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions){
            
        }
        public DbSet<Difficulty> Difficulties { get; set; }
         public DbSet<Region> Regions { get; set; }
          public DbSet<Walk> Walks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Seed the data for the difficulty
            //easy medium hard
            var difficulties = new List<Difficulty>()
            {
                new Difficulty()
                {
                Id = Guid.Parse("f362b16a-6f46-4f16-a01a-ae7a9b8c2678"), 
                Name = "Easy"
                },
                new Difficulty(){
                    Id = Guid.Parse("b084bc9e-da20-43c7-831f-56fa43eab106"), 
                    Name = "Medium"
                    },
                new Difficulty()
                     {
                    Id = Guid.Parse("6c218143-7a91-47bc-9c17-d9cd7a92d9c7"),
                    Name = "Hard"
                         }
            };
            //seed the diffifculty in the database
            modelBuilder.Entity<Difficulty>().HasData(difficulties);

            //seed the data for the regions
            //New Zealand
            var regions = new List<Region>()
            {
                new Region()
                {
                Id = Guid.Parse("919276a6-b956-41f5-856c-f27653292008"),
                Name = "New Zealand",
                Code = "AKL",
                RegionImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/9/9e/Auckland_City_Skyline.jpg/1200px-Auckland_City_Skyline.jpg"
                },
                //similarly add 5 more regions of newzeland
                new Region{
                    Id = Guid.Parse("919276a6-b956-41f5-856c-f27653292009"),
                    Name = "Queensland",
                    Code = "QLD",
                    RegionImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/7/72/Brisbane_CITY_Harbour_and_CBD.jpg/1200px-Brisbane_CITY_Harbour_and_CBD.jpg"
                },
                //similarly add 5 more regions of queensland
                new Region{
                    Id = Guid.Parse("919276a6-b956-41f5-856c-f27653292010"),
                    Name = "Victoria",
                    Code = "VIC",
                    RegionImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/0/01/Melbourne_City_Skyline.jpg/1200px-Melbourne_City_Skyline.jpg"
                },
                //similarly add 5 more regions of victoria
                new Region{
                    Id = Guid.Parse("919276a6-b956-41f5-856c-f27653292011"),
                    Name = "Tasmania",
                    Code = "TAS",
                    RegionImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c6/Hobart_City_Skyline.jpg/1200px-Hobart_City_Skyline.jpg"
                },
                //similarly add 5 more regions of wellington
                
                
            };
            //seed the regions in the database
            modelBuilder.Entity<Region>().HasData(regions);

        }
    }
}