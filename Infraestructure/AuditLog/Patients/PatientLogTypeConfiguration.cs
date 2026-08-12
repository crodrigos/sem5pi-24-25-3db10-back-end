using App.Domain.SystemUser;
using dddnet8.AuditLog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourNamespace.Converters;
using YourNamespace.GDPR.Entities;

namespace dddnet8.Infraestructure.AuditLog.Patients;

public class PatientLogTypeConfiguration : IEntityTypeConfiguration<PatientLog>
{
    public void Configure(EntityTypeBuilder<PatientLog> builder)
    {
        builder.ToTable("PatientLogs");
                
        builder.HasKey(u => u.Id);

        builder.Property(p => p.FirstName).IsRequired().HasConversion(new NameConverter());
                
        builder.Property(p => p.LastName).IsRequired().HasConversion(new NameConverter());
                
        builder.Property(p => p.ContactInformation).IsRequired().HasConversion(new ContactInfoConverter());
                
        builder.Property(p => p.DateOfBirth).IsRequired().HasConversion(new DateOfBirthConverter());
                
        builder.Property(p => p.Gender).IsRequired().HasConversion<string>();
                
        builder.Property(p => p.FullName).IsRequired().HasConversion(new NameConverter());
                
        builder.Property(p => p.MedicalRecordNumber).IsRequired().HasConversion(new MedicalRecordNumberConverter());
                
        builder.Property(p => p.EmergencyContact).IsRequired().HasConversion(new EmergencyContactConverter());

        builder.Property(p => p.Gender).IsRequired().HasConversion<string>();

        builder.Property(p => p.MedicalCondition).HasColumnName("MedicalCondition").HasConversion(new MedicalConditionConverter());
                
        builder.Property(p => p.DeletionStatus).HasColumnName("DeletionStatus").HasConversion(new DeletionStatusConverter());
    }
}