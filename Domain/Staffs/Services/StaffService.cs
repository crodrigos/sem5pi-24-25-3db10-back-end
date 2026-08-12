using App.Onion.Domain.V.O.Patient;
using dddnet8.AuditLog.Interfaces;
using dddnet8.Domain.Patients.DTO;
using dddnet8.Domain.Patients.V.O;
using dddnet8.Domain.Specializations;
using dddnet8.Domain.Specializations.Interfaces;
using dddnet8.Domain.Staffs.DTO;
using dddnet8.Domain.Staffs.Interfaces;
using dddnet8.Domain.Staffs.V.O;
using dddnet8.Domain.SystemUsers;
using dddnet8.Infraestructure.Email;
using SurgicalManagement.Domain.Domain;

namespace dddnet8.Domain.Staffs.Services;

public class StaffService : IStaffService
{
    private readonly IEmailService _emailService;
    private readonly ILicenseNumberGenerator _licenseNumberGenerator;

    private readonly ILogService<Staff> _staffLogService;
    private readonly IStaffRepository _staffRepository;

    private readonly ISpecializationRepository _specializationRepository;

    /// <summary>
    ///     Initializes a new instance of the <see cref="StaffService" /> class.
    /// </summary>
    /// <param name="staffRepository">The staff repository to be used for data operations.</param>
    /// <param name="licenseNumberGenerator">The license number generator to create unique license numbers.</param>
    /// <param name="emailService">The email service for notifications.</param>
    /// <param name="staffLogService">The staff log service to save information in the system.</param>
    /// <exception cref="ArgumentNullException">Thrown when any of the dependencies are null.</exception>
    public StaffService(IStaffRepository staffRepository, ILicenseNumberGenerator licenseNumberGenerator,
        IEmailService emailService, ILogService<Staff> staffLogService, ISpecializationRepository specializationRepository)
    {
        _staffRepository = staffRepository ?? throw new ArgumentNullException(nameof(staffRepository));
        _licenseNumberGenerator =
            licenseNumberGenerator ?? throw new ArgumentNullException(nameof(licenseNumberGenerator));
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _staffLogService = staffLogService ?? throw new ArgumentNullException(nameof(staffLogService));
        _specializationRepository = specializationRepository ?? throw new ArgumentNullException(nameof(specializationRepository));
    }

    /// <summary>
    ///     Updates the data of a staff member based on the provided criteria and license number.
    /// </summary>
    /// <param name="staffCriteria">The criteria containing updated staff information.</param>
    /// <param name="licenseNumber">The license number of the staff member to be updated.</param>
    /// <returns>A task that represents the asynchronous operation, containing the updated staff member as a DTO.</returns>
    public async Task<StaffDto> UpdateStaffData(StaffCriteria staffCriteria, string licenseNumber)
    {
        var staff = await GetStaffByLicenseNumberAsync(licenseNumber);

        staff.UpdateStaff(staffCriteria);
        await UpdateStaffInRepository(staff);

        await _staffLogService.LogActionAsync("Update", staff);

        await NotifyStaffIfContactUpdated(staff, staffCriteria);

        return StaffMapper.ToDto(staff);
    }

    /// <summary>
    ///     Validates whether a staff member can be marked for deletion based on their medical record number.
    /// </summary>
    /// <param name="licenseNumber">The license number of the staff member to validate.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation, containing a tuple with a boolean indicating the result and
    ///     a message.
    /// </returns>
    public async Task<(bool result, string message)> ValidateStaffForDeletion(string licenseNumber)
    {
        var staff = await GetStaffByLicenseNumberAsync(licenseNumber);

        if (staff == null) return (false, "Staff not found.");

        return staff.DeletionStatus.IsToDelete
            ? (false, "Staff is already marked for deletion.")
            : (true,
                $"Staff {staff.FullName} is eligible for deletion. Are you sure you want to mark this staff for deletion?");
    }

    /// <summary>
    ///     Marks a staff member for deletion based on their license number.
    /// </summary>
    /// <param name="licenseNumber">The license number of the staff member to be marked for deletion.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task MarkStaffForDeletion(string licenseNumber)
    {
        var staff = await GetStaffByLicenseNumberAsync(licenseNumber);
        staff.MarkForDeletion();
        await UpdateStaffInRepository(staff);
    }

