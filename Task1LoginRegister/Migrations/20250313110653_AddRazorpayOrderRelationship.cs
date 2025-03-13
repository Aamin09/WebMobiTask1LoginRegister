using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task1LoginRegister.Migrations
{
    /// <inheritdoc />
    public partial class AddRazorpayOrderRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RazorpayOrders_ApplicationOrderId",
                table: "RazorpayOrders",
                column: "ApplicationOrderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RazorpayOrders_Orders_ApplicationOrderId",
                table: "RazorpayOrders",
                column: "ApplicationOrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RazorpayOrders_Orders_ApplicationOrderId",
                table: "RazorpayOrders");

            migrationBuilder.DropIndex(
                name: "IX_RazorpayOrders_ApplicationOrderId",
                table: "RazorpayOrders");
        }
    }
}
