using Hahn.ApplicationProcess.December2020.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Hahn.ApplicationProcess.December2020.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Applicant> Applicants { get; set; }

    }
}
