using dddnet8.Domain.SurgeryRooms;
using dddnet8.Domain.SurgeryRooms.Interfaces;
using dddnet8.Domain.SurgeryRooms.V.O;
using dddnet8.Infraestructure.UtilsBootstrapper.SurgeryRooms;

namespace dddnet8.Infraestructure.UtilsBootstrapper.MaintanceSlots;

public class MaintenanceSlotsUtils
{
    private IMaintenanceSlotRepository _maintenanceSlotRepository;
    private SurgeryRoomsUtils _surgeryRoomsUtils;

    public MaintenanceSlotsUtils(IMaintenanceSlotRepository maintenanceSlotRepository, SurgeryRoomsUtils surgeryRoomsUtils)
    {
        _maintenanceSlotRepository = maintenanceSlotRepository;
        _surgeryRoomsUtils = surgeryRoomsUtils;
    }
    
    public async Task InitializeMaintenanceSlotsAsync(){
        var slots = await _maintenanceSlotRepository.GetAllAsync();

        if (!slots.Any())
        {
            await SaveMaintenanceSlot(await CreateMaintenanceSlot(
                await _surgeryRoomsUtils.GetSurgeryRoom("R0003"),
                new DateTime(2024, 11, 26, 0, 0, 0, DateTimeKind.Utc),  // Data do slot em UTC
                new TimeSpan(8, 0, 0),  // Início às 08:00:00
                new TimeSpan(10, 0, 0)  // Término às 10:00:00
                ));
        } 
    }

    private async Task<MaintenanceSlot> CreateMaintenanceSlot(SurgeryRoom? surgeryRoomNumber, DateTime dateTime, TimeSpan timeSpan, TimeSpan timeSpan1)
    {
      return new MaintenanceSlot(surgeryRoomNumber!.RoomNumber, dateTime, timeSpan, timeSpan1);
    }

    private async Task SaveMaintenanceSlot(MaintenanceSlot createMaintenanceSlot)
    {
       await _maintenanceSlotRepository.Add(createMaintenanceSlot);
    }


    public async Task CreateMaintenanceSlotForAppointment(DateOnly appointmentAppointmentDate, RoomNumber appointmentSurgeryRoom, TimeSpan timeSpan, TimeSpan timeSpan1)
    {
        var surgeryRoom = await _surgeryRoomsUtils.GetSurgeryRoom(appointmentSurgeryRoom.Value);
        
        var maitenance = await CreateMaintenanceSlot(surgeryRoom, appointmentAppointmentDate.ToDateTime(new TimeOnly(0, 0)), timeSpan, timeSpan1);

        await SaveMaintenanceSlot(maitenance);

    }
}