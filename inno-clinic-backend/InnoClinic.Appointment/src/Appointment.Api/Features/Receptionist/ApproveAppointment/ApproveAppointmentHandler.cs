using Appointment.Api.Data;

using InnoClinic.Shared.DTOs;

using QuestPDF;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

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
        var pdfStream = GeneratePdf(doctor!, patient!, appointment);

        pdfStream.Position = 0;

        await context.SaveChangesAsync();

        return Results.File(
            pdfStream,
            "application/pdf",
            fileDownloadName: $"appointment_{appointment.Id}.pdf");
    }

    private static MemoryStream GeneratePdf(DoctorDto doctor, PatientDto patient, Data.Appointment appointment)
    {
        Settings.License = LicenseType.Community;
        var pdfStream = new MemoryStream();

        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.Black);
                page.DefaultTextStyle(x => x.FontSize(20).FontColor(Colors.White));
                page.Content().Column(col =>
                {
                    col.Item().Text($"Appointment ID: {appointment.Id}");
                    col.Item().Text($"Patient Name: {patient.LastName}, {patient.FirstName} {patient.MiddleName}");
                    col.Item().Text($"Doctor Name: {doctor.LastName}, {doctor.FirstName} {doctor.MiddleName}");
                    col.Item().Text($"Time: {appointment.Duration.Start:HH:mm} - {appointment.Duration.End:HH:mm}");
                    col.Item().Text($"Status: Approved");
                });
            });
        }).GeneratePdf(pdfStream);

        return pdfStream;
    }
}