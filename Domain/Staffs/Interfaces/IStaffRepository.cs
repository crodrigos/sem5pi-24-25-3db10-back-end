using dddnet8.Domain.Shared;
using dddnet8.Domain.Staffs.DTO;
using dddnet8.Domain.Staffs.V.O;
using dddnet8.Domain.SystemUsers;

namespace dddnet8.Domain.Staffs.Interfaces;

/// <summary>
///     Interface representing the contract for a repository managing staff entities.
///     This repository provides methods to add, update, remove, and query staff data.
/// </summary>
public interface IStaffRepository : IRepository<Staff, Guid>
{
    /// <summary>
    ///     Asynchronously adds a new staff member to the repository.
    /// </summary>
    /// <param name="staff">The staff entity to be added.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task AddStaffAsync(Staff staff);

    /// <summary>
    ///     Asynchronously retrieves a staff member by their license number.
    /// </summary>
    /// <param name="licenseNumber">The license number of the staff member to retrieve.</param>
    /// <returns>
    ///     A Task representing the asynchronous operation, with a result of the staff member if found, or null if not.
    /// </returns>
    Task<Staff?> GetByLicenseNumberAsync(LicenseNumber licenseNumber);

    /// <summary>
    ///     Asynchronously searches for staff members that match the provided criteria.
    /// </summary>
    /// <param name="criteria">The filter criteria to search staff members.</param>
    /// <returns>
    ///     A Task representing the asynchronous operation, with a result of an enumerable list of staff members that
    ///     match the criteria.
    /// </returns>
    Task<IEnumerable<Staff>> SearchStaffByFiltersAsync(StaffCriteria criteria);

    /// <summary>
    ///     Asynchronously updates the data of an existing staff member.
    /// </summary>
    /// <param name="staff">The staff entity with updated data.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task UpdateStaffDataAsync(Staff staff);

    /// <summary>
    ///     Asynchronously retrieves the license number of a staff member by their email address.
    /// </summary>
    /// <param name="userEmail">The email address of the staff member.</param>
    /// <returns>
    ///     A Task representing the asynchronous operation, with a result of the license number associated with the
    ///     provided email.
    /// </returns>
    Task<LicenseNumber> GetLicenseNumberByEmailAddressAsync(EmailAddress userEmail);

    /// <summary>
    ///     Asynchronously retrieves a list of staff members who are marked for deletion.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation, with a result of a list of staff members flagged for deletion.</returns>
    Task<List<Staff>> GetStaffMarkedForDeletionAsync();

    /// <summary>
    ///     Asynchronously removes a staff member from the repository.
    /// </summary>
    /// <param name="staff">The staff entity to be removed.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task RemoveStaffAsync(Staff staff);
}