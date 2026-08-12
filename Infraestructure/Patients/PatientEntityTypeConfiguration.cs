using App.Domain.SystemUser;
using dddnet8.Domain.Patients.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourNamespace.Converters;

namespace dddnet8.Infraestructure.Patients;

public class PatientEntityTypeConfiguration : IEntityTypeConfiguration<PatientDataModel>
{
    // TODO -> CRIAR FOLDER DATA MODEL E CRIAR PATIENTDATAMODEL E SUBSTITUIR PATIENT POR PATIENTDATAMODEL
    public void Configure(EntityTypeBuilder<PatientDataModel> builder)
    {
                builder.ToTable("Patients");
                
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

                builder.Property(p => p.DeletionStatus).HasColumnName("DeletionStatus").HasConversion(new DeletionStatusConverter());

                builder.HasIndex(p => p.MedicalRecordNumber).IsUnique();
                
                builder.HasIndex(p => p.ContactInformation).IsUnique();
    }
}