using dddnet8.Domain.Staffs.DTO;

namespace dddnet8.Domain.Staffs.Interfaces;

/// <summary>
///     Defines the contract for the service managing staff-related business operations.
///     Provides methods for creating, searching, updating, and managing the deletion of staff members.
/// </summary>
public interface IStaffService
{
    /// <summary>
    ///     Creates a new staff member based on the provided data.
    /// </summary>
    /// <param name="staffDto">The DTO containing the details required to create a new staff member.</param>
    /// <returns>A Task representing the asynchronous operation, with the result being the created staff's DTO.</returns>
    Task<StaffDto> CreateStaffAsync(CreateStaffDto staffDto);

    /// <summary>
    ///     Searches for staff members using the specified filter criteria.
    /// </summary>
    /// <param name="criteria">The filter criteria to apply when searching for staff members.</param>
    /// <returns>
    ///     A Task representing the asynchronous operation, with the result being an enumerable of staff DTOs
    ///     that match the provided criteria, or null if no matches are found.
    /// </returns>
    Task<IEnumerable<StaffDto>?> SearchStaffByFiltersAsync(StaffCriteria criteria);

    /// <summary>
    ///     Updates the data of an existing staff member based on their license number.
    /// </summary>
    /// <param name="staffDto">The DTO containing the updated information for the staff member.</param>
    /// <param name="licenseNumber">The license number of the staff member to be updated.</param>
    /// <returns>A Task representing the asynchronous operation, with the result being the updated staff's DTO.</returns>
    Task<StaffDto> UpdateStaffData(StaffCriteria staffDto, string licenseNumber);

    /// <summary>
    ///     Validates whether a staff member can be safely marked for deletion.
    /// </summary>
    /// <param name="licenseNumber">The license number of the staff member to validate for deletion.</param>
    /// <returns>
    ///     A Task representing the asynchronous operation, with a result tuple indicating
    ///     whether the staff member can be deleted and an accompanying message.
    /// </returns>
    Task<(bool result, string message)> ValidateStaffForDeletion(string licenseNumber);

    /// <summary>
    ///     Marks a staff member for deletion based on their license number.
    /// </summary>
    /// <param name="licenseNumber">The license number of the staff member to be marked for deletion.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task MarkStaffForDeletion(string licenseNumber);
}