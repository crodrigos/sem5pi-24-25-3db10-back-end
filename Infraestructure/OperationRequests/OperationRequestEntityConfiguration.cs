using dddnet8.Domain.OperationRequests;
using dddnet8.Domain.OperationRequests.Converter;
using dddnet8.Domain.OperationTypes;
using dddnet8.Domain.Staffs.Converter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourNamespace.Converters;

namespace dddnet8.Infraestructure.OperationRequests;

public class OperationRequestEntityConfiguration : IEntityTypeConfiguration<OperationRequest>
{
    public void Configure(EntityTypeBuilder<OperationRequest> builder)
    {
        builder.ToTable("OperationRequest");
        
        builder.HasKey(x => x.Id);
        builder.Property(e => e.PatientId).IsRequired().HasConversion(new MedicalRecordNumberConverter());
        builder.Property(e => e.DoctorId).IsRequired().HasConversion(new LicenseNumberConverter());
        builder.Property(e => e.OperationTypeId).IsRequired().HasConversion(
            v => v._OperationTypeCode, // Converte o Value Object para string
            v => OperationTypeCode.Create(v));
        builder.Property(e => e.DeadlineDate).IsRequired();
        builder.Property(e => e.Priority).IsRequired().HasConversion<string>();
        builder.Property(e => e.Status).IsRequired().HasConversion<string>();
        builder.Property(e => e.OperationDescription).IsRequired().HasConversion(new DescriptionConverter());
        builder.Property(e => e.CreatedDate).IsRequired();
        builder.Property(e => e.LastUpdatedDate).IsRequired();
        builder.Property(e => e.OperationRequestCode).HasConversion(
            v => v._operationRequestCode, // Converte o Value Object para string
            v => OperationRequestCode.Create(v));
    }
}