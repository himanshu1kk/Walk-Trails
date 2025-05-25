using Microsoft.EntityFrameworkCore;
using NzWalks.Models.Domain;
using NzWalks.Models.DTO;

namespace NzWalks.Data
{
    public class NzWalksDbContext : DbContext
    {
        public NzWalksDbContext(DbContextOptions<NzWalksDbContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }

        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<User> Users { get; set; } 
        public DbSet<UserVerification> UserVerificationDb { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ Configure User Entity (optional fine-tuning)
            modelBuilder.Entity<User>().HasKey(u => u.UserId);
            modelBuilder.Entity<User>().Property(u => u.UserId).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Email).IsRequired();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique(); // Optional: Unique email

            // Seed Difficulties
            var difficulties = new List<Difficulty>
            {
                new Difficulty
                {
                    Id = Guid.Parse("f362b16a-6f46-4f16-a01a-ae7a9b8c2678"),
                    Name = "Easy"
                },
                new Difficulty
                {
                    Id = Guid.Parse("b084bc9e-da20-43c7-831f-56fa43eab106"),
                    Name = "Medium"
                },
                new Difficulty
                {
                    Id = Guid.Parse("6c218143-7a91-47bc-9c17-d9cd7a92d9c7"),
                    Name = "Hard"
                }
            };
            modelBuilder.Entity<Difficulty>().HasData(difficulties);

            // Seed Regions
            var regions = new List<Region>
            {
                new Region
                {
                    Id = Guid.Parse("919276a6-b956-41f5-856c-f27653292008"),
                    Name = "New Zealand",
                    Code = "AKL",
                    RegionImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/9/9e/Auckland_City_Skyline.jpg/1200px-Auckland_City_Skyline.jpg"
                },
                new Region
                {
                    Id = Guid.Parse("919276a6-b956-41f5-856c-f27653292009"),
                    Name = "Queensland",
                    Code = "QLD",
                    RegionImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/7/72/Brisbane_CITY_Harbour_and_CBD.jpg/1200px-Brisbane_CITY_Harbour_and_CBD.jpg"
                },
                new Region
                {
                    Id = Guid.Parse("919276a6-b956-41f5-856c-f27653292010"),
                    Name = "Victoria",
                    Code = "VIC",
                    RegionImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/0/01/Melbourne_City_Skyline.jpg/1200px-Melbourne_City_Skyline.jpg"
                },
                new Region
                {
                    Id = Guid.Parse("919276a6-b956-41f5-856c-f27653292011"),
                    Name = "Tasmania",
                    Code = "TAS",
                    RegionImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c6/Hobart_City_Skyline.jpg/1200px-Hobart_City_Skyline.jpg"
                }
            };
            modelBuilder.Entity<Region>().HasData(regions);
        }
    }
}
