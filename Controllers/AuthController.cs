using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using App.Security;
using App.SystemUser.Domain.DTO;
using dddnet8.Domain.Authentication;
using dddnet8.Domain.Authentication.token;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;


namespace App.Login;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ITokenService _tokenService;

    public AuthController(IAuthService authService, ITokenService tokenService)
    {
        _authService = authService;
        _tokenService = tokenService;
    }
    
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (loginDto == null) 
        {
            return BadRequest("Invalid login data");
        }

        var (token, message, userdto) = await _authService.Login(loginDto);
        
        Console.WriteLine(message + "-- " +  token);

        return token == null ? StatusCode(500, message) : Ok(new { Token = token, Message = message, User = userdto });
    }

    

    [HttpPost("activate-account")]
    public async Task<IActionResult> ActivateAccount([FromQuery] string token,[FromBody] PasswordDto request)
    {

        if (request.Password != request.Confirmation) {return BadRequest("Password and confirmation do not match.");}

        var (result, message) = await _authService.ActivateUserAccount(request,token);

        return result ? Ok(message) : StatusCode(500, $"Failed to activate account.{message}" );
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPasswordRequest([FromBody] ResetPasswordRequestDTO request)
    {
        var (result, message) = await _authService.ResetUserPasswordRequest(request.UserEmail);

        return result ? Ok(new { message }) : StatusCode(500, new { message });
    }


    [HttpPut("change-password")]
    public async Task<IActionResult> ResetPasswordRequest([FromQuery] string token,[FromBody] PasswordDto request){
        
        Console.WriteLine(token);
        
        if (request.Password != request.Confirmation)
        {
            return BadRequest("Password and confirmation do not match.");
        }

        var (result, message) = await _authService.PasswordUserReset(request.Password, token);

        return result ? Ok(new{message}) : StatusCode(500, new{message});
    }
    
    //-------------------------------------------------------------------------------------------------------------------------

    // Endpoint para iniciar o login com o Facebook
    [HttpGet("login-facebook")]
    public async Task<IActionResult> LoginFacebook(string returnUrl = "/")
    {
        var redirectUrl = Url.Action("FacebookResponse", "Auth", new { returnUrl });
        
        return await _authService.CreateAccountFromFacebook(redirectUrl);
    }
    

    // Endpoint que lida com a resposta do Facebook após o login
    [HttpGet("facebook-signup")]
    public async Task<IActionResult> FacebookResponse(string returnUrl = "/")
    {
        var result = await HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);

        if (!result.Succeeded) {return BadRequest("Error authenticating with Facebook.");}

         var (redirectUrl, message) = await _authService.FacebookResponse(result);

        if (message == null) {return Redirect(redirectUrl);}

        return StatusCode(500, new { message = message });
    }
}




