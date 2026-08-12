namespace dddnet8.Domain.PlanningModuleNotifications.DTOs;

public class PlanningStaffDTO
{
    /// <summary>
    /// The license number of the staff member or doctor.
    /// </summary>
    public string LicenseNumber { get; set; }

    /// <summary>
    /// The type of staff member (e.g., "Doctor", "Nurse").
    /// </summary>
    public string StaffType { get; set; }

    /// <summary>
    /// The specialization of the staff member.
    /// </summary>
    public string Specialization { get; set; }

    /// <summary>
    /// The list of operation type codes the staff member is qualified for.
    /// </summary>
    public List<string> OperationTypeCodes { get; set; }

    /// <summary>
    /// Constructor for PlanningStaffDTO.
    /// </summary>
    /// <param name="licenseNumber">The license number of the staff member.</param>
    /// <param name="staffType">The type of staff member.</param>
    /// <param name="specialization">The specialization of the staff member.</param>
    /// <param name="operationTypeCodes">The list of operation type codes.</param>
    public PlanningStaffDTO(string licenseNumber, string staffType, string specialization, List<string> operationTypeCodes)
    {
        LicenseNumber = licenseNumber;
        StaffType = staffType;
        Specialization = specialization;
        OperationTypeCodes = operationTypeCodes;
    }

    /// <summary>
    /// Parameterless constructor for PlanningStaffDTO.
    /// </summary>
    public PlanningStaffDTO()
    {
        OperationTypeCodes = new List<string>();
    }
}
