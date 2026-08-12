using dddnet8.AuditLog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dddnet8.Infraestructure.AuditLog.OperationRequests;

public class OperationRequestLogTypeConfiguration : IEntityTypeConfiguration<OperationRequestLog>
{
    public void Configure(EntityTypeBuilder<OperationRequestLog> builder)
    {
        builder.ToTable("OperationRequestLog");

        // Primary Key
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        // Properties
        builder.Property(x => x.OperationRequestId).IsRequired();
        builder.Property(x => x.PatientId).IsRequired();
        builder.Property(x => x.DoctorId).IsRequired();
        builder.Property(x => x.OperationTypeId).IsRequired();
        builder.Property(x => x.DeadlineDate).IsRequired();
        builder.Property(x => x.Priority).HasConversion<string>().IsRequired();
        builder.Property(x => x.OperationDescription).IsRequired();
        builder.Property(x => x.Status).HasConversion<string>().IsRequired();
        builder.Property(x => x.CreatedDate).IsRequired();
        builder.Property(x => x.LastUpdatedDate).IsRequired();
        builder.Property(x => x.IsScheduled).IsRequired();
        
        // Indexes
        builder.HasIndex(x => x.PatientId);
        builder.HasIndex(x => x.DoctorId);
    }
}