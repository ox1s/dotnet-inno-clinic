namespace Appointment.Api.Features.Receptionist.ListAppointments;

public class AppointmentFilter
{

    public Guid? DoctorId { get; set; }
    public Guid? PatientId { get; set; }
    public Guid? ServiceId { get; set; }
    public Guid? OfficeId { get; set; }
    public DateOnly? Date { get; set; }
    public DateOnly? DateStart { get; set; }
    public DateOnly? DateEnd { get; set; }
    public bool? IsApproved { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public SortOptions SortBy { get; set; }
    public SortDirection SortDirection { get; set; }

    public AppointmentFilter(
        int page,
        int pageSize,
        Guid? doctorId,
        Guid? patientId,
        Guid? serviceId,
        Guid? officeId,
        DateOnly? date,
        DateOnly? dateStart,
        DateOnly? dateEnd,
        bool? isApproved,
        SortOptions sortBy = SortOptions.Date,
        SortDirection sortDirection = SortDirection.Asc)

    {
        DoctorId = doctorId;
        PatientId = patientId;
        ServiceId = serviceId;
        OfficeId = officeId;
        Date = date;
        DateStart = dateStart;
        DateEnd = dateEnd;
        IsApproved = isApproved;
        SortBy = sortBy;
        SortDirection = sortDirection;
        Page = page;
        PageSize = pageSize;
    }

}
public enum SortOptions
{
    Date,
    ServiceName,
    DoctorName,
}
public enum SortDirection
{
    Asc,
    Desc,
}