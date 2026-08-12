using dddnet8.Domain.OperationRequests.Converter;
using dddnet8.Domain.Specializations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourNamespace.Converters;

namespace dddnet8.Infraestructure.Specializations;

public class SpecializationEntityTypeConfiguration : IEntityTypeConfiguration<Specialization>
{
    public void Configure(EntityTypeBuilder<Specialization> builder)
    {
        builder.ToTable("Specialization");
        
        builder.HasKey(u => u.Id);
        
        builder.Property(p => p.Name)
            .IsRequired()
            .HasConversion(new NameConverter());

        builder.Property(p => p.Description).IsRequired().HasConversion(new DescriptionConverter());

        builder.Property(p => p.Code).IsRequired().HasConversion(new SpecializationCodeConverter());
        
        builder.HasIndex(p => p.Name).IsUnique();
        
        builder.HasIndex(p => p.Code).IsUnique();

        
        builder.HasIndex(p => p.Description).IsUnique();
    }
}