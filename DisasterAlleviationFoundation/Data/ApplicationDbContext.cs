using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DisasterAlleviationFoundation.Models;

namespace DisasterAlleviationFoundation.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<DisasterReport> DisasterReports { get; set; }
        public DbSet<Donation> Donations { get; set; }
    }
}