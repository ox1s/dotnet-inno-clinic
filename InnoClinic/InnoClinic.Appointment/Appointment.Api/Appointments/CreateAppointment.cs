using Appointment.Api.Endpoints;

using FluentValidation;

namespace Appointment.Api.Appointments;

public class CreateAppointment
{
    public record Request(
        Guid PatientId,
        Guid DoctorId,
        DateOnly Date,
        TimeOnly Time);

    public record Response(Guid Id,
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
            RuleFor(x => x.Time).NotEmpty();
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
            return Results.BadRequest(validationResult.Errors);

        var appointment = new Appointment
        {
            Id = Guid.NewGuid(),
            PatientId = request.PatientId,
            DoctorId = request.DoctorId,
            Date = request.Date,
            Time = request.Time,
            IsApproved = false,
        };

        dbContext.Appointments.Add(appointment);

        await dbContext.SaveChangesAsync();

        return Results.Ok(
            new Response(
                appointment.Id,
                appointment.PatientId,
                appointment.DoctorId,
                appointment.Date,
                appointment.Time));
    }
}
