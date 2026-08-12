using dddnet8.Domain.Staffs.V.O;

namespace dddnet8.Domain.Staffs.Interfaces;

public interface ITimeSlotService
{
    Task<List<TimeSlot>> GetAllTimeSlots();


    Task<bool> CheckIfStaffTimetableIsAvailable(DateTime appointmentDate, string surgeryStartTime, EstimatedDuration estimatedDuration, string teamLicenseNumber);
    Task<bool> CheckIfStaffIsAvailable2(DateTime dateTime, string licenseNumber);
    Task<bool> CheckIfStaffTimeSlotIsAvailable(DateTime appointmentDate, string surgeryStartTime, EstimatedDuration estimatedDuration, string teamLicenseNumber);
    Task SaveTimeSlot(TimeSlot timeSlot);
    Task<List<TimeSlot>> GetTimeSlotByLicenseNumberAndDate(DateOnly date, LicenseNumber licenseNumber);
}