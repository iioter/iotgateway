using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IoTGateway.DataAccess.Migrations
{
    public partial class mqttClientId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "SystemConfig",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "SystemConfig");
        }
    }
}
