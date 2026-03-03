using Appointment.Api.Data;
using Appointment.Api.Extensions;

using InnoClinic.Shared.DTOs;

namespace Appointment.Api.Features.Receptionist.ApproveAppointment;

public class ApproveAppointmentHandler
{
    public static async Task<IResult> HandleAsync(
        Guid id,
        AppointmentDbContext context)
    {
        var appointment = await context.Appointments.FindAsync(id);
        if (appointment is null)
            return Results.NotFound();
        appointment.Approve();

        // var patient = await context.Patients.FindAsync(appointment.PatientId);
        // var doctor = await context.Doctors.FindAsync(appointment.DoctorId);

        var doctor = new DoctorDto(Guid.NewGuid(), "John", "Doe", "M", true);
        var patient = new PatientDto(Guid.NewGuid(), "Jane", "Smith", "A", "+37533333333", true);
        var pdfStream = GeneratePdfExtensions.GeneratePdf(doctor!, patient!, appointment);

        pdfStream.Position = 0;

        await context.SaveChangesAsync();

        return Results.File(
            pdfStream,
            "application/pdf",
            fileDownloadName: $"appointment_{appointment.Id}.pdf");
    }
}