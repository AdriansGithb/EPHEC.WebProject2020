using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MyLibrary.Models;

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
            //modelBuilder
            //    .Entity<ApplicationUser>()
            //    .Property(e => e.GenderType_Id)
            //    .HasConversion<int>();

            //modelBuilder
            //    .Entity<GenderTypesModel>()
            //    .Property(e => e.Id)
            //    .HasConversion<int>();

            //modelBuilder
            //    .Entity<GenderTypesModel>().HasData(
            //        Enum.GetValues(typeof(GenderTypesId))
            //            .Cast<GenderTypesId>()
            //            .Select(e => new GenderTypesModel()
            //            {
            //                Id = e,
            //                Name = e.ToString()
            //            })
            //    );
        }
        //private DbSet<ApplicationUser> aspNetUsers { get; set; }
        //private DbSet<GenderTypesModel> Gender_Types { get; set; }

    }
}
