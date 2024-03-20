using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finals.Migrations
{
    /// <inheritdoc />
    public partial class LoanPeriodType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "LoanPeriod",
                table: "Loans",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c8b6b1-e2b6-45e8-b044-8f2178a90111",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9ca3a722-c495-4913-b0f8-eed38b704317", "AQAAAAIAAYagAAAAEFxjqakJUi6y0mgme3YGtg0o1WSojlFkbMbvw0EwVV5e3QOtXMdH9gUofhztRc8eDA==", "7ff01be0-19b2-407f-baf3-eb97f27bc626" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "LoanPeriod",
                table: "Loans",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c8b6b1-e2b6-45e8-b044-8f2178a90111",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "58192d5f-cf37-42e5-b1be-1cd5e1bf78b6", "AQAAAAIAAYagAAAAENo1uD5n2+hhmjAPn6uiKSyBEVpGiUlBB7M/MJ/3LRa0GiuqhlBAJ8Zf1qIMlzqa1g==", "77e32dde-3a6a-4957-b412-c8f44740a52a" });
        }
    }
}
