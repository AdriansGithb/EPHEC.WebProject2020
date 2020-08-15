using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAPI.Data.Migrations
{
    public partial class NewsAddTitleDbMigrationAppDbCtx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "EstablishmentsNews",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "EstablishmentsNews");
        }
    }
}
