using FluentValidation;

using Appointment.Api.Common;
using Appointment.Api.Data;

using System.Security.Claims;
using Appointment.Api.External;

namespace Appointment.Api.Features.CreateAppointment;

public class CreateAppointmentHandler
{
    public async static Task<IResult> HandleAsync(
        CreateAppointmentRequest request,
        AppointmentDbContext context,
        IValidator<CreateAppointmentRequest> validator,
        ClaimsPrincipal user,
        IServiceGateway ServiceGateway,
        IOfficeGateway OfficeGateway,
        IProfileGateway ProfileGateway)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Results.BadRequest(new Error(
                Code: "Validation",
                Description: validationResult.ToString()));

        var patientIdClaim = user.FindFirst("id");
        if (patientIdClaim == null || !Guid.TryParse(patientIdClaim.Value, out var patientId))
            return Results.Unauthorized();

        var date = DateOnly.FromDateTime(request.StartDateTime);
        var startTime = TimeOnly.FromDateTime(request.StartDateTime);
        var endTime = TimeOnly.FromDateTime(request.EndDateTime);

        var timeRangeResult = TimeRange.Create(startTime, endTime);

        if (!timeRangeResult.Succeeded)
            return Results.BadRequest(timeRangeResult.Error.Description);

        var existingAppointment = context.Appointments
            .Where(a => a.DoctorId == request.DoctorId && a.Date == date)
            .AsEnumerable()
            .FirstOrDefault(a => a.Time.Overlaps(timeRangeResult.Value!));

        if (existingAppointment is not null)
            return Results.BadRequest(Errors.OverlappingAppointment);

        var serviceActiveResult = ServiceGateway.IsServiceActiveAsync(request.ServiceId);
        if (!serviceActiveResult.Result) return Results.BadRequest(Errors.ServiceIsNotActive);
        var officeActiveResult = OfficeGateway.IsOfficeActiveAsync(request.OfficeId);
        if (!officeActiveResult.Result) return Results.BadRequest(Errors.OfficeIsNotActive);
        var doctorActiveResult = ProfileGateway.IsDoctorActiveAsync(request.DoctorId);
        if (!doctorActiveResult.Result) return Results.BadRequest(Errors.DoctorIsNotActive);

        var appointment = Data.Appointment.Create(
            patientId: patientId,
            doctorId: request.DoctorId,
            serviceId: request.ServiceId,
            officeId: request.OfficeId,
            date: date,
            time: timeRangeResult.Value!);

        context.Appointments.Add(appointment);

        await context.SaveChangesAsync();

        return Results.Ok(
                    new CreateAppointmentResponse(
                Id: appointment.Id,
                PatientId: appointment.PatientId,
                DoctorId: appointment.DoctorId,
                ServiceId: appointment.ServiceId,
                OfficeId: appointment.OfficeId,
                StartDateTime: appointment.Date.ToDateTime(appointment.Time.Start),
                EndDateTime: appointment.Date.ToDateTime(appointment.Time.End)));
    }

}

