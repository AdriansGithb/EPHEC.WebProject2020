using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MyLibrary.Entities;

namespace MyAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<ApplicationUser>()
                .HasOne<GenderTypes>(s => s.GenderType)
                .WithMany(g => g.ApplicationUsers)
                .HasForeignKey(s => s.GenderType_Id);

            modelBuilder
                .Entity<ApplicationUser>()
                .Property(e => e.GenderType_Id)
                .HasConversion<int>();

            modelBuilder
                .Entity<GenderTypes>()
                .Property(e => e.Id)
                .HasConversion<int>();

            modelBuilder
                .Entity<GenderTypes>().HasData(
                    Enum.GetValues(typeof(GenderTypesId))
                        .Cast<GenderTypesId>()
                        .Select(e => new GenderTypes()
                        {
                            Id = e,
                            Name = e.ToString()
                        })
                );

        }

        public DbSet<ApplicationUser> AspNetUsers { get; set; }
        public DbSet<GenderTypes> Gender_Types { get; set; }


    }
}
