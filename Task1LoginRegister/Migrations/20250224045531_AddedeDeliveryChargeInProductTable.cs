using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task1LoginRegister.Migrations
{
    /// <inheritdoc />
    public partial class AddedeDeliveryChargeInProductTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DeliveryCharge",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryCharge",
                table: "Products");
        }
    }
}
