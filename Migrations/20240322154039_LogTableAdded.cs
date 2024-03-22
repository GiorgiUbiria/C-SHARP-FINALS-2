using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finals.Migrations
{
    /// <inheritdoc />
    public partial class LogTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c8b6b1-e2b6-45e8-b044-8f2178a90111",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a30c6549-ccfd-4972-9301-9d56386fa1ca", "AQAAAAIAAYagAAAAEAy+UpMcl2ZV7Zy93zEv2LeItRdl8RVf/pGGruLQZc5L4sAzaZhhc55qBoifIgkXUw==", "b23dc7f4-93b0-4b86-bea5-48724f9057d5" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c8b6b1-e2b6-45e8-b044-8f2178a90111",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "dc5dad2b-78be-491d-858c-01e3d637fd68", "AQAAAAIAAYagAAAAEJIoXzMYcEU045fMHcudwIJptdEx+xaJOHhghwbrtHoFsp18vLUxvWo0L/BrSFHcGQ==", "ae28b046-3d8e-44f6-9b1a-8e60a8cf9d41" });
        }
    }
}
