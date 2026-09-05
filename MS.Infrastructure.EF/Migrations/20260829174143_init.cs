using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MS.Infrastructure.EF.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "doctors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Speciality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MedicalSystemNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_doctors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "patients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "schedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_schedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_schedules_doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "appointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Time = table.Column<TimeOnly>(type: "time", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_appointments_doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_appointments_patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_appointments_DoctorId",
                table: "appointments",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_PatientId",
                table: "appointments",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_schedules_DoctorId",
                table: "schedules",
                column: "DoctorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "appointments");

            migrationBuilder.DropTable(
                name: "schedules");

            migrationBuilder.DropTable(
                name: "patients");

            migrationBuilder.DropTable(
                name: "doctors");
        }
    }
}
