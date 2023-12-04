using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PulseStore.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddSensorReadings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SensorReading_Sensors_SensorId",
                table: "SensorReading");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SensorReading",
                table: "SensorReading");

            migrationBuilder.RenameTable(
                name: "SensorReading",
                newName: "SensorReadings");

            migrationBuilder.RenameIndex(
                name: "IX_SensorReading_SensorId",
                table: "SensorReadings",
                newName: "IX_SensorReadings_SensorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SensorReadings",
                table: "SensorReadings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SensorReadings_Sensors_SensorId",
                table: "SensorReadings",
                column: "SensorId",
                principalTable: "Sensors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SensorReadings_Sensors_SensorId",
                table: "SensorReadings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SensorReadings",
                table: "SensorReadings");

            migrationBuilder.RenameTable(
                name: "SensorReadings",
                newName: "SensorReading");

            migrationBuilder.RenameIndex(
                name: "IX_SensorReadings_SensorId",
                table: "SensorReading",
                newName: "IX_SensorReading_SensorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SensorReading",
                table: "SensorReading",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SensorReading_Sensors_SensorId",
                table: "SensorReading",
                column: "SensorId",
                principalTable: "Sensors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
