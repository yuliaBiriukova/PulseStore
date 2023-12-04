using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PulseStore.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialCategoryProductProductPhotoData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "BCAA" },
                    { 2, "Fat Burner" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "DateCreated", "Description", "IsPublished", "MaxTemperature", "MinTemperature", "Name", "Price" },
                values: new object[] { 1, 1, new DateTime(2023, 9, 28, 16, 45, 20, 180, DateTimeKind.Local).AddTicks(3137), "High quality balanced amino acid complex.", true, 25, 0, "Olimp Labs, BCAA Xplode Powder, 500 g", 25m });

            migrationBuilder.InsertData(
                table: "ProductPhotos",
                columns: new[] { "Id", "ImagePath", "ProductId", "SequenceNumber" },
                values: new object[] { 1, "https://PulseStorestorage.blob.core.windows.net/photo-container/testproduct.jpeg", 1, 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProductPhotos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
