using Microsoft.EntityFrameworkCore;
using RaceDayP1.Models;

// Beginning of RaceDay database namespace
namespace RaceDayP1.Data
{
    // Beginning of RaceDayDbContext class
    public class RaceDayDbContext : DbContext
    {
        // Beginning of RaceDayDbContext constructor
        public RaceDayDbContext(DbContextOptions<RaceDayDbContext> options)
            : base(options)
        {
        }
        // End of RaceDayDbContext constructor

        public DbSet<User> Users { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<EventType> EventTypes { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Enrolment> Enrolments { get; set; }

        public DbSet<Result> Results { get; set; }

        // Beginning of OnModelCreating method
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Event distance precision
            modelBuilder.Entity<Event>()
                .Property(e => e.Distance)
                .HasPrecision(10, 2);

            // User to Event relationship
            modelBuilder.Entity<Event>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.OrganiserID)
                .OnDelete(DeleteBehavior.Restrict);

            // EventType to Event relationship
            modelBuilder.Entity<Event>()
                .HasOne<EventType>()
                .WithMany()
                .HasForeignKey(e => e.EventTypeID)
                .OnDelete(DeleteBehavior.Restrict);

            // Event to Category relationship
            modelBuilder.Entity<Category>()
                .HasOne<Event>()
                .WithMany()
                .HasForeignKey(c => c.EventID)
                .OnDelete(DeleteBehavior.Cascade);

            // User to Enrolment relationship
            modelBuilder.Entity<Enrolment>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            // Event to Enrolment relationship
            modelBuilder.Entity<Enrolment>()
                .HasOne<Event>()
                .WithMany()
                .HasForeignKey(e => e.EventID)
                .OnDelete(DeleteBehavior.Restrict);

            // Category to Enrolment relationship
            modelBuilder.Entity<Enrolment>()
                .HasOne<Category>()
                .WithMany()
                .HasForeignKey(e => e.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);

            // Enrolment to Result relationship
            modelBuilder.Entity<Result>()
                .HasOne<Enrolment>()
                .WithMany()
                .HasForeignKey(r => r.EnrolmentID)
                .OnDelete(DeleteBehavior.Cascade);
        }
        // End of OnModelCreating method
    }
    // End of RaceDayDbContext class
}
// End of RaceDay database namespace