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

        var logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../Resources", "logo.png");
        byte[]? clinicLogo = logoPath is not null ? File.ReadAllBytes(logoPath) : null;

        Document.Create(container =>
        {

            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(1.25f, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(18).FontColor(Colors.Black).FontFamily(Fonts.Calibri));

                if (clinicLogo is not null)
                {
                    page.Footer()
                        .AlignRight()
                        .Width(120)
                        .Image(clinicLogo)
                        .FitWidth();
                }

                page.Content().Column(column =>
                {
                    column.Spacing(28);

                    column.Item()
                        .PaddingTop(20)
                        .Background(Colors.Grey.Lighten2)
                        .Text("Approved appointment details")
                        .SemiBold()
                        .FontSize(36);

                    column.Item().Row(row =>
                    {
                        row.Spacing(32);

                        row.ConstantItem(220).Column(leftColumn =>
                        {
                            leftColumn.Spacing(8);
                            leftColumn.Item().Text("Appointment №").SemiBold();
                            leftColumn.Item().Text($"{appointment.Id}").FontSize(13);
                        });

                        row.RelativeItem().Column(rightColumn =>
                        {
                            rightColumn.Spacing(10);

                            rightColumn.Item().Column(details =>
                            {
                                details.Spacing(8);
                                details.Item().Text("Patient name").SemiBold();
                                details.Item().Text($"{patient.LastName}, {patient.FirstName} {patient.MiddleName}");
                            });

                            rightColumn.Item().Column(details =>
                            {
                                details.Spacing(8);
                                details.Item().Text("Doctor name").SemiBold();
                                details.Item().Text($"{doctor.LastName}, {doctor.FirstName} {doctor.MiddleName}");
                            });

                            rightColumn.Item().Column(details =>
                            {
                                details.Spacing(8);
                                details.Item().Text("Date and Time").SemiBold();
                                details.Item().Text($"{appointment.Duration.Start:dd.MM.yyyy HH:mm} - {appointment.Duration.End:HH:mm}");
                            });
                        });
                    });
                });
            });
        }).GeneratePdf(pdfStream);

        return pdfStream;
    }
}