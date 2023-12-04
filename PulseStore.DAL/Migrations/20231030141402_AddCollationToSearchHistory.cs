using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PulseStore.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCollationToSearchHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Query",
                table: "SearchHistory",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                collation: "SQL_Latin1_General_CP1_CS_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Query",
                table: "SearchHistory",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldCollation: "SQL_Latin1_General_CP1_CS_AS");
        }
    }
}
