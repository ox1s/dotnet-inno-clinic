using System.Globalization;

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

        var workStartTime = TimeOnly
            .Parse(Appointments_Resourses.Work_StartTime, CultureInfo.InvariantCulture);
        var workEndTime = TimeOnly
            .Parse(Appointments_Resourses.Work_EndTime, CultureInfo.InvariantCulture);

        var targetDate = DateOnly.FromDateTime(request.DateTime.DateTime);
        var targetOffset = request.DateTime.Offset;

        var dayStart = new DateTimeOffset(targetDate.ToDateTime(workStartTime), targetOffset);
        var dayEnd = new DateTimeOffset(targetDate.ToDateTime(workEndTime), targetOffset);

        var availableSlots = new List<DateTimeOffset>();

        var currentSlotStart = dayStart;
        while (currentSlotStart.Add(duration.Value) <= dayEnd)
        {
            var currentSlotEnd = currentSlotStart.Add(duration.Value);

            var slotRangeResult = TimeRange.Create(currentSlotStart, currentSlotEnd);

            if (slotRangeResult.Succeeded)
            {

                var slotRange = slotRangeResult.Value!;

                // FEAT: overlapping appointments | я тут изменила на метод контекста
                if (!await context.IsOverlappingAsync(request.DoctorId, slotRange))
                    availableSlots.Add(currentSlotStart);
            }

            currentSlotStart = currentSlotEnd;
        }

        return Results.Ok(new GetAvailableSlotsResponse(availableSlots));
    }
}