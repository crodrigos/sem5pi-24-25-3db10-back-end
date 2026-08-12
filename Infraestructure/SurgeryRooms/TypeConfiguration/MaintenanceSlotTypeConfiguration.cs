using dddnet8.Domain.SurgeryRooms;
using dddnet8.Domain.SurgeryRooms.Converter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dddnet8.Infraestructure.SurgeryRooms.TypeConfiguration;

public class MaintenanceSlotTypeConfiguration : IEntityTypeConfiguration<MaintenanceSlot>{
    public void Configure(EntityTypeBuilder<MaintenanceSlot> builder)
    {
        builder.HasKey(ms => ms.Id);

        // TODO -> INVESTIGAR O QUE SE PASSA AQUI
        //builder.HasOne<SurgeryRoom>()
        //    .WithMany()  // Defina o relacionamento inverso corretamente se necessário
        //    .HasForeignKey(ms => ms.Id)  // Alterado para 'SurgeryRoomId'
        //    .OnDelete(DeleteBehavior.Restrict);
        
        builder.Property(s => s.SurgeryRoomNumber)
            .HasConversion(new RoomNumberConverter()) 
            .IsRequired();

        
        builder.Property(ms => ms.Date)
            .IsRequired(); 


        builder.Property(ms => ms.StartTime)
            .IsRequired(); 


        builder.Property(ms => ms.EndTime)
            .IsRequired(); 
    }
}