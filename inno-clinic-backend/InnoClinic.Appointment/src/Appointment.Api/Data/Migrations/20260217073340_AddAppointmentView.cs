// using Microsoft.EntityFrameworkCore.Migrations;

// #nullable disable

// namespace Appointment.Api.Data.Migrations
// {
//     /// <inheritdoc />
//     public partial class AddAppointmentView : Migration
//     {
//         /// <inheritdoc />
//         protected override void Up(MigrationBuilder migrationBuilder)
//         {
//             migrationBuilder.Sql(@"
//             CREATE OR REPLACE VIEW appointment.appointments_view AS
//             SELECT 
//                 a.id AS appointment_id,
//                 a.local_date,           
//                 a.duration_start,      
//                 a.duration_end,
//                 a.is_approved,

//                 a.doctor_id,
//                 d.first_name AS doctor_first_name,
//                 d.last_name AS doctor_last_name,
//                 d.middle_name AS doctor_middle_name,

//                 a.patient_id,
//                 p.first_name AS patient_first_name,
//                 p.last_name AS patient_last_name,
//                 p.phone_number AS patient_phone,

//                 a.service_id,
//                 s.name AS service_name

//             FROM appointment.appointments a
//             JOIN profile.doctors d ON a.doctor_id = d.id
//             JOIN profile.patients p ON a.patient_id = p.id
//             JOIN service.services s ON a.service_id = s.id;
//         ");
//         }

//         /// <inheritdoc />
//         protected override void Down(MigrationBuilder migrationBuilder)
//         {
//             migrationBuilder.Sql("DROP VIEW appointment.appointments_view;");
//         }
//     }
// }