namespace ClinicManagement.Api.Services;

public class Service
{
    // id (PK)
    // category_id (FK)
    // service_name
    // price
    // specialization_id
    // isActive

    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public string ServiceName { get; set; } = null!;
    public decimal Price { get; set; }
    public Guid SpecializationId { get; set; }
    public bool IsActive { get; set; }
}
