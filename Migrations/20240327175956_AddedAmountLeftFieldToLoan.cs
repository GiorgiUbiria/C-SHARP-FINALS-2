using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finals.Migrations
{
    /// <inheritdoc />
    public partial class AddedAmountLeftFieldToLoan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "RequstedAmount",
                table: "Loan",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<decimal>(
                name: "FinalAmount",
                table: "Loan",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<decimal>(
                name: "AmountLeft",
                table: "Loan",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c8b6b1-e2b6-45e8-b044-8f2178a90111",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "eecd0f20-bd13-47c6-b81e-d170b21290ed", "AQAAAAIAAYagAAAAEG9Y/WPhJGVSPPXpnMvfEnFiS067aToVddMf5TpZLHxswl5uqr99a0ojOmlAeD2EDw==", "5e8ce3e0-79c4-4308-b728-3a2af1d20e5c" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountLeft",
                table: "Loan");

            migrationBuilder.AlterColumn<int>(
                name: "RequstedAmount",
                table: "Loan",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "FinalAmount",
                table: "Loan",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c8b6b1-e2b6-45e8-b044-8f2178a90111",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8fd977ae-ccdf-463c-a188-e46a4a10bbe8", "AQAAAAIAAYagAAAAEF0Tlr+L0uQIMYOaV3a9Buv3pBcL8iREs7E8esMPnjB5NgVYrJ6b538sbM+EowumJQ==", "de00691e-4558-46ed-8412-9ee5ea8b5dec" });
        }
    }
}
