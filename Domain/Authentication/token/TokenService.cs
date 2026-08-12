using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web;
using App.Domain.SystemUser;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using SurgicalManagement.Domain.Domain;


namespace dddnet8.Domain.Authentication.token
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public string GenerateJwtToken(SystemUserDto userDto){
            var claims = UserClaimsPrincipal(userDto);
            
            return GenerateToken(claims, DateTime.Now.AddMinutes(int.Parse(_configuration["JwtSettings:TokenExpirationMinutes"])));
        }

        private List<Claim> UserClaimsPrincipal(SystemUserDto userDto)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userDto.Username),
                new Claim(ClaimTypes.Role, userDto.Role),
                new Claim(ClaimTypes.Email, userDto.EmailAddress),
                
            };
            return claims;
        }

        public string GenerateResetToken(SystemUserDto userDto)
        {
            var claims = UserClaimsPrincipal(userDto);
            claims.Add(new Claim("jti", Guid.NewGuid().ToString()));
            
            return GenerateToken(claims, DateTime.UtcNow.AddHours(24));
        }

        public string GenerateToken(IEnumerable<Claim> claims, DateTime expiration, DateTime? notBefore = null)
        {
            // Read JWT settings from configuration
            var secretKey = _configuration["JwtSettings:SecretKey"];
            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var notBeforeTime = notBefore ?? DateTime.UtcNow.AddSeconds(-5);

            if (expiration <= notBeforeTime)
            {
                throw new ArgumentException("Expiration time must be after NotBefore time.", nameof(expiration));
            }
            
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: notBeforeTime,
                expires: expiration, // Use the provided expiration
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token); // Return the token as a string
        }

        public string GenerateJwtTokenForPatient(AuthenticateResult result)
        {
            if (result.Principal == null)
            {
                throw new InvalidOperationException("Cannot generate token for unauthenticated user.");
            }

            var claimsIdentity = (ClaimsIdentity)result.Principal.Identity;

            var role = UserRole.Patient.ToString();

            // Criando claims para o paciente
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, claimsIdentity.FindFirst(ClaimTypes.Name)?.Value),
                new Claim(ClaimTypes.Email, claimsIdentity.FindFirst(ClaimTypes.Email)?.Value),
                new Claim("UserId", claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                new Claim(ClaimTypes.Role, role) // Adicionando a claim de Role
            };

            return GenerateToken(claims, DateTime.Now.AddMinutes(int.Parse(_configuration["JwtSettings:TokenExpirationMinutes"])));
        }
        
        public string ExtractTokenFromURL(string token)
        {
            var uri = new Uri(token);
            var query = HttpUtility.ParseQueryString(uri.Query);

        
            var extractedToken = query.Get("token");
        
            Console.WriteLine(extractedToken);

            return extractedToken;

        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = _configuration["JwtSettings:SecretKey"];
            var key = Encoding.UTF8.GetBytes(secretKey);
            try
            {
                return tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _configuration["JwtSettings:Issuer"],
                    ValidAudience = _configuration["JwtSettings:Audience"],
                    ClockSkew = TimeSpan.Zero // No time skew
                }, out SecurityToken validatedToken);
            }
            catch (SecurityTokenExpiredException)
            {
                throw new SecurityTokenException("Token has expired.");
            }
            catch (Exception ex)
            {
                throw new SecurityTokenException("Invalid token.", ex);
            }
        }
        
        

        public string GetTokenFromHeader()
        {
            
            var httpContext = _httpContextAccessor.HttpContext;
            
            return httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        }
    }
}
