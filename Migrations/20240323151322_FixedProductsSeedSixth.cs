using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Finals.Migrations
{
    /// <inheritdoc />
    public partial class FixedProductsSeedSixth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: -14);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: -13);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: -12);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: -11);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: -10);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: -9);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c8b6b1-e2b6-45e8-b044-8f2178a90111",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "59905d5e-5d5b-48e1-b9d2-da938eecb66c", "AQAAAAIAAYagAAAAEEiRtJ/olJk7qQD4OQjqntE4Z6BEnLu8ZnsEa0eCIokngmOFvO8tWo3bW8OKrLeDdw==", "eb787a57-bfee-461f-95b1-3a1626d0246b" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Price", "Title" },
                values: new object[,]
                {
                    { 9, 64m, "WD 2TB Elements Portable External Hard Drive - USB 3.0 " },
                    { 10, 109m, "SanDisk SSD PLUS 1TB Internal SSD - SATA III 6 Gb/s" },
                    { 11, 109m, "Silicon Power 256GB SSD 3D NAND A55 SLC Cache Performance Boost SATA III 2.5" },
                    { 12, 114m, "WD 4TB Gaming Drive Works with Playstation 4 Portable External Hard Drive" },
                    { 13, 599m, "Acer SB220Q bi 21.5 inches Full HD (1920 x 1080) IPS Ultra-Thin" },
                    { 14, 999.99m, "Samsung 49-Inch CHG90 144Hz Curved Gaming Monitor (LC49HG90DMNXZA) – Super Ultrawide Screen QLED " }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c8b6b1-e2b6-45e8-b044-8f2178a90111",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "987c4ef7-eb49-48e8-9637-c5bae58a9362", "AQAAAAIAAYagAAAAENk+WvZnobsqHwxTWr50BGIusEHoEgUrmo+liOmxyReLKOeKDEHBjKsyuCa8VUesIQ==", "94c12d7a-2020-4502-9393-153a7cfbb22b" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Price", "Title" },
                values: new object[,]
                {
                    { -14, 999.99m, "Samsung 49-Inch CHG90 144Hz Curved Gaming Monitor (LC49HG90DMNXZA) – Super Ultrawide Screen QLED " },
                    { -13, 599m, "Acer SB220Q bi 21.5 inches Full HD (1920 x 1080) IPS Ultra-Thin" },
                    { -12, 114m, "WD 4TB Gaming Drive Works with Playstation 4 Portable External Hard Drive" },
                    { -11, 109m, "Silicon Power 256GB SSD 3D NAND A55 SLC Cache Performance Boost SATA III 2.5" },
                    { -10, 109m, "SanDisk SSD PLUS 1TB Internal SSD - SATA III 6 Gb/s" },
                    { -9, 64m, "WD 2TB Elements Portable External Hard Drive - USB 3.0 " }
                });
        }
    }
}
