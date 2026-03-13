using Appointment.Api.Data;
using Appointment.Api.Extensions;
using Appointment.Api.External;

using InnoClinic.Shared.DTOs;

namespace Appointment.Api.Features.Receptionist.ApproveAppointment;

public static class ApproveAppointmentHandler
{
    public static async Task<IResult> HandleAsync(
        Guid id,
        AppointmentDbContext context,
        IProfileGateway profileGateway)
    {
        var appointment = await context.Appointments.FindAsync(id);
        if (appointment is null)
            return Results.NotFound();
        appointment.Approve();

        var patient = await profileGateway.GetPatientAsync(appointment.PatientId);
        var doctor = await profileGateway.GetDoctorAsync(appointment.DoctorId);

        if (patient.Value is null || doctor.Value is null || !patient.Succeeded || !doctor.Succeeded)
        {
            return Results.NotFound();
        }

        var pdfStream = GeneratePdfExtensions.GeneratePdf(doctor.Value, patient.Value, appointment);

        pdfStream.Position = 0;

        await context.SaveChangesAsync();

        return Results.File(
            pdfStream,
            "application/pdf",
            fileDownloadName: $"appointment_{appointment.Id}.pdf");
    }
}