using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finals.Migrations
{
    /// <inheritdoc />
    public partial class RelationLoanProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Loans",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c8b6b1-e2b6-45e8-b044-8f2178a90111",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2088b921-fefd-4f6c-b093-8582f6f35435", "AQAAAAIAAYagAAAAELXEg7rdSbYryhH4K7uF0V2zTLkh/u7kg+dHL4n1OhodtAiU4osaq2S+CsojLL7YTQ==", "ab701d07-4ab2-4bbb-a8fb-1139363ff4df" });

            migrationBuilder.CreateIndex(
                name: "IX_Loans_ProductId",
                table: "Loans",
                column: "ProductId",
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Loan_Product_For_Installment",
                table: "Loans",
                sql: "((LoanType = 2 AND ProductId IS NOT NULL) OR LoanType != 2)");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Products_ProductId",
                table: "Loans",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Products_ProductId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_ProductId",
                table: "Loans");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Loan_Product_For_Installment",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Loans");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c8b6b1-e2b6-45e8-b044-8f2178a90111",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "59905d5e-5d5b-48e1-b9d2-da938eecb66c", "AQAAAAIAAYagAAAAEEiRtJ/olJk7qQD4OQjqntE4Z6BEnLu8ZnsEa0eCIokngmOFvO8tWo3bW8OKrLeDdw==", "eb787a57-bfee-461f-95b1-3a1626d0246b" });
        }
    }
}
