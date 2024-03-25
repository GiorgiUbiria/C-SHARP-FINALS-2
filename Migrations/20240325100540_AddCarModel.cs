using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finals.Migrations
{
    /// <inheritdoc />
    public partial class AddCarModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loans_AspNetUsers_ApplicationUserId",
                table: "Loans");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Products_ProductId",
                table: "Loans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Loans",
                table: "Loans");

            migrationBuilder.RenameTable(
                name: "Loans",
                newName: "Loan");

            migrationBuilder.RenameIndex(
                name: "IX_Loans_ProductId",
                table: "Loan",
                newName: "IX_Loan_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Loans_ApplicationUserId",
                table: "Loan",
                newName: "IX_Loan_ApplicationUserId");

            migrationBuilder.AddColumn<int>(
                name: "CarId",
                table: "Loan",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Loan",
                table: "Loan",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Model = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c8b6b1-e2b6-45e8-b044-8f2178a90111",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "961e85d5-8bc2-4329-bc06-fa03f32f0329", "AQAAAAIAAYagAAAAEJT7Cxi31iY2FJ2u5I2oXR0WupCy8wSQsM0QeqGh4C4/Shtw+NrMcFEDEb6XJa9q2w==", "f138368f-3639-41e4-acea-097f96acc44d" });

            migrationBuilder.CreateIndex(
                name: "IX_Loan_CarId",
                table: "Loan",
                column: "CarId",
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Loan_Car_For_CarLoan",
                table: "Loan",
                sql: "((LoanType = 1 AND CarId IS NOT NULL) OR LoanType != 1)");

            migrationBuilder.AddForeignKey(
                name: "FK_Loan_AspNetUsers_ApplicationUserId",
                table: "Loan",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Loan_Cars_CarId",
                table: "Loan",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Loan_Products_ProductId",
                table: "Loan",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loan_AspNetUsers_ApplicationUserId",
                table: "Loan");

            migrationBuilder.DropForeignKey(
                name: "FK_Loan_Cars_CarId",
                table: "Loan");

            migrationBuilder.DropForeignKey(
                name: "FK_Loan_Products_ProductId",
                table: "Loan");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Loan",
                table: "Loan");

            migrationBuilder.DropIndex(
                name: "IX_Loan_CarId",
                table: "Loan");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Loan_Car_For_CarLoan",
                table: "Loan");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "Loan");

            migrationBuilder.RenameTable(
                name: "Loan",
                newName: "Loans");

            migrationBuilder.RenameIndex(
                name: "IX_Loan_ProductId",
                table: "Loans",
                newName: "IX_Loans_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Loan_ApplicationUserId",
                table: "Loans",
                newName: "IX_Loans_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Loans",
                table: "Loans",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c8b6b1-e2b6-45e8-b044-8f2178a90111",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "03fa47a2-b078-4f83-9c41-cbe9e8140bd1", "AQAAAAIAAYagAAAAEPktDmVxUir5T32kJU19kniJkZPjIr0wK3rNFP0mdl/qmrloy5sWjfNaMhRkBQzHig==", "6355e601-ae66-430e-b447-fe091580cc22" });

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_AspNetUsers_ApplicationUserId",
                table: "Loans",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Products_ProductId",
                table: "Loans",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