    /// <summary>
    ///     Creates a new staff member based on the provided data transfer object (DTO).
    /// </summary>
    /// <param name="createStaffDto">The create DTO containing staff information.</param>
    /// <returns>A task that represents the asynchronous operation, containing the created staff member as a DTO.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the staff DTO is null.</exception>
    public async Task<StaffDto> CreateStaffAsync(CreateStaffDto createStaffDto)
    {
        if (createStaffDto == null)
            throw new ArgumentNullException(nameof(createStaffDto), "Staff DTO cannot be null.");

        var userRole = ParseEnum<UserRole>(createStaffDto.Role, "Invalid user role");

        var licenseNumber = _licenseNumberGenerator.GenerateLicenseNumber(userRole);
        var contactInfo = CreateContactInfo(createStaffDto.ContactInformation);

        var specialization = await _specializationRepository.GetByNameAsync(createStaffDto.Specialization);

        if (specialization == null)
        {
            throw new KeyNotFoundException("Specialization not found.");
        }

        var staff = new Staff(
            Name.Create(createStaffDto.FirstName),
            Name.Create(createStaffDto.LastName),
            specialization,
            contactInfo,
            licenseNumber
        );

        await _staffRepository.AddStaffAsync(staff);
        return StaffMapper.ToDto(staff);
    }

    /// <summary>
    ///     Searches for staff members based on the provided filter criteria.
    /// </summary>
    /// <param name="criteria">The criteria used to filter staff members.</param>
    /// <returns>A task that represents the asynchronous operation, containing a list of filtered staff members as DTOs.</returns>
    public async Task<IEnumerable<StaffDto>?> SearchStaffByFiltersAsync(StaffCriteria criteria)
    {
        var staffs = await _staffRepository.SearchStaffByFiltersAsync(criteria);
        return staffs.Select(StaffMapper.ToDto);
    }

    /// <summary>
    ///     Retrieves all staff members from the repository.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation, containing a list of all staff members as DTOs.</returns>
    public async Task<IEnumerable<StaffDto>> GetAllStaffAsync()
    {
        var staffList = await _staffRepository.GetAllAsync();
        return StaffMapper.ToDtoList(staffList);
    }

    /// <summary>
    ///     Retrieves a staff member by their license number.
    /// </summary>
    /// <param name="licenseNumber">The license number to search for.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation, containing the staff member associated with the license
    ///     number.
    /// </returns>
    /// <exception cref="KeyNotFoundException">Thrown when the staff member is not found.</exception>
    private async Task<Staff> GetStaffByLicenseNumberAsync(string licenseNumber)
    {
        var staff = await _staffRepository.GetByLicenseNumberAsync(new LicenseNumber(licenseNumber));
        return staff;
    }

    /// <summary>
    ///     Parses a string value into the specified enum type.
    /// </summary>
    /// <typeparam name="TEnum">The enum type to parse into.</typeparam>
    /// <param name="value">The string value to parse.</param>
    /// <param name="errorMessage">The error message to throw if parsing fails.</param>
    /// <returns>The parsed enum value.</returns>
    /// <exception cref="ArgumentException">Thrown when the string value cannot be parsed.</exception>
    private static TEnum ParseEnum<TEnum>(string value, string errorMessage) where TEnum : struct
    {
        if (!Enum.TryParse(value, true, out TEnum result)) throw new ArgumentException(errorMessage);

        return result;
    }

    /// <summary>
    ///     Creates a ContactInfo object from the provided DTO.
    /// </summary>
    /// <param name="contactInfoDto">The DTO containing contact information.</param>
    /// <returns>A new ContactInfo object.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the contact info DTO is null.</exception>
    private static ContactInfo CreateContactInfo(ContactInfoDto contactInfoDto)
    {
        if (contactInfoDto == null)
            throw new ArgumentNullException(nameof(contactInfoDto), "Contact information cannot be null.");

        return new ContactInfo(
            new PhoneNumber(contactInfoDto.PhoneNumber),
            new EmailAddress(contactInfoDto.EmailAddress)
        );
    }

    /// <summary>
    ///     Notifies the staff member if their contact information has been updated.
    /// </summary>
    /// <param name="staff">The staff member whose contact information may have changed.</param>
    /// <param name="criteria">The criteria containing the updated contact information.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task NotifyStaffIfContactUpdated(Staff staff, StaffCriteria criteria)
    {
        if (criteria.ContactInformation != null)
            await _emailService.NotifyClientAboutUpdate(staff.ContactInfo.EmailAddress);
    }

    /// <summary>
    ///     Updates the staff member's information in the repository.
    /// </summary>
    /// <param name="staff">The staff member to update.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task UpdateStaffInRepository(Staff staff)
    {
        await _staffRepository.UpdateStaffDataAsync(staff);
    }
}