using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DOTNET_lab4.Models;
using Microsoft.EntityFrameworkCore;

namespace DOTNET_lab4.Data
{
    public class SchoolCommunityContext : DbContext
    {
        public SchoolCommunityContext(DbContextOptions<SchoolCommunityContext> options) : base(options)
        {
        }
        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<CommunityAdvert> Adverts { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Community> Communities { get; set; }
        public DbSet<CommunityMembership> Memberships { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Advertisement>().ToTable("Advertisement");
            modelBuilder.Entity<Community>().ToTable("Community");
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<CommunityMembership>()
                .HasKey(c => new { c.StudentID, c.CommunityID });
            modelBuilder.Entity<CommunityAdvert>()
                .HasKey(c => new { c.AdvertisementID, c.CommunityID });
        }
    }
}
