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
            
        }

        public DbSet<ApplicationUser> AspNetUsers { get; set; }
        public DbSet<GenderTypes> Gender_Types { get; set; }


    }
}
