using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finals.Migrations
{
    /// <inheritdoc />
    public partial class NullableProductId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c8b6b1-e2b6-45e8-b044-8f2178a90111",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "03fa47a2-b078-4f83-9c41-cbe9e8140bd1", "AQAAAAIAAYagAAAAEPktDmVxUir5T32kJU19kniJkZPjIr0wK3rNFP0mdl/qmrloy5sWjfNaMhRkBQzHig==", "6355e601-ae66-430e-b447-fe091580cc22" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c8b6b1-e2b6-45e8-b044-8f2178a90111",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2088b921-fefd-4f6c-b093-8582f6f35435", "AQAAAAIAAYagAAAAELXEg7rdSbYryhH4K7uF0V2zTLkh/u7kg+dHL4n1OhodtAiU4osaq2S+CsojLL7YTQ==", "ab701d07-4ab2-4bbb-a8fb-1139363ff4df" });
        }
    }
}
