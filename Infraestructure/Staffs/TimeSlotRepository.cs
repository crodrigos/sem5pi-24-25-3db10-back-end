using dddnet8.Domain.Staffs;
using dddnet8.Domain.Staffs.Interfaces;
using dddnet8.Domain.Staffs.V.O;
using dddnet8.Infraestructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace dddnet8.Infraestructure.Staff;

public class TimeSlotRepository : BaseRepository<TimeSlot, Guid>, ITimeSlotRepository
{
    private ApplicationDbContext _context;

    public TimeSlotRepository(ApplicationDbContext context) : base(context.TimeSlot)
    {
        _context = context;
    }

    /// <summary>
    ///     Retrieves a TimeSlot by its LicenseNumber.
    /// </summary>
    /// <param name="licenseNumber">The license number associated with the time slot.</param>
    /// <returns>The matching TimeSlot or null if not found.</returns>
    /// <exception cref="ArgumentNullException">Thrown if licenseNumber is null.</exception>
    public async Task<TimeSlot> GetByLicenseNumberAsync(LicenseNumber licenseNumber)
    {
        if (licenseNumber == null)
            throw new ArgumentNullException(nameof(licenseNumber));

        return await _context.TimeSlot
            .FirstOrDefaultAsync(s => s.LicenseNumber == licenseNumber);
    }

    public async Task<List<TimeSlot>> GetTimeSlotByLicenseNumberAndDate(DateOnly fromDateTime, LicenseNumber licenseNumber)
    {
        var staffTimeSlots = await _context.TimeSlot
            .Where(t => t.LicenseNumber == licenseNumber && t.Date == fromDateTime)
            .ToListAsync();

        return staffTimeSlots;
    }

    public async Task AddTimeSlot(TimeSlot timeslot)
    {
        await _context.TimeSlot.AddAsync(timeslot);
        await _context.SaveChangesAsync();

    }

    public async Task<List<TimeSlot>> GetStaffAllTimeSlots(LicenseNumber sLicenseNumber)
    {
        return await _context.TimeSlot.Where(t => t.LicenseNumber == sLicenseNumber).ToListAsync();
    }

    public async Task<TimeSlot> GetTimeSlotByLicenseNumberAndDateAndTime(DateOnly date, LicenseNumber licenseNumber, TimeShift shift)
    {
        return await _context.TimeSlot.Where(t => t.LicenseNumber == licenseNumber && t.Date == date && t.TimeShift == shift).FirstAsync();
    }
}