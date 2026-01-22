using FluentValidation;

namespace Appointment.Api.Features.UpdateAppointment;

public sealed class UpdateAppointmentValidator
        : AbstractValidator<UpdateAppointmentRequest>
    {
        public UpdateAppointmentValidator()
        {
            RuleFor(x => x.PatientId).NotEmpty();
            RuleFor(x => x.DoctorId).NotEmpty();
            RuleFor(x => x.StartDateTime).NotEmpty();
            RuleFor(x => x.EndDateTime).NotEmpty();
        }
    }





