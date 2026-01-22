using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;

using Appointment.Api.Common;
using Appointment.Api.Endpoints;

namespace Appointment.Api.Features.Appointments;

public class UpdateAppointment
{

    public record Request(
        Guid PatientId,
        Guid DoctorId,
        DateTime StartDateTime,
        DateTime EndDateTime);

    public record Response(
        Guid Id,
        Guid PatientId,
        Guid DoctorId,
        DateTime StartDateTime,
        DateTime EndDateTime);

    public sealed class Validator
        : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.PatientId).NotEmpty();
            RuleFor(x => x.DoctorId).NotEmpty();
            RuleFor(x => x.StartDateTime).NotEmpty();
            RuleFor(x => x.EndDateTime).NotEmpty();
        }
    }

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("/appointments/{id}", Handler)
                .WithTags("Appointments");
        }
    }


    public async static Task<Results<Ok<Response>, NotFound>> Handler(Guid id,
        Request request,
        AppointmentDbContext context,
        IValidator<Request> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return (Results<Ok<Response>, NotFound>)Results.BadRequest(new Error(
                Code: "Validation",
                Description: validationResult.ToString()));

        var appointmentToUpdate = await context.Appointments.FindAsync(id);
        if (appointmentToUpdate is null)
            return Result.Failure(new Error(
                Code: "NotFound",
                Description: "Appointment not found"));

        var date = DateOnly.FromDateTime(request.StartDateTime);
        var startTime = TimeOnly.FromDateTime(request.StartDateTime);
        var endTime = TimeOnly.FromDateTime(request.EndDateTime);

        var timeRangeResult = TimeRange.Create(startTime, endTime);

        if (!timeRangeResult.Succeeded)
        {
            return (Results<Ok<Response>, NotFound>)Results.BadRequest(timeRangeResult.Error.Description);
        }

        var appointment = Data.Appointment.Create(
            patientId: request.PatientId,
            doctorId: request.DoctorId,
            date: date,
            time: timeRangeResult.Value!);

        await context.SaveChangesAsync();

        return TypedResults.Ok(
            new Response(
                appointment.Id,
                appointment.PatientId,
                appointment.DoctorId,
                StartDateTime: appointment.Date.ToDateTime(appointment.Time.Start),
                EndDateTime: appointment.Date.ToDateTime(appointment.Time.End)));
    }

}
