using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finals.Migrations
{
    /// <inheritdoc />
    public partial class LoanRequestedAmountAndFinalAmount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Loans",
                newName: "RequstedAmount");

            migrationBuilder.AddColumn<int>(
                name: "FinalAmount",
                table: "Loans",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c8b6b1-e2b6-45e8-b044-8f2178a90111",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "869123ef-9af0-4898-b700-41073d6a5f25", "AQAAAAIAAYagAAAAELsgzuMzYCDQTJmRBTmR4UdsF3pIFFyOo5sbBIi1ZZP7QsCZ3lE7juzNCXmA81l8jw==", "a3d27d2f-1a2f-4adb-b772-28983590cbd4" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinalAmount",
                table: "Loans");

            migrationBuilder.RenameColumn(
                name: "RequstedAmount",
                table: "Loans",
                newName: "Amount");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c8b6b1-e2b6-45e8-b044-8f2178a90111",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9ca3a722-c495-4913-b0f8-eed38b704317", "AQAAAAIAAYagAAAAEFxjqakJUi6y0mgme3YGtg0o1WSojlFkbMbvw0EwVV5e3QOtXMdH9gUofhztRc8eDA==", "7ff01be0-19b2-407f-baf3-eb97f27bc626" });
        }
    }
}
