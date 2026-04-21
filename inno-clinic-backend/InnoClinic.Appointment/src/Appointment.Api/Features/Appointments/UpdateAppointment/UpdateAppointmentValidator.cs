using FluentValidation;

namespace Appointment.Api.Features.Appointments.UpdateAppointment;

public sealed class UpdateAppointmentValidator : AbstractValidator<UpdateAppointmentRequest>
{
    public UpdateAppointmentValidator()
    {
        RuleFor(x => x.DoctorId).NotEmpty();
        RuleFor(x => x.ServiceId).NotEmpty();
        RuleFor(x => x.OfficeId).NotEmpty();
        RuleFor(x => x.StartDateTime).NotEmpty();
        RuleFor(x => x.EndDateTime).NotEmpty()
            .GreaterThan(x => x.StartDateTime);
    }
}