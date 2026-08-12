using dddnet8.Domain.OperationTypes;
using dddnet8.Domain.OperationTypes.Names;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace dddnet8.Infraestructure.OperationTypes;

public class OperationTypeEntityConfiguration : IEntityTypeConfiguration<OperationType>
{
    public void Configure(EntityTypeBuilder<OperationType> builder)
    {
        builder.ToTable("OperationTypes");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Status).IsRequired().HasConversion<string>();

        builder.Property(x => x.EstimatedDuration).IsRequired()
            .HasConversion(
                v => v.ToString(), // Serializa EstimatedDuration para double (em minutos)
                v => EstimatedDuration.FromString(v));
        
        builder.Property(p => p.Name).IsRequired().HasConversion(
            v => v.Value, // Converte o Value Object para string
            v => new Name(v) // Converte string de volta para Value Object
        );
        
        builder.Property(p => p.OperationTypeCode).IsRequired().HasConversion(
            v => v._OperationTypeCode, // Converte o Value Object para string
            v => OperationTypeCode.Create(v)
            );
           
            
        
    }
}