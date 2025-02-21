using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task1LoginRegister.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleColumnINUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "userlogin",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "userlogin");
        }
    }
}
