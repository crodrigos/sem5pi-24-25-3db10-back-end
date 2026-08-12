using System.Security.Claims;
using App.Domain.SystemUser;
using Microsoft.AspNetCore.Authentication;

namespace dddnet8.Domain.Authentication.token;

public interface ITokenService
{
    string GenerateJwtToken(SystemUserDto userDto);

    ClaimsPrincipal ValidateToken(string token);

    string ExtractTokenFromURL(string token);

    string GetTokenFromHeader();
    string GenerateResetToken(SystemUserDto userDto);

    //FOR TESTS PURPOSES
    string GenerateToken(IEnumerable<Claim> claims, DateTime expiration, DateTime? notBefore = null);
    string GenerateJwtTokenForPatient(AuthenticateResult result);
}