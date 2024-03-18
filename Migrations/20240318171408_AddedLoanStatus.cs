using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finals.Migrations
{
    /// <inheritdoc />
    public partial class AddedLoanStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoanStatus",
                table: "Loans",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c8b6b1-e2b6-45e8-b044-8f2178a90111",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "57f87963-148c-44ee-a009-afad48c7a6d2", "AQAAAAIAAYagAAAAEHQ0lIufJJaj2PDcuzw4B99Ee1TTQqvKRQdwYCFdPIqVNiLYpmoU05+HPOEDT7kg+g==", "ec4f7799-4e16-4730-8c2c-c194f918dd51" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoanStatus",
                table: "Loans");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c8b6b1-e2b6-45e8-b044-8f2178a90111",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "67b9707a-3b07-493f-a2f9-fa10568be080", "AQAAAAIAAYagAAAAEI/4Vr4cEIMYT7wgKTdy3PxgrT08DjOfC38kLY9cLGkMVUwUTlaVNn0IpeKVUnC0+Q==", "e13359cd-8c9d-4edf-8466-2849a72712e2" });
        }
    }
}
