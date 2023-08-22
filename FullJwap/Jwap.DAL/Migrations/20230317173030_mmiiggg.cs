using Microsoft.EntityFrameworkCore.Migrations;

namespace Jwap.DAL.Migrations
{
    public partial class mmiiggg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Online",
                table: "Connections",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Online",
                table: "Connections");
        }
    }
}
