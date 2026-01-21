using Appointment.Api.Endpoints;

using FluentValidation;

namespace Appointment.Api.Futures.Appointments;

public class CreateAppointment
{
    public record Request(
        Guid PatientId,
        Guid DoctorId,
        DateTime StartDateTime,
        DateTime EndDateTime);

    private record Response(Guid Id,
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

        var appointment = Appointment.Create(
            patientId: request.PatientId,
            doctorId: request.DoctorId,
            date: request.Date,
            time: request.Time);

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
