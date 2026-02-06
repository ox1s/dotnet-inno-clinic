using Appointment.Api.Data;
using Appointment.Api.External;

using Microsoft.EntityFrameworkCore;

namespace Appointment.Api.Features.GetAvailableSlots;

public static class GetAvailableSlotsHandler
{
    public static async Task<IResult> Handle(
        [AsParameters] GetAvailableSlotsRequest request,
        AppointmentDbContext context,
        IServiceGateway serviceGateway)
    {
        var duration = await serviceGateway.GetServiceDurationAsync(request.ServiceId);
        if (duration is null)
            return Results.BadRequest("Invalid Service");

        var workStartTime = new TimeOnly(9, 0);
        var workEndTime = new TimeOnly(18, 0);

        var targetDate = DateOnly.FromDateTime(request.DateTime.DateTime);
        var targetOffset = request.DateTime.Offset;

        var dayStart = new DateTimeOffset(targetDate.ToDateTime(workStartTime), targetOffset);
        var dayEnd = new DateTimeOffset(targetDate.ToDateTime(workEndTime), targetOffset);

        var appointments = await context.Appointments
            .AsNoTracking()
            .Where(a => a.DoctorId == request.DoctorId)
            .Where(a => a.Time.Start < dayEnd && a.Time.End > dayStart)
            .ToListAsync();

        var availableSlots = new List<DateTimeOffset>();

        var currentSlotStart = dayStart;

        while (currentSlotStart.Add(duration.Value) <= dayEnd)
        {
            var currentSlotEnd = currentSlotStart.Add(duration.Value);

            var slotRangeResult = TimeRange.Create(currentSlotStart, currentSlotEnd);

            if (slotRangeResult.Succeeded)
            {
                var slotRange = slotRangeResult.Value!;

                bool isOverlapping = context.TimeRanges
                    .Any(a =>
                        a.Start == slotRange.Start
                        && a.End == slotRange.End);

                if (!isOverlapping)
                    availableSlots.Add(currentSlotStart);
            }

            currentSlotStart = currentSlotEnd;
        }

        return Results.Ok(new GetAvailableSlotsResponse(availableSlots));
    }
}