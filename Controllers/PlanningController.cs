using dddnet8.Domain.PlanningModuleNotifications;
using dddnet8.Domain.PlanningModuleNotifications.DTOs;
using Microsoft.AspNetCore.Mvc;


namespace dddnet8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanningController : ControllerBase
    {
        private readonly IPlanningService _planningService;

        // Constructor to inject the IPlanning service
        public PlanningController(IPlanningService planningService)
        {
            _planningService = planningService;
        }

        /// <summary>
        /// Get all surgery types with their estimated times (anesthesia, surgery, cleaning).
        /// </summary>
        [HttpGet("get-surgeries")]
        public async Task<IActionResult> GetAllSurgeries()
            // TODO -> WORKING
        {
            var surgeries = await _planningService.GetAllSurgeriesAsync();
            return Ok(surgeries);
        }

        /// <summary>
        /// Get all staff with their roles and specialties.
        /// </summary>
        [HttpGet("get-staff")]
        public async Task<IActionResult> GetAllStaff() // TODO -> DONE
        {
            var staffList = await _planningService.GetAllStaffAsync();
            return Ok(staffList);
        }

        /// <summary>
        /// Get timetable for a specific doctor/staff member.
        /// </summary>
        [HttpGet("get-timetable")]
        public async Task<IActionResult> GetAllStaffTimetable([FromQuery] DateOnly date) // TODO -> DONE
        {
            var timetable = await _planningService.GetTimetableForStaffAsync(date);
            if (timetable == null)
            {
                return NotFound("Timetable not found.");
            }

            return Ok(timetable);
        }

        /// <summary>
        /// Get the assignments (doctor-staff assignments) for surgeries.
        /// </summary>
        [HttpGet("get-assignments")]
        public async Task<IActionResult> GetAllAssignmentSurgery() // TODO -> ADAPT
        {
            var assignments = await _planningService.GetAllAssignmentsAsync();
            return Ok(assignments);
        }

        /// <summary>
        /// Get a specific surgery by its operation type code.
        /// </summary>
        [HttpGet("get-surgeriesId")]
        public async Task<IActionResult> GetAllSurgeriesId() // TODO -> DONE
        {
            var surgery = await _planningService.GetAllSurgeriesId();
            if (surgery == null)
            {
                return NotFound($"Surgery not found.");
            }

            return Ok(surgery);
        }

        /// <summary>
        /// Get staff assignments for a specific surgery request.
        /// </summary>
        [HttpGet("get-agenda-staffs")]
        public async Task<IActionResult> GetAllAgendaStaff([FromQuery] DateOnly date) // TODO -> DONE
        {
            Console.WriteLine("Date-------->" + date);
            var assignments = await _planningService.GetAllAgendaStaff(date);
            return Ok(assignments);
        }

        [HttpGet("get-agenda-operation-room")]

        public async Task<IActionResult> GetAllAgendaOperationRoom([FromQuery] DateOnly date, [FromQuery] string room)
        {
            var agendaRooms = await _planningService.GetAllAgendaOperationRoom(date, room);
            return Ok(agendaRooms);
        }

        [HttpGet("get-rooms-occupation")]
        public async Task<IActionResult> GetAllRoomsOccupationByDate([FromQuery] string date)
        {
            try
            {
                Console.WriteLine("Cheguei aqui com a data ----------------> " + date);

                var result = await _planningService.GetAllRoomsOccupationByDate(DateTime.Parse(date));

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    new { Message = $"Erro ao processar a solicitação. Tente novamente mais tarde.{ex.Message}" });
            }
        }

        [HttpGet("get-all-data")]
        public async Task<IActionResult> GetAllSurgeriesForScheduling([FromQuery] DateOnly date, [FromQuery] string room)
        {
            var allDataForScheduling = await _planningService.GetAllDataForScheduling(date,room);
            return Ok(allDataForScheduling);
        }

        [HttpPost("save-planning")]
        public async Task<IActionResult> SavePlanning([FromBody] SavePlanningDto savePlanningDto)
        {

           await _planningService.SavePlanning(savePlanningDto);

            return Ok();

        }
    }
}
    

