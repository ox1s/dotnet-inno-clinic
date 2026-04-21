using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Appointment.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDurationProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Duration_Start",
                schema: "appointment",
                table: "appointments",
                newName: "duration_start");

            migrationBuilder.RenameColumn(
                name: "Duration_End",
                schema: "appointment",
                table: "appointments",
                newName: "duration_end");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "duration_start",
                schema: "appointment",
                table: "appointments",
                newName: "Duration_Start");

            migrationBuilder.RenameColumn(
                name: "duration_end",
                schema: "appointment",
                table: "appointments",
                newName: "Duration_End");
        }
    }
}
