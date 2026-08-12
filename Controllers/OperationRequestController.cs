using dddnet8.Domain.OperationRequests;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dddnet8.Controllers;

[ApiController]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//[Authorize(Roles = "Doctor")]
[Route("api/[controller]")]
public class OperationRequestController : ControllerBase
{
    private readonly IOperationRequestService _operationRequestService;

    public OperationRequestController(IOperationRequestService operationRequestService)
    {
        _operationRequestService = operationRequestService;
    }

    // Create OperationRequest

    [HttpPost]
    public async Task<IActionResult> CreateOperationRequest([FromBody] CreateOperationRequestDto dto){
        
        if (dto is null)
            return BadRequest("Invalid operation request data.");
        
        var result = await _operationRequestService.CreateOperationRequest(dto);

        return result != null
            ? Created(nameof(CreateOperationRequest), result)
            : StatusCode(500, new { message = "Internal server error in OperationRequest" });
        
    }

    // Get OperationRequest by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _operationRequestService.GetOperationRequest(id);
        if (result.IsFailure)
            return NotFound(result.Error);

        return Ok(result.Value);
    }

    // Get all OperationRequests
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _operationRequestService.GetAllOperationRequests();
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }
    
    
    
    
    

    // Update a OperationRequest
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] OperationRequestCriteria dto)
    {
        if (dto == null || id == null)
            return BadRequest("Invalid operation request data or mismatched ID.");

        try
        {
            var updatedDto = await _operationRequestService.UpdateOperationRequest(dto, id);
            return Ok(new { message = "Operation Request updated successfully.", updatedDto });
        }
        catch (KeyNotFoundException ex)
        {
            Console.WriteLine("message -> " + ex.Message);
            return NotFound(new { message = ex });

        }

        catch (Exception ex)
        { Console.WriteLine("message -> " + ex.Message );
            return StatusCode(500,new { message = "An error occurred while updating the Operation Request.", details = ex.Message });
            
        }
    }

    
    // Delete/Remove a OperationRequest by ID
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        Console.WriteLine("CHEGUEI AQUI");
        var result = await _operationRequestService.DeleteOperationRequest(id);
        if (result.IsFailure)
        {
            Console.Error.WriteLine(result.Error);
            return BadRequest(result.Error);
        }
        return NoContent();
    }
    
    // Search OperationRequests
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] OperationRequestCriteria criteria)
    {
        
        var result = await _operationRequestService.SearchOperationRequests(criteria);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
}