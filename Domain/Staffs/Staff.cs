using dddnet8.Domain.OperationTypes;
using dddnet8.Domain.Patients.V.O;
using dddnet8.Domain.Shared;
using dddnet8.Domain.Specializations;
using dddnet8.Domain.Staffs.DTO;
using dddnet8.Domain.Staffs.V.O;
using SurgicalManagement.Domain.Common;
using Name = dddnet8.Domain.Patients.V.O.Name;

namespace dddnet8.Domain.Staffs;

/// <summary>
///     Represents a staff member entity.
/// </summary>
public class Staff : Entity<Guid>, IAggregateRoot
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Staff" /> class.
    /// </summary>
    /// <param name="firstName">The first name of the staff member.</param>
    /// <param name="lastName">The last name of the staff member.</param>
    /// <param name="specialization">The specialization of the staff member.</param>
    /// <param name="contactInfo">The contact information of the staff member.</param>
    /// <param name="licenseNumber">The license number of the staff member.</param>
    /// <exception cref="ArgumentNullException">Thrown when any required parameter is null.</exception>
    public Staff(Name firstName, Name lastName, Specialization specialization, ContactInfo contactInfo,
        LicenseNumber licenseNumber)
        : base(Guid.NewGuid())
    {
        ValidateParameters(firstName, lastName, specialization, contactInfo, licenseNumber);

        FirstName = firstName;
        LastName = lastName;
        FullName = Name.Create($"{firstName} {lastName}");
        Specialization = specialization;
        ContactInfo = contactInfo;
        LicenseNumber = licenseNumber;
        DeletionStatus = DeletionStatus.Create(false); // Initialized without deletion
        CreatedOn = DateTime.UtcNow; 
    }

    /// <summary>
    ///     Parameterless constructor for ORM purposes.
    /// </summary>
    protected Staff() : base(Guid.NewGuid())
    {
    }

    /// <summary>Gets the first name of the staff member.</summary>
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
    public DateTime CreatedOn { get; private set; }

    /// <summary>
    ///     Updates the staff member's specialization.
    /// </summary>
    /// <param name="newSpecialization">New specialization to be assigned.</param>
    /// <exception cref="ArgumentException">Thrown when the provided specialization is invalid.</exception>
    public void UpdateSpecialization(Specialization newSpecialization)
    {
        if (newSpecialization == null)
            throw new ArgumentException("Specialization cannot be null.", nameof(newSpecialization));

        Specialization = newSpecialization;
    }

    /// <summary>
    ///     Updates the staff member's information based on the provided criteria.
    /// </summary>
    /// <param name="staffCriteria">The criteria containing updated staff information.</param>
    public void UpdateStaff(StaffCriteria staffCriteria){
        if (staffCriteria.FirstName != null) 
            UpdateFirstName(Name.Create(staffCriteria.FirstName));

        if (staffCriteria.LastName != null) 
            UpdateLastName(Name.Create(staffCriteria.LastName));

        if (staffCriteria.ContactInformation != null)
            ContactInfo.UpdateContactInformation(staffCriteria.ContactInformation);
    }
            

    /// <summary>
    ///     Marks the staff member for deletion.
    /// </summary>
    public void MarkForDeletion()
    {
        DeletionStatus = DeletionStatus.Create(true, DateTime.UtcNow);
    }

    /// <summary>
    ///     Determines if the staff member can be deleted.
    /// </summary>
    /// <returns>True if the staff member can be deleted; otherwise, false.</returns>
    public bool CanDelete()
    {
        return DeletionStatus.CanDelete();
    }

    /// <summary>
    ///     Determines if the staff member has the required specialization for a given operation type.
    /// </summary>
    /// <param name="operationType">The operation type to check against.</param>
    /// <returns>True if the staff member has the required specialization, otherwise false.</returns>
    public bool HasSpecializationForOperationType(OperationType operationType)
    {
        // TODO -> TENS QUE IR VER DO REQUIRED STAFF AP
        return Specialization.Name == operationType.SpecializationRequired.Name;
    }

    /// <summary>
    ///     Validates the required parameters for the staff constructor.
    /// </summary>
    /// <param name="parameters">The parameters to validate.</param>
    /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
    private static void ValidateParameters(params object[] parameters)
    {
        foreach (var param in parameters)
            if (param == null)
                throw new ArgumentNullException(nameof(param), "All parameters must be provided and cannot be null.");
    }

    /// <summary>
    ///     Updates the first name and refreshes the full name.
    /// </summary>
    /// <param name="newFirstName">New first name.</param>
    private void UpdateFirstName(Name newFirstName)
    {
        FirstName = newFirstName;
        UpdateFullName();
    }

    /// <summary>
    ///     Updates the last name and refreshes the full name.
    /// </summary>
    /// <param name="newLastName">New last name.</param>
    private void UpdateLastName(Name newLastName)
    {
        LastName = newLastName;
        UpdateFullName();
    }

    /// <summary>
    ///     Updates the full name of the staff member.
    /// </summary>
    private void UpdateFullName()
    {
        FullName = Name.Create($"{FirstName} {LastName}");
    }

    public bool IsDoctor()
    {
        return this.LicenseNumber.Value.StartsWith("D");
    }

    public string GetRole(string licenseNumberValue)
    {
        
        if (string.IsNullOrEmpty(licenseNumberValue)) {throw new ArgumentException("License number cannot be null or empty.", nameof(licenseNumberValue));}
        
        var firstChar = licenseNumberValue[0];

        return firstChar switch
        {
            'D' => "Doctor",
            'N' => "Nurse",
            'T' => "Technician",
            _ => throw new ArgumentException($"Unknown role for license number: {licenseNumberValue}")
        };
    }

}