using System;
using dddnet8.Domain.OperationTypes;
using dddnet8.Domain.OperationTypes.DTO;
using dddnet8.Infraestructure.OperationTypes;
using dddnet8.Infraestructure.Shared.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace dddnet8.Controllers;
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
public class OperationTypeController : ControllerBase
{

    private readonly IOperationTypeService _operationTypeService;

    public OperationTypeController(IOperationTypeService operationTypeService)
    {
        _operationTypeService = operationTypeService ?? throw new ArgumentNullException(nameof(operationTypeService));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOperationTypesAsync()
    {
        try
        {
            List<OperationTypeDTO> all = await _operationTypeService.GetAll();

            if (all == null)
            {
                return NotFound();
            }

            return Ok(all);
        }
        catch (Exception ex) {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }

    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddOperationTypeAsync([FromBody] OperationTypeAddDTO operationTypeDTO)
    {
        try
        {
            OperationTypeDTO added = await _operationTypeService.Add(operationTypeDTO);

            return Ok(added);
        }
        catch (MultipleArgumentException ex)
        {
            return StatusCode(StatusCodes.Status400BadRequest, ex._messages);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateOperationTypeAsync(string id, [FromBody] OperationTypeDTO operationTypeDTO)
    {
        try
        {
            OperationTypeDTO updated = await _operationTypeService.Update(id, operationTypeDTO);

            return Ok(updated);
        }
        catch (MultipleArgumentException ex)
        {
            return StatusCode(StatusCodes.Status400BadRequest, ex._messages);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpDelete("{code}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveOperationTypeAsync(string code)
    {
        try
        {
            OperationTypeDTO removed = await _operationTypeService.RemoveByCode(code);

            return Ok(removed);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}