using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Appointment.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAppointmentLookupIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_appointments_doctor_id",
                schema: "appointment",
                table: "appointments",
                column: "doctor_id");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_patient_id",
                schema: "appointment",
                table: "appointments",
                column: "patient_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_appointments_doctor_id",
                schema: "appointment",
                table: "appointments");

            migrationBuilder.DropIndex(
                name: "IX_appointments_patient_id",
                schema: "appointment",
                table: "appointments");
        }
    }
}
