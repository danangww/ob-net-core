using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace DAL
{
    public class OnBoardingDanangDbContext : DbContext
    {
        public OnBoardingDanangDbContext(DbContextOptions<OnBoardingDanangDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Book>().HasIndex(t => t.Title).IsUnique(true);
        }

        public DbSet<Book> Books { get; set; }

    }
}

