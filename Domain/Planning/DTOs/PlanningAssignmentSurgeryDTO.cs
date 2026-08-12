namespace dddnet8.Domain.PlanningModuleNotifications.DTOs;

public class PlanningAssignmentSurgeryDTO
{
    /// <summary>
    /// The code of the operation request.
    /// </summary>
    public string OpRequestCode { get; set; }

    /// <summary>
    /// The license number of the assigned staff member or doctor.
    /// </summary>
    public string LicenseNumber { get; set; }

    /// <summary>
    /// Constructor for PlanningAssignmentSurgeryDTO.
    /// </summary>
    /// <param name="opRequestCode">The code of the operation request.</param>
    /// <param name="licenseNumber">The license number of the assigned staff member.</param>
    public PlanningAssignmentSurgeryDTO(string opRequestCode, string licenseNumber)
    {
        OpRequestCode = opRequestCode;
        LicenseNumber = licenseNumber;
    }

    /// <summary>
    /// Parameterless constructor for PlanningAssignmentSurgeryDTO.
    /// </summary>
    public PlanningAssignmentSurgeryDTO()
    {
    }
}
