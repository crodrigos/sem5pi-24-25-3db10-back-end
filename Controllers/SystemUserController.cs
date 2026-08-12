using System.Web;
using App.SystemUserStuff;
using dddnet8.Domain.Authentication.token;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace YourNamespace.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ISystemUserService _userService;
    private readonly ITokenService _tokenService;

    public UserController(ISystemUserService userService, ITokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] SystemUserRequestDto userRequestDto)
    {
        if (userRequestDto == null) return BadRequest("Invalid data.");

        try
        {
            var userDto = await _userService.CreateUser(userRequestDto);

            var response = "Usuário criado com sucesso. Um e-mail de ativação foi enviado para " +
                           userDto.EmailAddress;

            return Created("", response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    
    [HttpPost("IAM")] 
    public async Task<IActionResult> CreateUserFromIAM([FromQuery] string token)
    {
        try
        {
            var claimsPrincipal = _tokenService.ValidateToken(token);

            var (systemuserdto, returnedToken, errorMessage) = await _userService.CreateUserFromIAM(claimsPrincipal);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                return Conflict(new { message = "Error creating user", details = errorMessage });
            }

            return Ok(new
            {
                message = "User created successfully",
                user = systemuserdto,
                token = returnedToken
            });
        }
        catch (SecurityTokenException ex){ return Unauthorized(new { message = ex.Message });}
        catch (Exception ex) {return StatusCode(500, new { message = "Internal server error", details = ex.Message });}
    }


   
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "Patient")]
    [HttpDelete("{username}")]
    public async Task<IActionResult> DeleteUser(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return BadRequest(new { message = "Username cannot be empty or null." });
        }

        try
        {
            var (isDeleted, responseMessage) = await _userService.DeleteUser(username);

            if (!isDeleted)
            {
                return NotFound(new { message = responseMessage });
            }

            return Ok(new { message = responseMessage });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An unexpected error occurred. Please try again later.", error = ex.Message });
        }
    }
}