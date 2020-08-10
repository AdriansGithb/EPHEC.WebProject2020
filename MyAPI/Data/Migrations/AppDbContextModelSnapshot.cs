﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyAPI.Data;

namespace MyAPI.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MyLibrary.Entities.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("GenderType_Id")
                        .HasColumnType("int");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<bool>("IsProfessional")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("GenderType_Id");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("MyLibrary.Entities.Establishments", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(2000)")
                        .HasMaxLength(2000);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<bool>("IsValidated")
                        .HasColumnType("bit");

                    b.Property<string>("ManagerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<int>("TypeId")
                        .HasColumnType("int");

                    b.Property<string>("VatNum")
                        .IsRequired()
                        .HasColumnType("nvarchar(25)")
                        .HasMaxLength(25);

                    b.HasKey("Id");

                    b.HasIndex("ManagerId");

                    b.HasIndex("TypeId");

                    b.ToTable("Establishments");
                });

            modelBuilder.Entity("MyLibrary.Entities.EstablishmentsAddresses", b =>
                {
                    b.Property<int>("EstablishmentId")
                        .HasColumnType("int");

                    b.Property<string>("BoxNumber")
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("HouseNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.HasKey("EstablishmentId");

                    b.ToTable("EstablishmentsAddresses");
                });

            modelBuilder.Entity("MyLibrary.Entities.EstablishmentsDetails", b =>
                {
                    b.Property<int>("EstablishmentId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("FacebookUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InstagramUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LinkedInUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(25)")
                        .HasMaxLength(25);

                    b.Property<string>("ShortUrl")
                        .HasColumnType("nvarchar(512)")
                        .HasMaxLength(512);

                    b.Property<string>("WebsiteUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EstablishmentId");

                    b.ToTable("EstablishmentsDetails");
                });

            modelBuilder.Entity("MyLibrary.Entities.EstablishmentsNews", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("EstablishmentId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("EstablishmentId");

                    b.ToTable("EstablishmentsNews");
                });

            modelBuilder.Entity("MyLibrary.Entities.EstablishmentsOpeningTimes", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("ClosingHour")
                        .HasColumnType("datetime2");

                    b.Property<int>("DayOfWeek")
                        .HasColumnType("int");

                    b.Property<int>("EstablishmentId")
                        .HasColumnType("int");

                    b.Property<bool>("IsOpen")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSpecialDay")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("OpeningHour")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("SpecialDayDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("EstablishmentId");

                    b.ToTable("EstablishmentsOpeningTimes");
                });

            modelBuilder.Entity("MyLibrary.Entities.EstablishmentsPictures", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("EstablishmentId")
                        .HasColumnType("int");

                    b.Property<bool>("IsLogo")
                        .HasColumnType("bit");

                    b.Property<byte[]>("Picture")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("Id");

                    b.HasIndex("EstablishmentId");

                    b.ToTable("EstablishmentsPictures");
                });

            modelBuilder.Entity("MyLibrary.Entities.EstablishmentsTypes", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("EstablishmentsTypes");

                    b.HasData(
                        new
                        {
                            Id = 0,
                            Name = "Bar"
                        },
                        new
                        {
                            Id = 1,
                            Name = "NightClub"
                        },
                        new
                        {
                            Id = 2,
                            Name = "ConcertHall"
                        },
                        new
                        {
                            Id = 3,
                            Name = "StudentsAssociation"
                        });
                });

            modelBuilder.Entity("MyLibrary.Entities.GenderTypes", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Gender_Types");

                    b.HasData(
                        new
                        {
                            Id = 0,
                            Name = "Male"
                        },
                        new
                        {
                            Id = 1,
                            Name = "Female"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Non_Binary"
                        });
                });

            modelBuilder.Entity("MyLibrary.Entities.NewsPictures", b =>
                {
                    b.Property<string>("NewsId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<byte[]>("Picture")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("NewsId");

                    b.ToTable("NewsPictures");
                });

            modelBuilder.Entity("MyLibrary.Entities.ApplicationUser", b =>
                {
                    b.HasOne("MyLibrary.Entities.GenderTypes", "GenderType")
                        .WithMany("ApplicationUsers")
                        .HasForeignKey("GenderType_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MyLibrary.Entities.Establishments", b =>
                {
                    b.HasOne("MyLibrary.Entities.ApplicationUser", "Manager")
                        .WithMany("Establishments")
                        .HasForeignKey("ManagerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyLibrary.Entities.EstablishmentsTypes", "Type")
                        .WithMany("Establishments")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("MyLibrary.Entities.EstablishmentsAddresses", b =>
                {
                    b.HasOne("MyLibrary.Entities.Establishments", "Establishment")
                        .WithOne("Address")
                        .HasForeignKey("MyLibrary.Entities.EstablishmentsAddresses", "EstablishmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MyLibrary.Entities.EstablishmentsDetails", b =>
                {
                    b.HasOne("MyLibrary.Entities.Establishments", "Establishment")
                        .WithOne("Details")
                        .HasForeignKey("MyLibrary.Entities.EstablishmentsDetails", "EstablishmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MyLibrary.Entities.EstablishmentsNews", b =>
                {
                    b.HasOne("MyLibrary.Entities.Establishments", "Establishment")
                        .WithMany("News")
                        .HasForeignKey("EstablishmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MyLibrary.Entities.EstablishmentsOpeningTimes", b =>
                {
                    b.HasOne("MyLibrary.Entities.Establishments", "Establishment")
                        .WithMany("OpeningTimes")
                        .HasForeignKey("EstablishmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MyLibrary.Entities.EstablishmentsPictures", b =>
                {
                    b.HasOne("MyLibrary.Entities.Establishments", "Establishment")
                        .WithMany("Pictures")
                        .HasForeignKey("EstablishmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MyLibrary.Entities.NewsPictures", b =>
                {
                    b.HasOne("MyLibrary.Entities.EstablishmentsNews", "News")
                        .WithOne("Picture")
                        .HasForeignKey("MyLibrary.Entities.NewsPictures", "NewsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
