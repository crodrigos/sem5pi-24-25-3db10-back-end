using dddnet8.Domain.Appointments.DTO;
using dddnet8.Domain.Appointments.Interfaces;
using dddnet8.Domain.Timetables;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dddnet8.Controllers;

//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;
    private readonly ILogger<AppointmentController> _logger;

    /// <summary>
    ///     Initializes a new instance of the <see cref="AppointmentController" /> class.
    /// </summary>
    /// <param name="appointmentService">The service for handling appointment-related operations.</param>
    /// <param name="logger">The logger instance for logging purposes.</param>
    /// <exception cref="ArgumentNullException">Thrown when any of the dependencies are null.</exception>
    public AppointmentController(IAppointmentService appointmentService, ILogger<AppointmentController> logger)
    {
        _appointmentService = appointmentService ?? throw new ArgumentNullException(nameof(appointmentService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }


    [HttpGet()]
    public async Task<IActionResult> GetAllOperationRequestWithoutAppointments()
    {
        try
        {
            var operationRequestsWithoutAppointments = await _appointmentService.GetOperationRequestsWithoutAppointmentsAsync();

            if (operationRequestsWithoutAppointments == null || !operationRequestsWithoutAppointments.Any())
            {
                return NotFound(new { message = "No operation requests without appointments were found." });
            }

            return Ok(operationRequestsWithoutAppointments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving operation requests without appointments.");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
        }
    }

    [HttpGet("appointments")]
    public async Task<IActionResult> GetAllAppointments() {
        try {
            var allAppointments = await _appointmentService.GetAllAppointments();
            return Ok(allAppointments);
        }catch (Exception ex) {
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error." + ex);
        } 
    }
    
    [HttpGet("appointment-data")] 
    public async Task<IActionResult> GetDataForAppointment([FromQuery] string operationRequestCode) {
        try {
            var appointmentData = await _appointmentService.GetDataForAppointment(operationRequestCode);
            return Ok(appointmentData);
        }catch (Exception ex) {
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error." + ex);
        } 
    }

    [HttpPut("update-appointment")]
    public async Task<IActionResult> UpdateAppointment([FromBody] UpdateAppointmentDTO updateAppointmentDto) {
        try
        {
            _appointmentService.UpdateAppointment(updateAppointmentDto);
            return Ok(updateAppointmentDto);
        }catch (Exception ex) {
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error." + ex);
        }
    }





    
    [HttpGet("{operationRequestCode}")]
    public async Task<IActionResult> GetAllInformationForAppointment(string operationRequestCode)
    {
        if (string.IsNullOrWhiteSpace(operationRequestCode))
            return BadRequest(new { message = "Operation request code must not be null or empty." });

        try
        {
            var appointmentDetails = await _appointmentService.GetStaffForAppointmenGetDetailsByCode(operationRequestCode);

            if (appointmentDetails == null)
                return NotFound(new { message = "No appointment found for the provided operation request code." });

            return Ok(appointmentDetails);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid input provided for operation request code.");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving appointment information.");
            return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
        }
    }

    [HttpGet("surgeryRooms")]
    public async Task<IActionResult> GetAllSurgeryRooms()
    {
        try
        {
            // Chama o método do AppointmentService para recuperar as salas de cirurgia
            var surgeryRooms = await _appointmentService.GetAllSurgeryRoomsForAppointment();

            if (surgeryRooms == null)
            {
                return NotFound(new { message = "No surgery rooms found." });
            }

            return Ok(surgeryRooms);
        }
        catch (Exception ex)
        {
            // Em caso de erro, retorna erro 500
            return StatusCode(500, new { message = $"An error occurred while retrieving surgery rooms: {ex.Message}" });
        }
    }
    
    // so para verificar se o method no service funciona. testado no postman, ele funciona.
    [HttpGet("test")]
    public async Task<IActionResult> TestTimetable([FromBody] TimetableRequest request)
    {
        try
        {
            var date = DateTime.Parse(request.StartTime);
            var test = await _appointmentService.testTimetable(date, request.LicenseNumber);

            if (test == null)
            {
                return NotFound(new { message = "test is null." });
            }

            return Ok(test);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
        }
    }
    
    [HttpPost("createAppointmentByDoctor")]
    public async Task<IActionResult> CreateAppointmentByDoctor([FromBody] CreateAppointmentDTO createAppointmentDto)
    {
        
        Console.WriteLine("entrei");
        
        // Verifica se o DTO é nulo ou inválido
        if (createAppointmentDto == null)
        {
            return BadRequest(new { message = "Appointment data must not be null." });
        }

        try
        {
            // Chama o serviço para criar o agendamento, passando os dados do DTO
            var appointment = await _appointmentService.CreateAppointmentByDoctorAsync(createAppointmentDto);

            // Verifica se o agendamento foi criado com sucesso
            if (appointment == null)
            {
                return BadRequest(new { message = "Failed to create the appointment." });
            }

        }
        catch (Exception ex)
        {
            // Em caso de erro, retorna um erro 500 com a mensagem
            _logger.LogError(ex, "An error occurred while creating the appointment.");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"An error occurred: {ex.Message}" });
        }
        Console.WriteLine("CHEGUEI NO OK");
        return Ok();
    }
}
