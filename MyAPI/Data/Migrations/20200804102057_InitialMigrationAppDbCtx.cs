using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAPI.Data.Migrations
{
    public partial class InitialMigrationAppDbCtx : Migration
    {
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
            //        GenderType_Id = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetUsers", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_AspNetUsers_Gender_Types_GenderType_Id",
            //            column: x => x.GenderType_Id,
            //            principalTable: "Gender_Types",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.InsertData(
            //    table: "Gender_Types",
            //    columns: new[] { "Id", "Name" },
            //    values: new object[] { 0, "Male" });

            //migrationBuilder.InsertData(
            //    table: "Gender_Types",
            //    columns: new[] { "Id", "Name" },
            //    values: new object[] { 1, "Female" });

            //migrationBuilder.InsertData(
            //    table: "Gender_Types",
            //    columns: new[] { "Id", "Name" },
            //    values: new object[] { 2, "Non_Binary" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_AspNetUsers_GenderType_Id",
            //    table: "AspNetUsers",
            //    column: "GenderType_Id");
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
