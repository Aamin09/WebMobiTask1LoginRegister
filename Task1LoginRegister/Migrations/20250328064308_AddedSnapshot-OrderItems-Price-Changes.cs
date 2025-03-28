using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
#nullable disable

namespace Task1LoginRegister.Migrations
{
    public partial class AddedSnapshotOrderItemsPriceChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Step 1: First, drop the TotalPrice computed column
            migrationBuilder.Sql("ALTER TABLE OrderItems DROP COLUMN TotalPrice");

            // Step 2: Add the new columns
            migrationBuilder.AddColumn<decimal>(
                name: "SnapshotPrice",
                table: "OrderItems",
                type: "decimal(18,2)",
                nullable: true);  // Allow null initially

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: true);  // Allow null initially

            migrationBuilder.AddColumn<string>(
                name: "ProductSKU",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: true);  // Allow null initially

            // Step 3: Update existing data
            migrationBuilder.Sql(@"
                UPDATE oi
                SET oi.SnapshotPrice = oi.Price,
                    oi.ProductName = p.Name,
                    oi.ProductSKU = p.SKU
                FROM OrderItems oi
                LEFT JOIN Products p ON oi.ProductId = p.ProductId
            ");

            // Step 4: Alter column constraints
            migrationBuilder.AlterColumn<decimal>(
                name: "SnapshotPrice",
                table: "OrderItems",
                type: "decimal(18,2)",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "ProductSKU",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: false);

            // Step 5: Drop the original Price column
            migrationBuilder.DropColumn(
                name: "Price",
                table: "OrderItems");

            // Step 6: Recreate the computed column
            migrationBuilder.Sql(@"
                ALTER TABLE OrderItems 
                ADD TotalPrice AS (SnapshotPrice * Quantity) PERSISTED
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Step 1: Drop the computed column
            migrationBuilder.Sql("ALTER TABLE OrderItems DROP COLUMN TotalPrice");

            // Step 2: Add back the Price column
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "OrderItems",
                type: "decimal(18,2)",
                nullable: false);

            // Step 3: Restore Price from SnapshotPrice
            migrationBuilder.Sql(@"
                UPDATE OrderItems
                SET Price = SnapshotPrice
            ");

            // Step 4: Drop the new columns
            migrationBuilder.DropColumn(
                name: "SnapshotPrice",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ProductSKU",
                table: "OrderItems");

            // Step 5: Recreate the original computed column
            migrationBuilder.Sql(@"
                ALTER TABLE OrderItems 
                ADD TotalPrice AS (Price * Quantity) PERSISTED
            ");
        }
    }
}