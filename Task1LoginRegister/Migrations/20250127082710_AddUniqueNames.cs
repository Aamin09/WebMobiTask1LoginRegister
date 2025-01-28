using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task1LoginRegister.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "Subcategory_Name_Unique",
                table: "Subcategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Product_Name_Unique",
                table: "Products",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Category_Name_Unique",
                table: "Categories",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Subcategory_Name_Unique",
                table: "Subcategories");

            migrationBuilder.DropIndex(
                name: "Product_Name_Unique",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "Category_Name_Unique",
                table: "Categories");
        }
    }
}
