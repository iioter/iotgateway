using Microsoft.EntityFrameworkCore.Migrations;

namespace IoTGateway.DataAccess.Migrations
{
    public partial class disperse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Disperse",
                table: "SystemConfig",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Disperse",
                table: "SystemConfig");
        }
    }
}
