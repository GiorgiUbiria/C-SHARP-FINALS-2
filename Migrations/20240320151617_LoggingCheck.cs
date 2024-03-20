using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finals.Migrations
{
    /// <inheritdoc />
    public partial class LoggingCheck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c8b6b1-e2b6-45e8-b044-8f2178a90111",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ab5854fc-e4c5-49af-bec4-c803ec83e534", "AQAAAAIAAYagAAAAEBpNU7uhk6s/7f3tp7oTbVFYsE0A2kGQQZilzCBSOjwi4eW7YlHzLydyrh1PQSyWOw==", "04bf5149-37cc-4dea-8762-21f36d1e557f" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c8b6b1-e2b6-45e8-b044-8f2178a90111",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "57f87963-148c-44ee-a009-afad48c7a6d2", "AQAAAAIAAYagAAAAEHQ0lIufJJaj2PDcuzw4B99Ee1TTQqvKRQdwYCFdPIqVNiLYpmoU05+HPOEDT7kg+g==", "ec4f7799-4e16-4730-8c2c-c194f918dd51" });
        }
    }
}
