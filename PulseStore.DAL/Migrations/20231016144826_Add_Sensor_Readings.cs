using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PulseStore.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Add_Sensor_Readings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sensors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceMacAddress = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SensorType = table.Column<int>(type: "int", nullable: false),
                    StockId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sensors_Stocks_StockId",
                        column: x => x.StockId,
                        principalTable: "Stocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SensorReading",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReadingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Temperature = table.Column<float>(type: "real", nullable: true),
                    Humidity = table.Column<float>(type: "real", nullable: true),
                    SensorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorReading", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SensorReading_Sensors_SensorId",
                        column: x => x.SensorId,
                        principalTable: "Sensors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SensorReading_SensorId",
                table: "SensorReading",
                column: "SensorId");

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_DeviceMacAddress",
                table: "Sensors",
                column: "DeviceMacAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_StockId",
                table: "Sensors",
                column: "StockId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SensorReading");

            migrationBuilder.DropTable(
                name: "Sensors");
        }
    }
}
