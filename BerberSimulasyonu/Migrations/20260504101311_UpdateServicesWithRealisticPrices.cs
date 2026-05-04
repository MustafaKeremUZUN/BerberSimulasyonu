using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BerberSimulasyonu.Migrations
{
    /// <inheritdoc />
    public partial class UpdateServicesWithRealisticPrices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "ImageUrl", "Price" },
                values: new object[] { "Makine veya makasla standart saç kesimi", "https://images.pexels.com/photos/2182970/pexels-photo-2182970.jpeg?w=400&h=300&fit=crop", 100m });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "ImageUrl", "Price" },
                values: new object[] { "Trend ve modern saç kesim teknikleri", "https://images.pexels.com/photos/3993444/pexels-photo-3993444.jpeg?w=400&h=300&fit=crop", 150m });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "ImageUrl", "Price" },
                values: new object[] { "Tek renk tam saç boyama hizmeti", "https://images.pexels.com/photos/1239288/pexels-photo-1239288.jpeg?w=400&h=300&fit=crop", 400m });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "ImageUrl", "Price" },
                values: new object[] { "Sakal şekillendirme ve tıraş", "https://images.pexels.com/photos/2379004/pexels-photo-2379004.jpeg?w=400&h=300&fit=crop", 80m });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Description", "ImageUrl", "Price" },
                values: new object[] { "Jilet ile profesyonel sakal traşı", "https://images.pexels.com/photos/428340/pexels-photo-428340.jpeg?w=400&h=300&fit=crop", 60m });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Description", "ImageUrl", "Price" },
                values: new object[] { "Sakal ve bıyık boyama hizmeti", "https://images.pexels.com/photos/3184418/pexels-photo-3184418.jpeg?w=400&h=300&fit=crop", 150m });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Description", "ImageUrl", "Price" },
                values: new object[] { "Saç kesimi + sakal kesimi kombin paketi", "https://images.pexels.com/photos/3182812/pexels-photo-3182812.jpeg?w=400&h=300&fit=crop", 160m });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Description", "ImageUrl", "Price" },
                values: new object[] { "Rahatlatıcı ve yenileyici yüz masajı", "https://images.pexels.com/photos/3812842/pexels-photo-3812842.jpeg?w=400&h=300&fit=crop", 100m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "ImageUrl", "Price" },
                values: new object[] { "Standart saç kesim hizmeti", "https://images.unsplash.com/photo-1585747820761-75a3fff3d3c1?w=400&h=300&fit=crop", 150m });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "ImageUrl", "Price" },
                values: new object[] { "Modern tarzda saç kesim", "https://images.unsplash.com/photo-1583947246080-518ba5c7563e?w=400&h=300&fit=crop", 200m });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "ImageUrl", "Price" },
                values: new object[] { "Tam saç boyama hizmeti", "https://images.unsplash.com/photo-1610916609215-8b9a4c7342f0?w=400&h=300&fit=crop", 300m });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "ImageUrl", "Price" },
                values: new object[] { "Standart sakal kesim ve şekillendirme", "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=400&h=300&fit=crop", 100m });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Description", "ImageUrl", "Price" },
                values: new object[] { "Profesyonel sakal traşı", "https://images.unsplash.com/photo-1581291518857-4e27b48ff24e?w=400&h=300&fit=crop", 80m });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Description", "ImageUrl", "Price" },
                values: new object[] { "Sakal boyama hizmeti", "https://images.unsplash.com/photo-1622286342621-7bd181b3d563?w=400&h=300&fit=crop", 120m });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Description", "ImageUrl", "Price" },
                values: new object[] { "Saç kesimi + sakal kesimi paket", "https://images.unsplash.com/photo-1580758462412-f6ac04c2ae1b?w=400&h=300&fit=crop", 220m });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Description", "ImageUrl", "Price" },
                values: new object[] { "Rahatlatıcı yüz masajı", "https://images.unsplash.com/photo-1571019613454-1cb2f08b8d86?w=400&h=300&fit=crop", 50m });
        }
    }
}
