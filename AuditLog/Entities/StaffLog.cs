using dddnet8.Domain.Patients.V.O;
using dddnet8.Domain.Specializations;
using dddnet8.Domain.Staffs.V.O;
using SurgicalManagement.Domain.Common;
using YourNamespace.GDPR.Entities;

namespace dddnet8.AuditLog.Entities;

public class StaffLog : LogEntry
{

    protected StaffLog() : base("action", "entitytype") {}
    public StaffLog(string action, Name firstName, Name lastName, LicenseNumber licenseNumber, Specialization specialization, ContactInfo contactInfo, DeletionStatus deletionStatus) : base(action, "Staff")
    {
        FirstName = firstName;
        LastName = lastName;
        FullName = Name.Create($"{firstName} {lastName}"); 
        LicenseNumber = licenseNumber;
        Specialization = specialization;
        ContactInfo = contactInfo;
        DeletionStatus = deletionStatus; 
    }

    public Name FirstName { get; private set; }

    /// <summary>Gets the last name of the staff member.</summary>
    public Name LastName { get; private set; }

    /// <summary>Gets the full name of the staff member.</summary>
    public Name FullName { get; private set; }

    /// <summary>Gets the license number of the staff member.</summary>
    public LicenseNumber LicenseNumber { get; }

    /// <summary>Gets the specialization of the staff member.</summary>
    public Specialization Specialization { get; private set; }

    /// <summary>Gets the contact information of the staff member.</summary>
    public ContactInfo ContactInfo { get; }
    
    
    /// <summary>Gets the deletion status of the staff member.</summary>
    public DeletionStatus DeletionStatus { get; private set; }
    
   
}