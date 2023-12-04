using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PulseStore.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddSecurityUserAndNfcDevice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NfcDevices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NUID = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NfcDevices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SecurityUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    UserType = table.Column<int>(type: "int", nullable: false),
                    NfcDeviceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SecurityUsers_NfcDevices_NfcDeviceId",
                        column: x => x.NfcDeviceId,
                        principalTable: "NfcDevices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SecurityUserStocks",
                columns: table => new
                {
                    SecurityUsersId = table.Column<int>(type: "int", nullable: false),
                    StocksId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityUserStocks", x => new { x.SecurityUsersId, x.StocksId });
                    table.ForeignKey(
                        name: "FK_SecurityUserStocks_SecurityUsers_SecurityUsersId",
                        column: x => x.SecurityUsersId,
                        principalTable: "SecurityUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SecurityUserStocks_Stocks_StocksId",
                        column: x => x.StocksId,
                        principalTable: "Stocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "NfcDevices",
                columns: new[] { "Id", "NUID" },
                values: new object[,]
                {
                    { 1, "231C0B0E" },
                    { 2, "2362AD33" },
                    { 3, "3309060E" },
                    { 4, "63D3159A" },
                    { 5, "B39A689A" },
                    { 6, "E9675F15" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_NfcDevices_NUID",
                table: "NfcDevices",
                column: "NUID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SecurityUsers_NfcDeviceId",
                table: "SecurityUsers",
                column: "NfcDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityUserStocks_StocksId",
                table: "SecurityUserStocks",
                column: "StocksId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SecurityUserStocks");

            migrationBuilder.DropTable(
                name: "SecurityUsers");

            migrationBuilder.DropTable(
                name: "NfcDevices");
        }
    }
}
