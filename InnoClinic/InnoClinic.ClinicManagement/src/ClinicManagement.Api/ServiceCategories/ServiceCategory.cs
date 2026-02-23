namespace ClinicManagement.Api.ServiceCategories;

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
            Name = name,
            TimeSlotSize = timeSlotSize,
        };
    }
}
