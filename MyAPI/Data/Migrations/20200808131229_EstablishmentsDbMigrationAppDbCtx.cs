using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAPI.Data.Migrations
{
    public partial class EstablishmentsDbMigrationAppDbCtx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EstablishmentsTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstablishmentsTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Establishments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    VatNum = table.Column<string>(maxLength: 25, nullable: false),
                    Email = table.Column<string>(maxLength: 255, nullable: false),
                    Description = table.Column<string>(maxLength: 2000, nullable: true),
                    IsValidated = table.Column<bool>(nullable: false),
                    ManagerId = table.Column<string>(nullable: false),
                    TypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Establishments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Establishments_AspNetUsers_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Establishments_EstablishmentsTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "EstablishmentsTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EstablishmentsAddresses",
                columns: table => new
                {
                    EstablishmentId = table.Column<int>(nullable: false),
                    Country = table.Column<string>(maxLength: 100, nullable: false),
                    City = table.Column<string>(maxLength: 100, nullable: false),
                    ZipCode = table.Column<string>(maxLength: 20, nullable: false),
                    Street = table.Column<string>(maxLength: 100, nullable: false),
                    HouseNumber = table.Column<string>(maxLength: 20, nullable: false),
                    BoxNumber = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstablishmentsAddresses", x => x.EstablishmentId);
                    table.ForeignKey(
                        name: "FK_EstablishmentsAddresses_Establishments_EstablishmentId",
                        column: x => x.EstablishmentId,
                        principalTable: "Establishments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EstablishmentsDetails",
                columns: table => new
                {
                    EstablishmentId = table.Column<int>(nullable: false),
                    Phone = table.Column<string>(maxLength: 25, nullable: true),
                    Email = table.Column<string>(maxLength: 255, nullable: true),
                    WebsiteUrl = table.Column<string>(nullable: true),
                    ShortUrl = table.Column<string>(maxLength: 512, nullable: true),
                    InstagramUrl = table.Column<string>(nullable: true),
                    FacebookUrl = table.Column<string>(nullable: true),
                    LinkedInUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstablishmentsDetails", x => x.EstablishmentId);
                    table.ForeignKey(
                        name: "FK_EstablishmentsDetails_Establishments_EstablishmentId",
                        column: x => x.EstablishmentId,
                        principalTable: "Establishments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EstablishmentsNews",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    EstablishmentId = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Text = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstablishmentsNews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstablishmentsNews_Establishments_EstablishmentId",
                        column: x => x.EstablishmentId,
                        principalTable: "Establishments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EstablishmentsOpeningTimes",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    EstablishmentId = table.Column<int>(nullable: false),
                    DayOfWeek = table.Column<int>(nullable: false),
                    IsSpecialDay = table.Column<bool>(nullable: false),
                    SpecialDayDate = table.Column<DateTime>(nullable: true),
                    IsOpen = table.Column<bool>(nullable: false),
                    OpeningHour = table.Column<DateTime>(nullable: true),
                    ClosingHour = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstablishmentsOpeningTimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstablishmentsOpeningTimes_Establishments_EstablishmentId",
                        column: x => x.EstablishmentId,
                        principalTable: "Establishments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EstablishmentsPictures",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    EstablishmentId = table.Column<int>(nullable: false),
                    Picture = table.Column<byte[]>(nullable: false),
                    IsLogo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstablishmentsPictures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstablishmentsPictures_Establishments_EstablishmentId",
                        column: x => x.EstablishmentId,
                        principalTable: "Establishments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NewsPictures",
                columns: table => new
                {
                    NewsId = table.Column<string>(nullable: false),
                    Picture = table.Column<byte[]>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsPictures", x => x.NewsId);
                    table.ForeignKey(
                        name: "FK_NewsPictures_EstablishmentsNews_NewsId",
                        column: x => x.NewsId,
                        principalTable: "EstablishmentsNews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Establishments_ManagerId",
                table: "Establishments",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Establishments_TypeId",
                table: "Establishments",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EstablishmentsNews_EstablishmentId",
                table: "EstablishmentsNews",
                column: "EstablishmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EstablishmentsOpeningTimes_EstablishmentId",
                table: "EstablishmentsOpeningTimes",
                column: "EstablishmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EstablishmentsPictures_EstablishmentId",
                table: "EstablishmentsPictures",
                column: "EstablishmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstablishmentsAddresses");

            migrationBuilder.DropTable(
                name: "EstablishmentsDetails");

            migrationBuilder.DropTable(
                name: "EstablishmentsOpeningTimes");

            migrationBuilder.DropTable(
                name: "EstablishmentsPictures");

            migrationBuilder.DropTable(
                name: "NewsPictures");

            migrationBuilder.DropTable(
                name: "EstablishmentsNews");

            migrationBuilder.DropTable(
                name: "Establishments");

            migrationBuilder.DropTable(
                name: "EstablishmentsTypes");
        }
    }
}
