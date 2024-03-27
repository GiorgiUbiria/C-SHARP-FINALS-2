using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finals.Migrations
{
    /// <inheritdoc />
    public partial class AddedApplicationUserEmailFieldToLoans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserEmail",
                table: "Loan",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c8b6b1-e2b6-45e8-b044-8f2178a90111",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8fd977ae-ccdf-463c-a188-e46a4a10bbe8", "AQAAAAIAAYagAAAAEF0Tlr+L0uQIMYOaV3a9Buv3pBcL8iREs7E8esMPnjB5NgVYrJ6b538sbM+EowumJQ==", "de00691e-4558-46ed-8412-9ee5ea8b5dec" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationUserEmail",
                table: "Loan");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c8b6b1-e2b6-45e8-b044-8f2178a90111",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "26500e84-f118-4002-9301-310240ae295a", "AQAAAAIAAYagAAAAEAAhw0E/xIFAh7C6MOJp2j+amOOQLKrScvl+hZ3WfToqAWg63vqaFp1lquuuV5eQCA==", "5a8ce1d9-4d6f-4fd0-9d9f-ab9777d8725b" });
        }
    }
}
