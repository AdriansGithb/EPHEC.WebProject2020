using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAPI.Data.Migrations
{
    public partial class GenderTypesMigrationAppDbCtx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GenderType_Id",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Gender_Types",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gender_Types", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Gender_Types",
                columns: new[] { "Id", "Name" },
                values: new object[] { 0, "Male" });

            migrationBuilder.InsertData(
                table: "Gender_Types",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Female" });

            migrationBuilder.InsertData(
                table: "Gender_Types",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Non_Binary" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_GenderType_Id",
                table: "AspNetUsers",
                column: "GenderType_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Gender_Types_GenderType_Id",
                table: "AspNetUsers",
                column: "GenderType_Id",
                principalTable: "Gender_Types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Gender_Types_GenderType_Id",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Gender_Types");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_GenderType_Id",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GenderType_Id",
                table: "AspNetUsers");
        }
    }
}
