using Microsoft.EntityFrameworkCore;
using RaceDayP1.Models;
using System.Collections.Generic;

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
    }
    // End of RaceDayDbContext class
}
// End of RaceDay database namespace