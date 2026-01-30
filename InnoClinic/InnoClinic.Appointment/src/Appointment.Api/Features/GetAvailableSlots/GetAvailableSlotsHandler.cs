using Microsoft.EntityFrameworkCore;
using Appointment.Api.Data;
using Appointment.Api.External;

namespace Appointment.Api.Features.GetAvailableSlots;

public static class GetAvailableSlotsHandler
{
    public async static Task<IResult> Handle(
        [AsParameters] GetAvailableSlotsRequest request,
        AppointmentDbContext context,
        IServiceGateway serviceGateway)
    {
        var duration = await serviceGateway.GetServiceDurationAsync(request.ServiceId);
        if (duration is null)
            return Results.BadRequest("Invalid Service");

        var workStart = new TimeOnly(9, 0);
        var workEnd = new TimeOnly(18, 0);

        var appointments = await context.Appointments
            .AsNoTracking()
            .Where(a => a.DoctorId == request.DoctorId && a.Date == request.Date)
            .ToListAsync();

        var availableSlots = new List<TimeOnly>();
        var currentSlotStart = workStart;

        while (currentSlotStart.Add(duration.Value) <= workEnd)
        {
            var currentSlotEnd = currentSlotStart.Add(duration.Value);

            var slotRangeResult = TimeRange.Create(currentSlotStart, currentSlotEnd);

            if (slotRangeResult.Succeeded)
            {
                var slotRange = slotRangeResult.Value!;

                bool isOverlapping = appointments.Any(a => a.Time.Overlaps(slotRange));

                if (!isOverlapping)
                    availableSlots.Add(currentSlotStart);
            }

            currentSlotStart = currentSlotEnd;
        }

        return Results.Ok(new GetAvailableSlotsResponse(availableSlots));
    }
}
