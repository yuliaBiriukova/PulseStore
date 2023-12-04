using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PulseStore.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddTemplateFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TemplateFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateFiles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TemplateFiles_Type",
                table: "TemplateFiles",
                column: "Type",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TemplateFiles");
        }
    }
}
