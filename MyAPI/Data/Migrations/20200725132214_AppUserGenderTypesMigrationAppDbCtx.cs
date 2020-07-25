using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAPI.Data.Migrations
{
    public partial class AppUserGenderTypesMigrationAppDbCtx : Migration
    {
        /* Leave all code as commented only
            These entities are created via the IdentityServerDbContext because IS manages the users
            We leave all lines here as comments because we do not want to create tables & columns again
            but we want to have a good AppDbContextSnapshot for our business context    */
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Gender_Types",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false),
            //        Name = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Gender_Types", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetUsers",
            //    columns: table => new
            //    {
            //        Id = table.Column<string>(nullable: false),
            //        UserName = table.Column<string>(nullable: true),
            //        NormalizedUserName = table.Column<string>(nullable: true),
            //        Email = table.Column<string>(nullable: true),
            //        NormalizedEmail = table.Column<string>(nullable: true),
            //        EmailConfirmed = table.Column<bool>(nullable: false),
            //        PasswordHash = table.Column<string>(nullable: true),
            //        SecurityStamp = table.Column<string>(nullable: true),
            //        ConcurrencyStamp = table.Column<string>(nullable: true),
            //        PhoneNumber = table.Column<string>(nullable: true),
            //        PhoneNumberConfirmed = table.Column<bool>(nullable: false),
            //        TwoFactorEnabled = table.Column<bool>(nullable: false),
            //        LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
            //        LockoutEnabled = table.Column<bool>(nullable: false),
            //        AccessFailedCount = table.Column<int>(nullable: false),
            //        LastName = table.Column<string>(nullable: true),
            //        FirstName = table.Column<string>(nullable: true),
            //        BirthDate = table.Column<DateTime>(nullable: false),
            //        IsProfessional = table.Column<bool>(nullable: false),
            //        IsAdmin = table.Column<bool>(nullable: false),
            //        GenderType_Id = table.Column<int>(nullable: false),
            //        GenderTypeId = table.Column<int>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetUsers", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_AspNetUsers_Gender_Types_GenderTypeId",
            //            column: x => x.GenderTypeId,
            //            principalTable: "Gender_Types",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_AspNetUsers_GenderTypeId",
            //    table: "AspNetUsers",
            //    column: "GenderTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "AspNetUsers");

            //migrationBuilder.DropTable(
            //    name: "Gender_Types");
        }
    }
}
