namespace Appointment.Api.Data;

public class AppointmentFilter
{

    public Guid? DoctorId { get; set; }
    public Guid? PatientId { get; set; }
    public Guid? ServiceId { get; set; }
    public DateOnly? Date { get; set; }
    public DateOnly? DateStart { get; set; }
    public DateOnly? DateEnd { get; set; }
    public bool? IsApproved { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public SortOptions SortBy { get; set; }
    public SortDirection SortDirection { get; set; }

    public AppointmentFilter(
        int Page,
        int PageSize,
        Guid? DoctorId,
        Guid? PatientId,
        Guid? ServiceId,
        DateOnly? Date,
        DateOnly? DateStart,
        DateOnly? DateEnd,
        bool? IsApproved,
        SortOptions SortBy = SortOptions.Date,
        SortDirection SortDirection = SortDirection.Asc)

    {
        this.DoctorId = DoctorId;
        this.PatientId = PatientId;
        this.ServiceId = ServiceId;
        this.Date = Date;
        this.DateStart = DateStart;
        this.DateEnd = DateEnd;
        this.IsApproved = IsApproved;
        this.SortBy = SortBy;
        this.SortDirection = SortDirection;
        this.Page = Page;
        this.PageSize = PageSize;
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