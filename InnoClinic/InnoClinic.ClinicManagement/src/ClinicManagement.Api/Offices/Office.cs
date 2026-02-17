namespace ClinicManagement.Api.Offices;


public class Office
{
    // id (PK)
    // address
    // photo_id (FK)
    // registry_phone_number
    // isActive

    public Guid Id { get; set; }
    public string Address { get; set; } = null!;
    public Guid? PhotoId { get; set; }
    public string RegistryPhoneNumber { get; set; } = null!;
    public bool IsActive { get; set; }

    public static Office Create(
        string address,
        Guid? photoId,
        string registryPhoneNumber,
        bool isActive)
    {
        return new Office
        {
            Id = Guid.NewGuid(),
            Address = address,
            PhotoId = photoId,
            RegistryPhoneNumber = registryPhoneNumber,
            IsActive = isActive
        };
    }

    public void Update(string address, string registryPhoneNumber, Guid? photoId, bool isActive)
    {
        Address = address;
        RegistryPhoneNumber = registryPhoneNumber;
        PhotoId = photoId;
        IsActive = isActive;
    }
    private Office() { }

}