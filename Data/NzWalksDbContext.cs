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

        public DbSet<User> Users { get; set; } 
        public DbSet<UserVerification> UserVerificationDb { get; set; } 
        public DbSet<Attraction> Attractions { get; set; }
        public DbSet<AttractionImage> AttractionImages { get; set; }
        public DbSet<Location> LocationInfos { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Contact> Contact { get; set; } 
        public DbSet<AttractionReport> AttractionReports { get; set; }
        public DbSet<Comment> Comment { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasKey(u => u.UserId);
            modelBuilder.Entity<User>().Property(u => u.UserId).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Email).IsRequired();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            modelBuilder.Entity<Contact>().HasKey(u => u.ContactId);
            modelBuilder.Entity<Contact>().Property(u => u.ContactId).IsRequired();

            modelBuilder.Entity<Review>().HasKey(r => r.ReviewId);

            modelBuilder.Entity<Attraction>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.ToTable("Attractions");
            });

            modelBuilder.Entity<AttractionImage>(entity =>
            {
                entity.HasKey(ai => ai.Id);
                entity.ToTable("AttractionImages");
                entity.HasIndex(ai => ai.AttractionId);
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(l => l.Id);
                entity.ToTable("Locations");
                entity.HasIndex(l => l.AttractionId).IsUnique();
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(ai => ai.CommentId);
                entity.ToTable("CommnetsOnAttraction");
            });

            modelBuilder.Entity<AttractionReport>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.ToTable("AttractionReports");
                entity.Property(r => r.Reason).IsRequired().HasMaxLength(500);
                entity.HasIndex(r => r.AttractionId);
            });
        }
    }
}
