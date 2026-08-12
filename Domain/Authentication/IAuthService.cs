using App.Domain.SystemUser;
using App.Security;
using App.SystemUser.Domain.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace dddnet8.Domain.Authentication;

public interface IAuthService { 
    Task<(string? token, string errorMessage, SystemUserDto? userDto)> Login(LoginDto loginDto);

    Task<(bool Success, string message)> ActivateUserAccount(PasswordDto passwordDto, string token);
    Task<(bool success, string message)> ResetUserPasswordRequest(string userDto);
    Task<(bool result, string message)> PasswordUserReset(string requestPassword, string token);
    Task<IActionResult> CreateAccountFromFacebook(string returnUrl);

    Task<(string redirect, string message)> FacebookResponse(AuthenticateResult result);
}