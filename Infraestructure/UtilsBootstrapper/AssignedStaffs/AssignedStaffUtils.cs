using dddnet8.Domain.Appointments.Interfaces;
using dddnet8.Domain.AssignedStaff.Interfaces;
using dddnet8.Domain.Staffs.V.O;
using dddnet8.Infraestructure.UtilsBootstrapper.Appointments;
using dddnet8.Infraestructure.UtilsBootstrapper.Staffs;

namespace dddnet8.Infraestructure.UtilsBootstrapper.AssignedStaffs;

public class AssignedStaffUtils
{
    private  readonly ITimeAssignedStaffRepository _timeAssignedStaffRepository;
    private readonly StaffUtils _staffUtils;
    private IAppointmentRepository _appointmentRepository;

    public AssignedStaffUtils(ITimeAssignedStaffRepository timeAssignedStaffRepository,
        StaffUtils staffUtils, IAppointmentRepository appointmentRepository)
    {
        _timeAssignedStaffRepository = timeAssignedStaffRepository;
        _staffUtils = staffUtils;
        _appointmentRepository = appointmentRepository;
    }


    public async Task InitializeAssignedStaffAsync()
    {

        await SaveAssignedStaff(await CreateAssignedStaff(new LicenseNumber("D9769")));
        await SaveAssignedStaff(await CreateAssignedStaff(new LicenseNumber("T1756")));
        await SaveAssignedStaff(await CreateAssignedStaff(new LicenseNumber("N1238")));

    }

    private async Task SaveAssignedStaff(AssignedStaff createAssignedStaff)
    {
        await _timeAssignedStaffRepository.AddAssignedStaff(createAssignedStaff);
    }

    private async Task<AssignedStaff> CreateAssignedStaff(LicenseNumber s1)
    {
        var appointment = await _appointmentRepository.GetAllAsync();

        var firstAppointment = appointment[0];

        return new AssignedStaff(firstAppointment.Id, s1);

    }
}