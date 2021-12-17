using Microsoft.EntityFrameworkCore.Migrations;

namespace IoTGateway.DataAccess.Migrations
{
    public partial class expression : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValueFactor",
                table: "DeviceVariables");

            migrationBuilder.AddColumn<string>(
                name: "Expressions",
                table: "DeviceVariables",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Expressions",
                table: "DeviceVariables");

            migrationBuilder.AddColumn<double>(
                name: "ValueFactor",
                table: "DeviceVariables",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
