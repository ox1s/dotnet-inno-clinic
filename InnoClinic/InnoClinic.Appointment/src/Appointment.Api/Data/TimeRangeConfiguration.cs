using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Appointment.Api.Data;

public class TimeRangeConfiguration : IEntityTypeConfiguration<TimeRange>
{
    public void Configure(EntityTypeBuilder<TimeRange> builder)
    {
        builder.ToTable("time_ranges", "appointment");

        builder.HasKey(a => a.TimeRangeId);
        builder.Property(a => a.TimeRangeId)
            .HasColumnName("time_range_id")
            .ValueGeneratedNever();

        builder.Property(a => a.Start)
            .HasColumnName("start");

        builder.Property(a => a.End)
            .HasColumnName("end");

    }
}
