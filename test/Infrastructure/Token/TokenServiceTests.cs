using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Http;
using dddnet8.Domain.Authentication.token;
using App.Domain.SystemUser;

namespace TokenServiceTests
{
    [TestFixture]
    public class TokenServiceUnitTests
    {
        private Mock<IConfiguration> _configurationMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private TokenService _tokenService;
        private SystemUserDto _testUserDto;

        [SetUp]
        public void SetUp()
        {
            // Initializes the mocks for IConfiguration and IHttpContextAccessor
            _configurationMock = new Mock<IConfiguration>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            // Sets up the mocked JWT settings from appsettings.json
            _configurationMock.Setup(c => c["JwtSettings:SecretKey"]).Returns("YourSecretKeyForTestingPurposes12345");
            _configurationMock.Setup(c => c["JwtSettings:TokenExpirationMinutes"]).Returns("1");
            _configurationMock.Setup(c => c["JwtSettings:Issuer"]).Returns("TestIssuer");
            _configurationMock.Setup(c => c["JwtSettings:Audience"]).Returns("TestAudience");

            // Initializes the TokenService with the mocked configurations
            _tokenService = new TokenService(_configurationMock.Object, _httpContextAccessorMock.Object);

            // Creates a test user for validation purposes
            _testUserDto = new SystemUserDto("TestUser", "testuser@example.com", "Admin");
        }

        /// <summary>
        /// Tests that the GenerateJwtToken method returns a valid JWT token for a given user.
        /// </summary>
        [Test]
        public void GenerateJwtToken_ShouldReturn_ValidToken()
        {
            // Act
            var token = _tokenService.GenerateJwtToken(_testUserDto);

            // Assert
            Assert.That(token, Is.Not.Null, "The generated token should not be null");

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            // Validates that the token contains the correct claims
            Assert.That(jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Name).Value, Is.EqualTo(_testUserDto.Username));
            Assert.That(jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Role).Value, Is.EqualTo(_testUserDto.Role));
            Assert.That(jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Email).Value, Is.EqualTo(_testUserDto.EmailAddress));
        }

        /// <summary>
        /// Tests that the ValidateToken method returns a valid ClaimsPrincipal when a valid token is provided.
        /// </summary>
        [Test]
        public void ValidateToken_WithValidToken_ShouldReturn_ValidClaimsPrincipal()
        {
            // Arrange
            var token = _tokenService.GenerateJwtToken(_testUserDto);

            // Act
            var claimsPrincipal = _tokenService.ValidateToken(token);

            // Assert
            Assert.That(claimsPrincipal, Is.Not.Null, "ClaimsPrincipal should not be null");
            Assert.That(claimsPrincipal.Identity.Name, Is.EqualTo(_testUserDto.Username));
            Assert.That(claimsPrincipal.IsInRole(_testUserDto.Role), Is.True, "ClaimsPrincipal should have the correct role");
        }

        /// <summary>
        /// Tests that the ValidateToken method throws a SecurityTokenException when an expired token is provided.
        /// </summary>
        [Test]
        public void ValidateToken_WithExpiredToken_ShouldThrowSecurityTokenExpiredException()
        {
            // Arrange
            _configurationMock.Setup(c => c["JwtSettings:TokenExpirationMinutes"]).Returns("0"); // Token expires immediately
            var expiredTokenService = new TokenService(_configurationMock.Object, _httpContextAccessorMock.Object);
            var expiredToken = expiredTokenService.GenerateJwtToken(_testUserDto);

            // Act & Assert
            Assert.That(() => expiredTokenService.ValidateToken(expiredToken),
                Throws.TypeOf<SecurityTokenException>().With.Message.EqualTo("Token has expired."));
        }

        /// <summary>
        /// Tests that the GetTokenFromHeader method correctly retrieves the token from the Authorization header.
        /// </summary>
        [Test]
        public void GetTokenFromHeader_ShouldReturn_TokenFromAuthorizationHeader()
        {
            // Arrange
            var expectedToken = "Bearer testtoken123";
            var httpContextMock = new DefaultHttpContext();
            httpContextMock.Request.Headers["Authorization"] = expectedToken;
            _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(httpContextMock);

            // Act
            var result = _tokenService.GetTokenFromHeader();

            // Assert
            Assert.That(result, Is.EqualTo("testtoken123"), "The extracted token should match the token in the Authorization header");
        }

        /// <summary>
        /// Tests that the GetTokenFromHeader method returns an empty string when no token is present in the header.
        /// </summary>
        [Test]
        public void GetTokenFromHeader_NoTokenInHeader_ShouldReturn_EmptyString()
        {
            // Arrange
            var httpContextMock = new DefaultHttpContext();
            _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(httpContextMock);

            // Act
            var result = _tokenService.GetTokenFromHeader();

            // Assert
            Assert.That(result, Is.Empty, "If there is no token in the Authorization header, the result should be an empty string");
        }

        [Test]
        public void GenerateResetToken_ShouldExpireAfter24Hours()
        {
            // Arrange
            var userDto = new SystemUserDto("testuser@example.com", "testuser@example.com", "Admin");

            _configurationMock.Setup(x => x["JwtSettings:SecretKey"]).Returns("this_is_a_secret_key_for_testing");
            _configurationMock.Setup(x => x["JwtSettings:Issuer"]).Returns("testIssuer");
            _configurationMock.Setup(x => x["JwtSettings:Audience"]).Returns("testAudience");

            // Act
            var token = _tokenService.GenerateResetToken(userDto);

            // Simulate the passage of time by directly setting the current time (not ideal but illustrative)
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            // Simulating a 25-hour wait
            var expirationTime = jwtToken.ValidTo; // This should be the expiration time set in the token
            Assert.That(expirationTime, Is.EqualTo(DateTime.UtcNow.AddHours(24)).Within(TimeSpan.FromMinutes(1)));
        }

        [Test]
        public void ValidateResetToken_ShouldThrowException_WhenTokenIsExpired()
        {
            // Arrange
            var userDto = new SystemUserDto("testuser@example.com", "testuser@example.com", "Admin");

            // Generate claims for the user
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userDto.Username.ToString()), // Assuming Id is defined in SystemUserDto
                new Claim("type", "reset"),
                new Claim(ClaimTypes.Email, userDto.EmailAddress)
            };

            // Simulate the generation of a token with an expiration in the past
            var expiration = DateTime.UtcNow.AddHours(-24); // Set expiration to 24 hours ago
            var notBefore = DateTime.UtcNow.AddHours(-25);
            var token = _tokenService.GenerateToken(claims, expiration, notBefore); // Use the existing GenerateToken method

            // Act & Assert
            var ex = Assert.Throws<SecurityTokenException>(() => _tokenService.ValidateToken(token));
            Assert.That(ex.Message, Is.EqualTo("Token has expired."));
        }
    }
}

    

