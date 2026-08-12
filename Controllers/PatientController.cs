using Microsoft.AspNetCore.Mvc;
using App.Onion.Application.Dtos;
using App.Onion.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;


namespace App.Onion.Application.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]")]

    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService ?? throw new ArgumentNullException(nameof(patientService));
        }


        [Authorize(Roles = "Admin,Patient")]
        [HttpPost]
        public async Task<ActionResult<PatientDto>> CreatePatient([FromBody] CreatePatientDTO createdPatient)
        {
            if (createdPatient == null)
            {
                return BadRequest("Invalid patient data");
            }

            var patientDto = await _patientService.CreatePatient(createdPatient);

            return patientDto != null
                ? Created(nameof(CreatePatient), patientDto)
                : StatusCode(500, new { message = "Internal server error" });
        }




        //[Authorize(Roles = "Admin")]
        [HttpGet("search")]
        public async Task<IActionResult> ListPatientsByFilter([FromQuery] PatientCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest(new { message = "Search criteria cannot be null." });
            }

            try
            {
                var patients = await _patientService.SearchPatientsByFilters(criteria);

                if (patients == null || !patients.Any())
                {
                    return NotFound(new { message = "No patients found matching the search criteria." });
                }

                return Ok(patients);
            }
            catch (Exception ex)
            {
                var errorMessage = "An error occurred while searching for patients: " + ex.Message;

                return StatusCode(500, new { message = errorMessage });
            }
        }

        [Authorize(Roles = "Admin, Patient")]
        [HttpPut("{medicalRecordNumber}")]
        public async Task<IActionResult> UpdatePatient([FromBody] PatientCriteria patientDto,
            string medicalRecordNumber)
        {

            if (patientDto == null)
            {
                return BadRequest(new { message = "Invalid patient data." });
            }

            try
            {
                var updatedPatientDto = await _patientService.UpdatePatientData(patientDto, medicalRecordNumber);

                return Ok(new { message = "Patient updated successfully.", updatedPatientDto });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex });
            }
            catch (Exception ex)
            {
                // Aqui você pode logar a exceção se necessário
                return StatusCode(500,
                    new { message = "An error occurred while updating the patient.", details = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("profile/confirmation/{medicalRecordNumber}")]
        public async Task<IActionResult> ConfirmPatientDelete(string medicalRecordNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(medicalRecordNumber))
                {
                    return BadRequest(new { message = "Invalid medical record number." });
                }

                await _patientService.MarkPatientForDeletion(medicalRecordNumber);

                return Ok(new { message = "Patient deleted successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }

        [Authorize(Roles = "Patient")]
        [HttpDelete("account/confirmation/{medicalRecordNumber}")]
        public async Task<IActionResult> ConfirmPatientAccountDelete(string medicalRecordNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(medicalRecordNumber))
                {
                    return BadRequest(new { message = "Invalid medical record number." });
                }

                Console.WriteLine($"Attempting to mark patient {medicalRecordNumber} for deletion.");

                await _patientService.MarkPatientAssociatedDataForDeletion(medicalRecordNumber);

                Console.WriteLine("ALL SET");

                return Ok(new { message = "Patient deleted successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }

        [Authorize(Roles = "Patient")]
        [HttpGet("{username}")]
        public async Task<IActionResult> GetPatientByUsername(string username)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username))
                {
                    return BadRequest(new { message = "Invalid username." });
                }

                var patient = await _patientService.GetPatientByUsername(username);

                return Ok(patient);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }

        [Authorize(Roles = "Patient")]
        [HttpPost("dpoRequest/{medicalRecordNumber}")]

        public async Task<IActionResult> RequestDeletionOfPersonalData([FromBody] string medicalRecordNumber)
        {
            if (medicalRecordNumber == null)
            {
                return BadRequest("Medical record number cannot be null.");
            }

            var (result, error) = await _patientService.RequestDpoToDeleteMyAccount(medicalRecordNumber);

            if (!result)
            {
                return BadRequest(error);
            }

            return Ok("Request to delete personal data has been sent successfully.");
        }
    }
}
