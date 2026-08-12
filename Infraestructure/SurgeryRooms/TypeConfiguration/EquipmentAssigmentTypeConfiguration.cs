using dddnet8.Domain.SurgeryRooms;
using dddnet8.Domain.SurgeryRooms.Converter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace dddnet8.Infraestructure.SurgeryRooms.TypeConfiguration;

public class EquipmentAssigmentTypeConfiguration : IEntityTypeConfiguration<EquipmentAssignment>
{
    public void Configure(EntityTypeBuilder<EquipmentAssignment> builder)
    {
        builder.HasOne<SurgeryRoom>()  
            .WithMany()  
            .HasForeignKey(ea => ea.SurgeryRoomId) 
            .OnDelete(DeleteBehavior.Restrict);  
        
        builder.Property(s => s.SurgeryRoomRumber)
            .HasConversion(new RoomNumberConverter()) 
            .IsRequired();
        
        builder.Property(ea => ea.EquipmentName)
            .IsRequired();
    }
}