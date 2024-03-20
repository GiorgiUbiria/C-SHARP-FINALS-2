﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finals.Migrations
{
    /// <inheritdoc />
    public partial class DeclinedTypeForLoanStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c8b6b1-e2b6-45e8-b044-8f2178a90111",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "58192d5f-cf37-42e5-b1be-1cd5e1bf78b6", "AQAAAAIAAYagAAAAENo1uD5n2+hhmjAPn6uiKSyBEVpGiUlBB7M/MJ/3LRa0GiuqhlBAJ8Zf1qIMlzqa1g==", "77e32dde-3a6a-4957-b412-c8f44740a52a" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c8b6b1-e2b6-45e8-b044-8f2178a90111",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ab5854fc-e4c5-49af-bec4-c803ec83e534", "AQAAAAIAAYagAAAAEBpNU7uhk6s/7f3tp7oTbVFYsE0A2kGQQZilzCBSOjwi4eW7YlHzLydyrh1PQSyWOw==", "04bf5149-37cc-4dea-8762-21f36d1e557f" });
        }
    }
}