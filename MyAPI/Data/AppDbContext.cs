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
            //Application User + Gender : One to Many Relation
            modelBuilder
                .Entity<ApplicationUser>()
                .HasOne<GenderTypes>(s => s.GenderType)
                .WithMany(g => g.ApplicationUsers)
                .HasForeignKey(s => s.GenderType_Id);

            //Gender Type Enum conversions
            modelBuilder
                .Entity<ApplicationUser>()
                .Property(e => e.GenderType_Id)
                .HasConversion<int>();

            modelBuilder
                .Entity<GenderTypes>()
                .Property(e => e.Id)
                .HasConversion<int>();

            //GenderTypesData loading
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

            //Application User + Establishments : One (mandatory) to Many (optional) Relation
            modelBuilder
                .Entity<ApplicationUser>()
                .HasMany<Establishments>(s => s.Establishments)
                .WithOne(g => g.Manager)
                .HasForeignKey(s => s.ManagerId)
                .OnDelete(DeleteBehavior.Cascade);

            //Establishments + EstablishmentsTypes : One (mandatory) to Many (optional) Relation
            modelBuilder
                .Entity<EstablishmentsTypes>()
                .HasMany<Establishments>(s => s.Establishments)
                .WithOne(g => g.Type)
                .HasForeignKey(s => s.TypeId)
                .OnDelete(DeleteBehavior.Restrict);

            //EstablishmentsTypes Enum conversions
            modelBuilder
                .Entity<EstablishmentsTypes>()
                .Property(e => e.Id)
                .HasConversion<int>();

            modelBuilder
                .Entity<Establishments>()
                .Property(e => e.TypeId)
                .HasConversion<int>();

            //Establishments + EstablishmentsOpeningTimes : One (mandatory) to Many (optional) Relation
            modelBuilder
                .Entity<Establishments>()
                .HasMany<EstablishmentsOpeningTimes>(s => s.OpeningTimes)
                .WithOne(g => g.Establishment)
                .HasForeignKey(s => s.EstablishmentId)
                .OnDelete(DeleteBehavior.Cascade);

            //Establishments + EstablishmentsPictures : One (mandatory) to Many (optional) Relation
            modelBuilder
                .Entity<Establishments>()
                .HasMany<EstablishmentsPictures>(s => s.Pictures)
                .WithOne(g => g.Establishment)
                .HasForeignKey(s => s.EstablishmentId)
                .OnDelete(DeleteBehavior.Cascade);

            //Establishments + EstablishmentsNews : One (mandatory) to Many (optional) Relation
            modelBuilder
                .Entity<Establishments>()
                .HasMany<EstablishmentsNews>(s => s.News)
                .WithOne(g => g.Establishment)
                .HasForeignKey(s => s.EstablishmentId)
                .OnDelete(DeleteBehavior.Cascade);

        }

        public DbSet<ApplicationUser> AspNetUsers { get; set; }
        public DbSet<GenderTypes> Gender_Types { get; set; }
        public DbSet<Establishments> Establishments { get; set; }
        public DbSet<EstablishmentsTypes> EstablishmentsTypes { get; set; }
        public DbSet<EstablishmentsDetails> EstablishmentsDetails { get; set; }
        public DbSet<EstablishmentsAddresses> EstablishmentsAddresses { get; set; }
        public DbSet<EstablishmentsOpeningTimes> EstablishmentsOpeningTimes { get; set; }
        public DbSet<EstablishmentsPictures> EstablishmentsPictures { get; set; }
        public DbSet<EstablishmentsNews> EstablishmentsNews { get; set; }
        public DbSet<NewsPictures> NewsPictures { get; set; }


    }
}
