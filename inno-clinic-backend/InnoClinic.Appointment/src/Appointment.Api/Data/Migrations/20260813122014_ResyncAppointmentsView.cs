using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Appointment.Api.Data.Migrations
{
    /// <summary>
    /// Rebuilds appointment.appointments_view so it matches what AppointmentDbContext maps.
    ///
    /// The original AddAppointmentsView migration produced a view without patient_middle_name,
    /// office_id or office_address, and the two follow-up migrations meant to add them
    /// (ChangePatientAndOfficeColumnsName, ChangeOfficeNameToOfficeAddrress) shipped empty.
    /// Every query against AppointmentViews therefore asked for columns the view did not have.
    ///
    /// It also wrapped creation in an "IF EXISTS (clinic_management.Services)" guard, so on a
    /// database where ClinicManagement had not migrated yet the view was silently skipped while
    /// the migration was still recorded as applied - and never retried. This migration fails
    /// loudly instead; the AppHost already orders appointment-api after the services it joins.
    /// </summary>
    public partial class ResyncAppointmentsView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // DROP + CREATE rather than CREATE OR REPLACE: replacing a view cannot add columns
            // anywhere except the end, and the column set here differs from the old definition.
            migrationBuilder.Sql(
                """
                DROP VIEW IF EXISTS appointment.appointments_view;

                CREATE VIEW appointment.appointments_view AS
                SELECT
                    a.appointment_id,
                    CAST(a.duration_start AS DATE) AS local_date,
                    a.duration_start,
                    a.duration_end,
                    a.is_approved,

                    a.doctor_id,
                    COALESCE(d.first_name, '')  AS doctor_first_name,
                    COALESCE(d.last_name, '')   AS doctor_last_name,
                    COALESCE(d.middle_name, '') AS doctor_middle_name,

                    a.patient_id,
                    COALESCE(p.first_name, '')  AS patient_first_name,
                    COALESCE(p.last_name, '')   AS patient_last_name,
                    COALESCE(p.middle_name, '') AS patient_middle_name,
                    COALESCE(acc.phone_number, '') AS patient_phone,

                    a.service_id,
                    COALESCE(s."ServiceName", '') AS service_name,

                    a.office_id,
                    COALESCE(o."Address", '') AS office_address

                FROM appointment.appointments a
                LEFT JOIN profile.doctors   d ON a.doctor_id  = d.account_profile_id
                LEFT JOIN profile.patients  p ON a.patient_id = p.account_profile_id
                LEFT JOIN identity.accounts acc ON p.account_id = acc."Id"
                LEFT JOIN clinic_management."Services" s ON a.service_id = s."Id"
                LEFT JOIN clinic_management."Offices"  o ON a.office_id  = o."Id";
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS appointment.appointments_view;");
        }
    }
}
