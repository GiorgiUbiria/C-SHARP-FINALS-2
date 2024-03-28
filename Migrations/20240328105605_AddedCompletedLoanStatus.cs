using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finals.Migrations
{
    /// <inheritdoc />
    public partial class AddedCompletedLoanStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c8b6b1-e2b6-45e8-b044-8f2178a90111",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "17ed9fac-2208-4527-aac4-e993aadb9ca0", "AQAAAAIAAYagAAAAEH1BDHXgEq6oF3k+FOXF++zreUsn2LR1oxONcSrOkq4rZgjlb3pTqUt0gB0u5JC6ag==", "57d087d1-94b2-4cb1-9be6-7d8005c8c898" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c8b6b1-e2b6-45e8-b044-8f2178a90111",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "eecd0f20-bd13-47c6-b81e-d170b21290ed", "AQAAAAIAAYagAAAAEG9Y/WPhJGVSPPXpnMvfEnFiS067aToVddMf5TpZLHxswl5uqr99a0ojOmlAeD2EDw==", "5e8ce3e0-79c4-4308-b728-3a2af1d20e5c" });
        }
    }
}
