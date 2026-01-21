using Appointment.Api.Endpoints;

using FluentValidation;

using Microsoft.AspNetCore.Http.HttpResults;

namespace Appointment.Api.Futures.Appointments;

public class UpdateAppointment
{

    public record Request(
        Guid PatientId,
        Guid DoctorId,
        DateOnly Date,
        TimeOnly Time);

    public record Response(
        Guid Id,
        Guid PatientId,
        Guid DoctorId,
        DateOnly Date,
        TimeOnly Time);

    public sealed class Validator
        : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.PatientId).NotEmpty();
            RuleFor(x => x.DoctorId).NotEmpty();
            RuleFor(x => x.Date).NotEmpty();
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


    public async static Task<Results<Ok<Response>, NotFound>> Handler(Guid id, Request request, AppointmentDbContext context, IValidator<Request> validator)
    {
        var appointment = await context.Appointments.FindAsync(id);
        if (appointment is null) return TypedResults.NotFound();

        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return (Results<Ok<Response>, NotFound>) Results.BadRequest(validationResult.Errors);

        appointment.PatientId = request.PatientId;
        appointment.DoctorId = request.DoctorId;
        appointment.Date = request.Date;
        appointment.Time = request.Time;

        await context.SaveChangesAsync();

        return TypedResults.Ok(
            new Response(
                appointment.Id,
                appointment.PatientId,
                appointment.DoctorId,
                appointment.Date,
                appointment.Time));
    }

}
