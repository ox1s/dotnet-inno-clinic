namespace ClinicManagement.Api.Data.Entities;

public class ServiceCategory
{
    // id (PK)
    // category_name
    // time_slot_size

    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public int TimeSlotSize { get; set; }

    public static ServiceCategory Create(string name, int timeSlotSize)
    {
        return new ServiceCategory
        {
            // Set explicitly, like Office and Service do, rather than relying on EF's
            // client-side Guid generator.
            Id = Guid.NewGuid(),
            Name = name,
            TimeSlotSize = timeSlotSize,
        };
    }

    public void Update(string name, int timeSlotSize)
    {
        Name = name;
        TimeSlotSize = timeSlotSize;
    }
}
