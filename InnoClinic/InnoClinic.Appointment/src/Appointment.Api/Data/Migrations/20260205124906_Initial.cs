using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Appointment.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "appointment");

            migrationBuilder.CreateTable(
                name: "time_ranges",
                schema: "appointment",
                columns: table => new
                {
                    time_range_id = table.Column<Guid>(type: "uuid", nullable: false),
                    start = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    end = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_time_ranges", x => x.time_range_id);
                });

            migrationBuilder.CreateTable(
                name: "appointments",
                schema: "appointment",
                columns: table => new
                {
                    appointment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    patient_id = table.Column<Guid>(type: "uuid", nullable: false),
                    doctor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    service_id = table.Column<Guid>(type: "uuid", nullable: false),
                    office_id = table.Column<Guid>(type: "uuid", nullable: false),
                    time_range_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_approved = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appointments", x => x.appointment_id);
                    table.ForeignKey(
                        name: "FK_appointments_time_ranges_time_range_id",
                        column: x => x.time_range_id,
                        principalSchema: "appointment",
                        principalTable: "time_ranges",
                        principalColumn: "time_range_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_appointments_time_range_id",
                schema: "appointment",
                table: "appointments",
                column: "time_range_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "appointments",
                schema: "appointment");

            migrationBuilder.DropTable(
                name: "time_ranges",
                schema: "appointment");
        }
    }
}
