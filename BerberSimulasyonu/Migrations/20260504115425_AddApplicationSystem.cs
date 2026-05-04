using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BerberSimulasyonu.Migrations
{
    /// <inheritdoc />
    public partial class AddApplicationSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.CreateTable(
                name: "EmployeeApplications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Experience = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DesiredPosition = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DesiredHourlyRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ApplicationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CorrectAnswers = table.Column<int>(type: "int", nullable: false),
                    TotalQuestions = table.Column<int>(type: "int", nullable: false),
                    QuizScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ReviewedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    QuizAnswers = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeApplications_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuizQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    OptionA = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OptionB = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OptionC = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OptionD = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CorrectOption = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    Explanation = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Difficulty = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizQuestions", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "QuizQuestions",
                columns: new[] { "Id", "Category", "CorrectOption", "CreatedDate", "Difficulty", "Explanation", "IsActive", "OptionA", "OptionB", "OptionC", "OptionD", "Question" },
                values: new object[,]
                {
                    { 1, "Saç Kesimi", "A", new DateTime(2026, 5, 4, 14, 54, 24, 293, DateTimeKind.Local).AddTicks(7084), 1, "Klasik erkek saç kesiminde genellikle 3-4 numara makine başlığı kullanılır.", true, "3-4 numara", "1-2 numara", "6-8 numara", "9-12 numara", "Makine ile saç kesiminde genellikle hangi numara makine başlığı kullanılır?" },
                    { 2, "Sakal", "A", new DateTime(2026, 5, 4, 14, 54, 24, 293, DateTimeKind.Local).AddTicks(7104), 1, "Sıcak havlu uygulaması sakalları yumuşatır ve cildi tıraşa hazırlar.", true, "Sıcak havlu uygulaması", "Soğuk su ile yıkama", "Kuru tıraş", "Jilet direkt uygulanır", "Sakal traşı öncesi cildi hazırlamak için en iyi yöntem nedir?" },
                    { 3, "Hijyen", "A", new DateTime(2026, 5, 4, 14, 54, 24, 293, DateTimeKind.Local).AddTicks(7107), 1, "Hijyen için her müşteri için temiz ve steril malzeme kullanmak esastır.", true, "Her müşteri için temiz malzeme kullanmak", "Hızlı çalışmak", "Pahalı ürünler kullanmak", "Dükkanı süslemek", "Berber dükkanında hijyen için en önemli kural nedir?" },
                    { 4, "Saç Boyama", "A", new DateTime(2026, 5, 4, 14, 54, 24, 293, DateTimeKind.Local).AddTicks(7110), 2, "Saç boyası genellikle 30-45 dakika etkisini gösterir.", true, "30-45 dakika", "5-10 dakika", "2-3 saat", "Hemen durulayabilir", "Saç boyama işlemi sonrasında ne kadar beklemek gerekir?" },
                    { 5, "Sakal", "A", new DateTime(2026, 5, 4, 14, 54, 24, 293, DateTimeKind.Local).AddTicks(7112), 2, "Jilet ile tıraşta genellikle 30 derecelik açı kullanılır.", true, "30 derece", "90 derece", "45 derece", "15 derece", "Jilet ile tıraşta en sık kullanılan açı nedir?" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeApplications_EmployeeId",
                table: "EmployeeApplications",
                column: "EmployeeId",
                unique: true,
                filter: "[EmployeeId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeApplications");

            migrationBuilder.DropTable(
                name: "QuizQuestions");

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "DefaultWorkEnd", "DefaultWorkStart", "Email", "FirstName", "HireDate", "HourlyRate", "IsActive", "LastName", "PhoneNumber", "Position", "WorkEndTime", "WorkStartTime" },
                values: new object[,]
                {
                    { 1, new TimeSpan(0, 18, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0), "ahmet@berber.com", "Ahmet", new DateTime(2023, 5, 4, 14, 21, 8, 812, DateTimeKind.Local).AddTicks(3152), 150m, true, "Yılmaz", "05321234567", "Usta Berber", null, null },
                    { 2, new TimeSpan(0, 18, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0), "mehmet@berber.com", "Mehmet", new DateTime(2024, 5, 4, 14, 21, 8, 812, DateTimeKind.Local).AddTicks(3163), 120m, true, "Kaya", "05327654321", "Berber", null, null },
                    { 3, new TimeSpan(0, 18, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0), "ali@berber.com", "Ali", new DateTime(2025, 11, 4, 14, 21, 8, 812, DateTimeKind.Local).AddTicks(3169), 80m, true, "Demir", "05339876543", "Çırak", null, null }
                });
        }
    }
}
