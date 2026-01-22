using FluentValidation;

using Appointment.Api.Common;
using Appointment.Api.Endpoints;

namespace Appointment.Api.Features.Appointments;

public static class CreateAppointment
{
    public record Request(
        Guid PatientId,
        Guid DoctorId,
        DateTime StartDateTime,
        DateTime EndDateTime);

    private record Response(
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
            app.MapPost("/appointments", Handler)
                .WithTags("Appointments");
        }
    }

    public async static Task<IResult> Handler(
        Request request,
        AppointmentDbContext dbContext,
        IValidator<Request> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Results.BadRequest(new Error(
                Code: "Validation",
                Description: validationResult.ToString()));

        var date = DateOnly.FromDateTime(request.StartDateTime);
        var startTime = TimeOnly.FromDateTime(request.StartDateTime);
        var endTime = TimeOnly.FromDateTime(request.EndDateTime);

        var timeRangeResult = TimeRange.Create(startTime, endTime);

        if (!timeRangeResult.Succeeded)
        {
            return Results.BadRequest(timeRangeResult.Error.Description);
        }

        var appointment = Data.Appointment.Create(
            patientId: request.PatientId,
            doctorId: request.DoctorId,
            date: date,
            time: timeRangeResult.Value!
            );

        dbContext.Appointments.Add(appointment);

        await dbContext.SaveChangesAsync();

        return Results.Ok(
            new Response(
                Id: appointment.Id,
                PatientId: appointment.PatientId,
                DoctorId: appointment.DoctorId,
                StartDateTime: appointment.Date.ToDateTime(appointment.Time.Start),
                EndDateTime: appointment.Date.ToDateTime(appointment.Time.End)));
    }
}
