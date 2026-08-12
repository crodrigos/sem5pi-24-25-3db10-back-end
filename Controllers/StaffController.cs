using dddnet8.Domain.Staffs.DTO;
using dddnet8.Domain.Staffs.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dddnet8.Controllers;
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class StaffController : ControllerBase
{
    private readonly IStaffService _staffService;

    /// <summary>
    ///     Initializes a new instance of the <see cref="StaffController" /> class.
    /// </summary>
    /// <param name="staffService">The service for handling staff-related operations.</param>
    /// <exception cref="ArgumentNullException">Thrown when any of the dependencies are null.</exception>
    public StaffController(IStaffService staffService, ILogger<StaffController> logger)
    {
        _staffService = staffService ?? throw new ArgumentNullException(nameof(staffService));
    }

    /// <summary>
    ///     Creates a new staff member based on the provided data transfer object (DTO).
    /// </summary>
    /// <param name="createStaffDto">The DTO containing staff information to create.</param>
    /// <returns>An ActionResult containing the created staff DTO and the appropriate status code.</returns>
    
    [HttpPost]
    public async Task<ActionResult<StaffDto>> CreateStaff([FromBody] CreateStaffDto createStaffDto)
    {
        if (createStaffDto == null) return BadRequest("Staff data must not be null.");

        try
        {
            var staffDto = await _staffService.CreateStaffAsync(createStaffDto);
            return CreatedAtAction(nameof(CreateStaff), new { licenseNumber = staffDto.LicenseNumber }, staffDto);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
        }
    }

    /// <summary>
    ///     Searches for staff members based on the specified criteria.
    /// </summary>
    /// <param name="criteria">The criteria for searching staff members.</param>
    /// <returns>An IActionResult containing the list of matching staff members or a not found status.</returns>
    
    [HttpGet("search")]
    public async Task<IActionResult> SearchStaffByCriteria([FromQuery] StaffCriteria criteria)
    {
        try
        {
            var staffs = await _staffService.SearchStaffByFiltersAsync(criteria);

            if (staffs == null || !staffs.Any())
                return NotFound(new { message = "No staff members found matching the search criteria." });

            return Ok(staffs);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
        }
    }

    /// <summary>
    ///     Updates the information of a staff member identified by their license number.
    /// </summary>
    /// <param name="licenseNumber">The license number of the staff member to update.</param>
    /// <param name="criteria">The criteria containing updated staff information.</param>
    /// <returns>An IActionResult indicating the outcome of the update operation.</returns>
    
    [HttpPut("{licenseNumber}")]
    public async Task<IActionResult> UpdateStaff(string licenseNumber, [FromBody] StaffCriteria criteria)
    {
        if (string.IsNullOrWhiteSpace(licenseNumber))
            return BadRequest(new { message = "License number is required." });

        try
        {
            var updatedStaffDto = await _staffService.UpdateStaffData(criteria, licenseNumber);
            return Ok(new { message = "Staff updated successfully.", updatedStaffDto });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
        }
    }

    /// <summary>
    ///     Marks a staff member for deletion based on their license number.
    /// </summary>
    /// <param name="licenseNumber">The license number of the staff member to delete.</param>
    /// <param name="confirm">Indicates whether the deletion should proceed.</param>
    /// <returns>An IActionResult indicating the result of the deletion operation.</returns>
    
    [HttpDelete("confirmation/{licenseNumber}")]
    public async Task<IActionResult> ConfirmStaffDelete(string licenseNumber)
    {
        if (string.IsNullOrWhiteSpace(licenseNumber)) return BadRequest(new { message = "Invalid license number." });
        
        try
        {
            await _staffService.MarkStaffForDeletion(licenseNumber);
            return Ok(new { message = "Staff marked for deletion successfully." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = $"An error occurred: {ex.Message}" });
        }
    }

    /// <summary>
    ///     Validates if a staff member can be marked for deletion.
    /// </summary>
    /// <param name="licenseNumber">The license number of the staff member to validate.</param>
    /// <returns>An IActionResult indicating whether the staff member can be marked for deletion.</returns>
    
    
    
    [HttpDelete("delete/{licenseNumber}")]
    public async Task<IActionResult> DeleteStaff(string licenseNumber)
    {
        if (string.IsNullOrWhiteSpace(licenseNumber)) return BadRequest(new { message = "Invalid license number." });

        try
        {
            var (result, message) = await _staffService.ValidateStaffForDeletion(licenseNumber);

            if (!result) return NotFound(new { message });

            return Accepted(new { message, confirmationRequired = true });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
        }
    }
}