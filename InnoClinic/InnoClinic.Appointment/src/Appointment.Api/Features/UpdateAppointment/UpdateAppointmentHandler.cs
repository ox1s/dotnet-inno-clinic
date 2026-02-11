
using System.Security.Claims;

using Appointment.Api.Common;
using Appointment.Api.Data;
using Appointment.Api.External;

using FluentValidation;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using Throw;

namespace Appointment.Api.Features.UpdateAppointment;

public class UpdateAppointmentHandler
{
    public static async Task<IResult> HandleAsync(Guid id,
        UpdateAppointmentRequest request,
        AppointmentDbContext context,
        IValidator<UpdateAppointmentRequest> validator,
        ClaimsPrincipal user,
        IServiceGateway serviceGateway,
        IOfficeGateway officeGateway,
        IProfileGateway profileGateway)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return TypedResults.BadRequest(new Error(
                Code: "Validation",
                Description: validationResult.ToString()));

        var appointment = await context.Appointments
            .Include(a => a.Duration)
            .FirstOrDefaultAsync(a => a.Id == id);
        if (appointment is null)
            return TypedResults.NotFound();

        var timeRangeResult = TimeRange.Create(
            request.StartDateTime.ToUniversalTime(),
            request.EndDateTime.ToUniversalTime());
            
        timeRangeResult.ThrowIfNull();
        if (!timeRangeResult.Succeeded)
            return TypedResults.BadRequest(timeRangeResult.Error.Description);

        if (await context.IsOverlappingAsync(request.DoctorId, timeRangeResult.Value!))
            return TypedResults.BadRequest(Errors.OverlappingAppointment);

        var serviceActive = serviceGateway.IsServiceActiveAsync(request.ServiceId);
        var officeActive = officeGateway.IsOfficeActiveAsync(request.OfficeId);
        var doctorActiveResult = profileGateway.IsDoctorActiveAsync(request.DoctorId);

        if (!doctorActiveResult.Result)
            return Results.BadRequest(Errors.DoctorIsNotActive);
        if (!serviceActive.Result)
            return Results.BadRequest(Errors.ServiceIsNotActive);
        if (!officeActive.Result)
            return Results.BadRequest(Errors.OfficeIsNotActive);

        appointment.Update(
            doctorId: request.DoctorId,
            serviceId: request.ServiceId,
            officeId: request.OfficeId,
            duration: timeRangeResult.Value!);

        await context.SaveChangesAsync();

        return TypedResults.Ok(
            new UpdateAppointmentResponse(
                Id: appointment.Id,
                PatientId: appointment.PatientId,
                DoctorId: appointment.DoctorId,
                ServiceId: appointment.ServiceId,
                OfficeId: appointment.OfficeId,
                StartDateTime: appointment.Duration.Start,
                EndDateTime: appointment.Duration.End));
    }
}