using ClinicManagement.Api.Data.Entities;

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
    public Photo Photo { get; set; }
    public string RegistryPhoneNumber { get; set; } = null!;
    public bool IsActive { get; set; }

    public static Office Create(
        string address,
        Photo photo,
        string registryPhoneNumber,
        bool isActive)
    {
        return new Office
        {
            Id = Guid.NewGuid(),
            Address = address,
            Photo = photo,
            RegistryPhoneNumber = registryPhoneNumber,
            IsActive = isActive
        };
    }

    public void Update(string address, string registryPhoneNumber, Photo photo, bool isActive)
    {
        Address = address;
        RegistryPhoneNumber = registryPhoneNumber;
        Photo = photo;
        IsActive = isActive;
    }
    private Office() { }

}