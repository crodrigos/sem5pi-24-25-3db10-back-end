using System.Security.Claims;
using App.Domain.SystemUser;
using App.Passsword.Encoder;
using App.Security;
using App.SystemUser.Domain.DTO;
using dddnet8.Domain.Authentication.token;
using dddnet8.Domain.SystemUsers.DTO;
using dddnet8.Infraestructure.Email;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using SurgicalManagement.Domain.Domain;

namespace dddnet8.Domain.Authentication;

public class AuthService : IAuthService
{
    private readonly IEmailService _emailService;
    private readonly ILoginAttemptsService _loginAttemptsService;
    private readonly IPasswordEncoder _passwordEncoder;

    private readonly ISystemUserService _systemUserService;
    private readonly ITokenService _tokenService;
    private readonly IAuthenticationService _authenticationService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;


    public AuthService(
        ISystemUserService systemUserService,
        ILoginAttemptsService loginAttemptsService,
        IEmailService emailService,
        ITokenService tokenService,
        IPasswordEncoder passwordEncoder,
        IAuthenticationService authenticationService,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration)
    {
        _systemUserService = systemUserService;
        _loginAttemptsService = loginAttemptsService;
        _emailService = emailService;
        _tokenService = tokenService;
        _passwordEncoder = passwordEncoder;
        _authenticationService = authenticationService;
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
    }


    public async Task<(string? token, string errorMessage, SystemUserDto? userDto)> Login(LoginDto loginDto)
    {
        var user = await _systemUserService.GetUserByUsername(loginDto.Username);

        if (user == null) return (null, "User does not exist.", null);

        if (!user.IsActive)
        {
            SendAccountActivationConfirmation(SystemUserMapper.ToDto(user));

            return (null,
                "You cannot log in at this time. Please activate your account first. An email has been sent with activation instructions.", null);
        }

        if (_loginAttemptsService.IsUserLockedOut(loginDto.Username))
        {
            var admins = await _systemUserService.GetUsersByRole(UserRole.Admin);

            _emailService.SendAdminWarningNotification(admins, user.EmailAddress.ToString());

            return (null, "You are temporarily locked out due to multiple failed login attempts. Admin was also notified.", null);
        }

        if (!await ValidateUser(loginDto))
        {
            _loginAttemptsService.RegisterFailedAttempt(loginDto.Username);
            return (null, "Invalid Password.", null);
        }

        _loginAttemptsService.ResetLoginAttempts(loginDto.Username);

        var userDto = new SystemUserDto(user.Username.ToString(), user.EmailAddress.ToString(), user.Role.ToString());

        var token = _tokenService.GenerateJwtToken(userDto);

        return (token, "Login Successful", userDto);
    }

    public async Task<(bool Success, string message)> ActivateUserAccount(PasswordDto request, string token)
    {
        var (isValid, result) = ValidateTokenAndEmail(token);

        if (!isValid) return (false, result);

        try
        {
            _systemUserService.ActivateUserAccount(result, request.Password);
        }
        catch (Exception ex)
        {
            return (false, $"Error activating user account: {ex.Message}");
        }

        var user = await _systemUserService.GetUserByEmail(result);

        SendAccountActivationConfirmation(SystemUserMapper.ToDto(user));

        return (true, $"Your account created with email {result} has been activated successfully.");
    }

    public async Task<(bool success, string message)> ResetUserPasswordRequest(string userEmail)
    {
        try
        {
            var userDto = await _systemUserService.GetUserByEmail(userEmail);

            var token = _tokenService.GenerateResetToken(SystemUserMapper.ToDto(userDto));
            
            Console.WriteLine("Token Gerado ->" + token);

            await _emailService.ResetPasswordNotification(userDto.EmailAddress.ToString(), token);

            return (true, "A notification has been sent to your email to reset your password.");
        }
        catch (Exception ex)
        {
            return (false, $"An error occurred while processing your request.{ex.Message}");
        }
    }

    public async Task<(bool result, string message)> PasswordUserReset(string requestPassword, string token)
    {
        
        var (isValid, result) = ValidateTokenAndEmail(token);
        
        if (!isValid) return (false, result);

        var user = await _systemUserService.GetUserByEmail(result);

        try{await _systemUserService.ResetUserPassword(user, requestPassword);}
        
        catch (Exception ex) {return (false, $"Error trying to rest user account password: {ex.Message}");}

        return (true, "Your account password has been reset successfully.");
    }

    public async Task<IActionResult> CreateAccountFromFacebook(string redirectUrl)
    {
        var properties = new AuthenticationProperties { RedirectUri = redirectUrl };

        var context = _httpContextAccessor.HttpContext;

         await _authenticationService.ChallengeAsync(context, FacebookDefaults.AuthenticationScheme, properties);

         return new EmptyResult();
    }

    public async Task<(string redirect, string message)> FacebookResponse(AuthenticateResult result)
    {
        var token = _tokenService.GenerateJwtTokenForPatient(result);

        if (token == null)
        {
            return (null, "Invalid token.");
        }

        var redirect = _configuration["PatientCreateAccountUrl"];

        return ($"{redirect}token={token}", null);

    }
    


    private async Task<bool> ValidateUser(LoginDto loginDto)
    {
        if (string.IsNullOrEmpty(loginDto.Username))
            throw new ArgumentException("O nome de usuário não pode ser nulo ou vazio.");

        if (string.IsNullOrEmpty(loginDto.Password)) throw new ArgumentException("A senha não pode ser nula ou vazia.");

        var user = await _systemUserService.GetUserByUsername(loginDto.Username);

        return _passwordEncoder.Verify(loginDto.Password, user.Password);
    }

    private async Task  SendAccountActivationConfirmation(SystemUserDto systemUserDto)
    {
        await _emailService.SendAccountActivationConfirmation(systemUserDto);
    }


    // TO AVOID CODE DUPLICATION
    private (bool, string) ValidateTokenAndEmail(string token)
    {

        var claims = _tokenService.ValidateToken(token);

        if (claims == null) return (false, "Invalid token.");

        var userEmail = claims.FindFirst(ClaimTypes.Email)?.Value;

        if (userEmail == null) return (false, "Invalid email.");

        return (true, userEmail);
    }
}