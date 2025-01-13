using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task1LoginRegister.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActiveColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "userlogin", // Use the existing table name
                nullable: false,
                defaultValue: false); // Default value set to false
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "userlogin");
        }
    }

}
