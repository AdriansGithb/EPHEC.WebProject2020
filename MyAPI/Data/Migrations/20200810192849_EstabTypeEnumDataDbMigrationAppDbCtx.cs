using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAPI.Data.Migrations
{
    public partial class EstabTypeEnumDataDbMigrationAppDbCtx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "EstablishmentsTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "Bar" },
                    { 1, "NightClub" },
                    { 2, "ConcertHall" },
                    { 3, "StudentsAssociation" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EstablishmentsTypes",
                keyColumn: "Id",
                keyValue: 0);

            migrationBuilder.DeleteData(
                table: "EstablishmentsTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "EstablishmentsTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "EstablishmentsTypes",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
