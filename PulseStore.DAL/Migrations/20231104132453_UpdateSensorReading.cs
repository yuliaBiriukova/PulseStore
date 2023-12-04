using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PulseStore.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSensorReading : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GatewayMacAddress",
                table: "Sensors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GatewayMacAddress",
                table: "Sensors");
        }
    }
}
