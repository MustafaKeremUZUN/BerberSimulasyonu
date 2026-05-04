using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BerberSimulasyonu.Migrations
{
    /// <inheritdoc />
    public partial class RemoveImageUrlsFromServices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 6,
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 7,
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 8,
                column: "ImageUrl",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://images.pexels.com/photos/2182970/pexels-photo-2182970.jpeg?w=400&h=300&fit=crop");

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "https://images.pexels.com/photos/3993444/pexels-photo-3993444.jpeg?w=400&h=300&fit=crop");

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "https://images.pexels.com/photos/1239288/pexels-photo-1239288.jpeg?w=400&h=300&fit=crop");

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: "https://images.pexels.com/photos/2379004/pexels-photo-2379004.jpeg?w=400&h=300&fit=crop");

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImageUrl",
                value: "https://images.pexels.com/photos/428340/pexels-photo-428340.jpeg?w=400&h=300&fit=crop");

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 6,
                column: "ImageUrl",
                value: "https://images.pexels.com/photos/3184418/pexels-photo-3184418.jpeg?w=400&h=300&fit=crop");

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 7,
                column: "ImageUrl",
                value: "https://images.pexels.com/photos/3182812/pexels-photo-3182812.jpeg?w=400&h=300&fit=crop");

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 8,
                column: "ImageUrl",
                value: "https://images.pexels.com/photos/3812842/pexels-photo-3812842.jpeg?w=400&h=300&fit=crop");
        }
    }
}
