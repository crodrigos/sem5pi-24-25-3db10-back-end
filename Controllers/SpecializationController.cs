using dddnet8.Domain.Specializations.DTO;
using dddnet8.Domain.Specializations.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dddnet8.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
public class SpecializationController : ControllerBase
{
    
    private readonly  ISpecializationService _specializationService;

    public SpecializationController(ISpecializationService specializationService)
    {
        _specializationService = specializationService;
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<SpecializationDto>> CreateSpecialization([FromBody] SpecializationDto createdSpecialization)
    {
        if (createdSpecialization == null)
        {
            return BadRequest("Invalid specilaization data");
        }

        var specializationDto = await _specializationService.CreateSpecialization(createdSpecialization);

        return specializationDto != null
            ? Created(nameof(CreateSpecialization), specializationDto)
            : StatusCode(500, new { message = "Internal server error" });
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet("search")]
    public async Task<IActionResult> GetSpecializationsByCriteria([FromQuery] SpecializationByCriteriaDTO criteriaDto)
    {
        try
        {
            // Chama o serviço para obter as especializações
            var specializationsDTO = await _specializationService.GetSpecializationsByCriteria(criteriaDto);

            // Se não encontrar especializações, retorna NotFound
            if (specializationsDTO == null || !specializationsDTO.Any())
            {
                return NotFound("No specializations found matching the criteria.");
            }

            // Retorna a lista de especializações com status 200 OK
            return Ok(specializationsDTO);
        }
        catch (Exception ex)
        {
            // Retorna erro interno do servidor (500) com a mensagem de exceção
            // Em produção, considere não expor a mensagem de erro diretamente
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPut("{specializationCode}")]
    public async Task<IActionResult> UpdateSpecialization([FromBody] SpecializationByCriteriaDTO specializationDto,
        string specializationCode)
    {

        if (specializationDto == null)
        {
            return BadRequest(new { message = "Invalid specialization data." });
        }

        try
        {
            var updatedSpecializationDto = await _specializationService.UpdateSpecializationData(specializationDto, specializationCode);

            return Ok(new { message = "Specialization updated successfully.", updatedSpecializationDto });
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
    [HttpDelete("confirmation/{specializationCode}")]
    public async Task<IActionResult> ConfirmSpecializationDelete(string specializationCode)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(specializationCode))
            {
                return BadRequest(new { message = "Invalid specialization code." });
            }

            await _specializationService.DeleteSpecialization(specializationCode);

            return Ok(new { message = "Specialization deleted successfully." });
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
}