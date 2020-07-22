using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyLibrary.Models;
using System;
using System.Linq;

namespace IdentityServer.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(modelBuilder);
            modelBuilder
                .Entity<ApplicationUser>()
                .HasOne<GenderTypesModel>(s => s.GenderTypeModel)
                .WithMany(g => g.ApplicationUsers)
                .HasForeignKey(s => s.GenderType_Id);
            
            modelBuilder
                .Entity<ApplicationUser>()
                .Property(e => e.GenderType_Id)
                .HasConversion<int>();

            modelBuilder
                .Entity<GenderTypesModel>()
                .Property(e => e.Id)
                .HasConversion<int>();

            modelBuilder
                .Entity<GenderTypesModel>().HasData(
                    Enum.GetValues(typeof(GenderTypesId))
                        .Cast<GenderTypesId>()
                        .Select(e => new GenderTypesModel()
                        {
                            Id = e,
                            Name = e.ToString()
                        })
                );
        }
        private DbSet<GenderTypesModel> Gender_Types { get; set; }
    }
}
