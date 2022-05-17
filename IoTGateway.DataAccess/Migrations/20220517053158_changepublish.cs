using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IoTGateway.DataAccess.Migrations
{
    public partial class changepublish : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CgUpload",
                table: "Devices",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<uint>(
                name: "EnforcePeriod",
                table: "Devices",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0u);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CgUpload",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "EnforcePeriod",
                table: "Devices");
        }
    }
}
