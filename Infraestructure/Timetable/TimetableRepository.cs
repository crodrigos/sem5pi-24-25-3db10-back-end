using dddnet8.Domain.Staffs.V.O;
using dddnet8.Domain.Timetable;
using dddnet8.Infraestructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace dddnet8.Infraestructure.Timetable;

public class TimetableRepository : BaseRepository<Domain.Timetables.Timetable, Guid>, ITimetableRepository
{

    private ApplicationDbContext _context;

    public TimetableRepository(ApplicationDbContext context) : base(context.Timetable)
    {
        _context = context;
    }

    public async Task AddTimetableAsync(Domain.Timetables.Timetable timetable)
    {
        await  _context.Timetable.AddAsync(timetable);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Domain.Timetables.Timetable>> GetAllTimetablesAsync()
    {
        return await _context.Timetable.ToListAsync();
    }

    public Task<Domain.Timetables.Timetable> GetByLicenseNumberAsync(LicenseNumber timeSlotLicenseNumber)
    {
        return _context.Timetable.FirstOrDefaultAsync(t => t.LicenseNumber == timeSlotLicenseNumber);
    }

    public async Task<Domain.Timetables.Timetable> GetTimetableByDateAndLicenseNumber(LicenseNumber licenseNumber, DateOnly dateToCheck)
    {
        var staffTimetables = await _context.Timetable
                .Where(t => t.LicenseNumber == licenseNumber)
                .ToListAsync();

        var timetableForDate = staffTimetables
                .FirstOrDefault(t => DateOnly.FromDateTime(t.DateShift) == dateToCheck);

            if (timetableForDate == null) {return null;}

            return timetableForDate;
        }

    public async Task<List<Domain.Timetables.Timetable>> GetAllStaffsForDate(DateTime parse)
    {
        return await _context.Timetable.Where(d => d.DateShift == parse).ToListAsync();
    }
}