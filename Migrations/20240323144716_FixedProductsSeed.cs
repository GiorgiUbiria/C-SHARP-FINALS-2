using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finals.Migrations
{
    /// <inheritdoc />
    public partial class FixedProductsSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c8b6b1-e2b6-45e8-b044-8f2178a90111",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "121f75e1-9630-45c7-bd62-9006ef56ac5a", "AQAAAAIAAYagAAAAEHuYFdISvP5FnU47fQ5zHDhnZ+JGZVx2C430VUmDnfL3XfFfvDQMPhFzN8bMosQPuQ==", "e5753a66-4426-4688-992f-8daaa59d2f0a" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c8b6b1-e2b6-45e8-b044-8f2178a90111",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c46adc84-9cb9-4954-8faa-b8ad6c61490d", "AQAAAAIAAYagAAAAEPrR5pHqYz7tg+RqhDGSIUQh/+EWjc9KWv0xP658vS7En+o+qFWjYI/k6j/P35TzUw==", "196b69da-06cf-46dd-9422-954e2e292a82" });
        }
    }
}
