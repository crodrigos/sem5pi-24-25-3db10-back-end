namespace dddnet8.Domain.AssignedStaff.Interfaces;

public interface ITimeAssignedStaffRepository
{
    Task AddAssignedStaff(global::AssignedStaff assignedStaff);
    Task<List<global::AssignedStaff>> GetStaffByAppointmentId(Guid appointmentId);
}