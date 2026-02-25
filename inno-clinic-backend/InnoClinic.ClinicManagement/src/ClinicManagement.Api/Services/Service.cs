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
    public Price Price { get; set; }
    public Guid SpecializationId { get; set; }
    public bool IsActive { get; set; }

    public static Service Create(
        Guid categoryId,
        string serviceName,
        Price price,
        Guid specializationId,
        bool isActive,
        Guid? id = null)
    {
        return new Service
        {
            Id = id ?? Guid.NewGuid(),
            CategoryId = categoryId,
            ServiceName = serviceName,
            Price = price,
            SpecializationId = specializationId,
            IsActive = isActive
        };
    }
    public void ChangeStatus(bool isActive)
    {
        IsActive = isActive;
    }

    public void Update(
        Guid categoryId,
        string serviceName,
        Price price,
        Guid specializationId,
        bool isActive)
    {
        CategoryId = categoryId;
        ServiceName = serviceName;
        Price = price;
        SpecializationId = specializationId;
        IsActive = isActive;
    }

    private Service() { }
}
