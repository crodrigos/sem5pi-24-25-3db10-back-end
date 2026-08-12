using dddnet8.Domain.Shared;
using dddnet8.Domain.Staffs.V.O;

namespace dddnet8.Domain.Timetable;

public interface ITimetableRepository : IRepository<Timetables.Timetable, Guid>
{
    Task AddTimetableAsync(Timetables.Timetable timetable);
    
    Task<List<Timetables.Timetable>> GetAllTimetablesAsync();
    Task<Timetables.Timetable> GetTimetableByDateAndLicenseNumber(LicenseNumber licenseNumber, DateOnly dateToCheck);
    Task<List<Domain.Timetables.Timetable>> GetAllStaffsForDate(DateTime parse);
}