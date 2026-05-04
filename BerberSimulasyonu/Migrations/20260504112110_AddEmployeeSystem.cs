using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BerberSimulasyonu.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Appointments",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedDate",
                table: "Appointments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Position = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    HourlyRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    WorkStartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WorkEndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DefaultWorkStart = table.Column<TimeSpan>(type: "time", nullable: false),
                    DefaultWorkEnd = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "DefaultWorkEnd", "DefaultWorkStart", "Email", "FirstName", "HireDate", "HourlyRate", "IsActive", "LastName", "PhoneNumber", "Position", "WorkEndTime", "WorkStartTime" },
                values: new object[,]
                {
                    { 1, new TimeSpan(0, 18, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0), "ahmet@berber.com", "Ahmet", new DateTime(2023, 5, 4, 14, 21, 8, 812, DateTimeKind.Local).AddTicks(3152), 150m, true, "Yılmaz", "05321234567", "Usta Berber", null, null },
                    { 2, new TimeSpan(0, 18, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0), "mehmet@berber.com", "Mehmet", new DateTime(2024, 5, 4, 14, 21, 8, 812, DateTimeKind.Local).AddTicks(3163), 120m, true, "Kaya", "05327654321", "Berber", null, null },
                    { 3, new TimeSpan(0, 18, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0), "ali@berber.com", "Ali", new DateTime(2025, 11, 4, 14, 21, 8, 812, DateTimeKind.Local).AddTicks(3169), 80m, true, "Demir", "05339876543", "Çırak", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_EmployeeId",
                table: "Appointments",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Employees_EmployeeId",
                table: "Appointments",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Employees_EmployeeId",
                table: "Appointments");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_EmployeeId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "CompletedDate",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Appointments");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);
        }
    }
}
