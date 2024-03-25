using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finals.Migrations
{
    /// <inheritdoc />
    public partial class TestAutoLoanMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c8b6b1-e2b6-45e8-b044-8f2178a90111",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "26500e84-f118-4002-9301-310240ae295a", "AQAAAAIAAYagAAAAEAAhw0E/xIFAh7C6MOJp2j+amOOQLKrScvl+hZ3WfToqAWg63vqaFp1lquuuV5eQCA==", "5a8ce1d9-4d6f-4fd0-9d9f-ab9777d8725b" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c8b6b1-e2b6-45e8-b044-8f2178a90111",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "961e85d5-8bc2-4329-bc06-fa03f32f0329", "AQAAAAIAAYagAAAAEJT7Cxi31iY2FJ2u5I2oXR0WupCy8wSQsM0QeqGh4C4/Shtw+NrMcFEDEb6XJa9q2w==", "f138368f-3639-41e4-acea-097f96acc44d" });
        }
    }
}
