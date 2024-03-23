using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finals.Migrations
{
    /// <inheritdoc />
    public partial class AddedProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c8b6b1-e2b6-45e8-b044-8f2178a90111",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c46adc84-9cb9-4954-8faa-b8ad6c61490d", "AQAAAAIAAYagAAAAEPrR5pHqYz7tg+RqhDGSIUQh/+EWjc9KWv0xP658vS7En+o+qFWjYI/k6j/P35TzUw==", "196b69da-06cf-46dd-9422-954e2e292a82" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c8b6b1-e2b6-45e8-b044-8f2178a90111",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "46be1f13-0db9-4a2d-85e4-d1b9d136e29f", "AQAAAAIAAYagAAAAEDZa8fGm4wpdKgf12OeS/1Hw/TMMytsc2xFHFp4GVZg1bQWgVSA/vN09IaaFbWmtbQ==", "179c3266-e4fb-49eb-a006-79ce7f487528" });
        }
    }
}
