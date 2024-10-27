using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_commerce.Migrations
{
    /// <inheritdoc />
    public partial class updatehistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Histories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Histories_OrderId",
                table: "Histories",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Histories_Orders_OrderId",
                table: "Histories",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Histories_Orders_OrderId",
                table: "Histories");

            migrationBuilder.DropIndex(
                name: "IX_Histories_OrderId",
                table: "Histories");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Histories");
        }
    }
}
