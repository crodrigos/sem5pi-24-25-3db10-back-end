using dddnet8.Domain.Staffs.Interfaces;
using dddnet8.Domain.Staffs.V.O;
using dddnet8.Domain.Timetable;

namespace dddnet8.Infraestructure.UtilsBootstrapper.Timetables;

public class TimetableUtils
{
    private readonly ITimetableRepository _timetableRepository;
    private readonly IStaffRepository _staffRepository;

    public TimetableUtils(ITimetableRepository timetableRepository, IStaffRepository staffRepository)
    {
        _timetableRepository = timetableRepository;
        _staffRepository = staffRepository;
    }


    public async Task CreateAndSaveTimetableForStaff(LicenseNumber licenseNumber, TimeSpan entranceTime, TimeSpan exitTime)
    {

        DateTime startDate =
            new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 8); // Definindo o dia 8 do mês atual
        DateTime endDate =
            new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 20); // Definindo o dia 20 do mês atual

        for (DateTime shiftDate = startDate; shiftDate <= endDate; shiftDate = shiftDate.AddDays(1))
        {
            TimeShift timeShift = new TimeShift(entranceTime, exitTime);

            Domain.Timetables.Timetable timetable = new Domain.Timetables.Timetable(
                Guid.NewGuid(),
                licenseNumber,
                shiftDate,
                timeShift
            );

            await SaveTimetableForStaff(timetable);
        }
    }
    private async Task SaveTimetableForStaff(Domain.Timetables.Timetable timetable)
    {
        await _timetableRepository.AddTimetableAsync(timetable);
    }
}