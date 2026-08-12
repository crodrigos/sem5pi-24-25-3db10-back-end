using dddnet8.Domain.Shared;
using dddnet8.Domain.Staffs.V.O;

namespace dddnet8.Domain.Staffs.Interfaces;

/// <summary>
///     Interface for repository operations related to TimeSlot entities.
/// </summary>
public interface ITimeSlotRepository : IRepository<TimeSlot, Guid>
{
    /// <summary>
    ///     Retrieves a TimeSlot by the staff member's license number.
    /// </summary>
    /// <param name="licenseNumber">The license number of the staff member.</param>
    /// <returns>The TimeSlot associated with the specified license number.</returns>
    Task<TimeSlot> GetByLicenseNumberAsync(LicenseNumber licenseNumber);

    Task<List<TimeSlot>> GetTimeSlotByLicenseNumberAndDate(DateOnly fromDateTime, LicenseNumber licenseNumber);
    Task AddTimeSlot(TimeSlot timeslot);
    Task<List<TimeSlot>> GetStaffAllTimeSlots(LicenseNumber sLicenseNumber);
    Task<TimeSlot> GetTimeSlotByLicenseNumberAndDateAndTime(DateOnly date, LicenseNumber licenseNumber, TimeShift shift);
}