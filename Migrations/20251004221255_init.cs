using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Clinic_Appointment_System.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    DoctorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Specialization = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.DoctorId);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    PatientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.PatientId);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    AppointmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    AppointmentDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.AppointmentId);
                    table.ForeignKey(
                        name: "FK_Appointments_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "DoctorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appointments_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "DoctorId", "Name", "Phone", "Specialization" },
                values: new object[,]
                {
                    { 1, "د. سالي ماهر", "0123456789", "باطنة" },
                    { 2, "د. سارة علي", "0987654321", "أطفال" },
                    { 3, "د. محمد حسن", "0112233445", "جراحة" }
                });

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "PatientId", "Address", "BirthDate", "Name", "Phone" },
                values: new object[,]
                {
                    { 1, "القاهرة", new DateTime(1990, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "علي أحمد", "0123456789" },
                    { 2, "الجيزة", new DateTime(1985, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "فاطمة خالد", "0987654321" },
                    { 3, "الإسكندرية", new DateTime(2000, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "خالد محمود", "0112233445" },
                    { 4, "المنصورة", new DateTime(1995, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "منى سعيد", "0129988776" },
                    { 5, "أسيوط", new DateTime(2010, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "يوسف عمر", "0155544332" }
                });

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "AppointmentId", "AppointmentDateTime", "DoctorId", "Notes", "PatientId", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 10, 6, 10, 0, 0, 0, DateTimeKind.Unspecified), 1, "فحص دوري", 1, 0 },
                    { 2, new DateTime(2025, 10, 7, 14, 0, 0, 0, DateTimeKind.Unspecified), 2, "استشارة", 2, 0 },
                    { 3, new DateTime(2025, 10, 4, 9, 0, 0, 0, DateTimeKind.Unspecified), 1, "متابعة", 3, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorId",
                table: "Appointments",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientId",
                table: "Appointments",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "Doctors");

            migrationBuilder.DropTable(
                name: "Patients");
        }
    }
}
