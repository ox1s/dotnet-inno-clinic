using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Appointment.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAppointmentsView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE VIEW appointment.appointments_view AS
                SELECT
                    a.appointment_id,
                    CAST(a.""Duration_Start"" AS DATE) AS local_date,
                    a.""Duration_Start"" AS duration_start,
                    a.""Duration_End"" AS duration_end,
                    a.is_approved,

                    a.doctor_id,
                    d.first_name AS doctor_first_name,
                    d.last_name AS doctor_last_name,
                    d.middle_name AS doctor_middle_name,

                    a.patient_id,
                    p.first_name AS patient_first_name,
                    p.last_name AS patient_last_name,

                    acc.phone_number AS patient_phone,

                    a.service_id,
                    s.""ServiceName"" AS service_name

                FROM appointment.appointments a
                LEFT JOIN profile.doctors d ON a.doctor_id = d.account_profile_id
                LEFT JOIN profile.patients p ON a.patient_id = p.account_profile_id

                LEFT JOIN identity.accounts acc ON p.account_id = acc.""Id""
                LEFT JOIN clinic_management.""Services"" s ON a.service_id = s.""Id"";
                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
