using System.Security.Claims;
using App.Domain.SystemUser;
using App.Passsword.Encoder;
using App.PassswordPolicy;
using App.Password.Generator;
using App.SystemUserStuff;
using dddnet8.AuditLog.Interfaces;
using dddnet8.Domain.Authentication.token;
using dddnet8.Domain.BackOfficeEmail;
using dddnet8.Domain.Staffs.Interfaces;
using dddnet8.Domain.SystemUsers.DTO;
using dddnet8.Infraestructure.Email;
using SurgicalManagement.Domain.Domain;

namespace dddnet8.Domain.SystemUsers;

public class SystemUserService : ISystemUserService
{
    private readonly IEmailService _emailService;
    private readonly IBackOfficeEmailGenerator _officeEmailGenerator;
    private readonly IPasswordEncoder _passwordEncoder;
    private readonly IPasswordGenerator _passwordGenerator;
    private readonly IPasswordPolicy _passwordPolicy;
    private readonly IStaffRepository _staffRepository;
    private readonly ITokenService _tokenService;
    private readonly ISystemUserRepository _userRepository;


    public SystemUserService(
        ISystemUserRepository userRepository,
        IEmailService emailService,
        ITokenService tokenService,
        IPasswordEncoder passwordEncoder,
        IPasswordPolicy passwordPolicy,
        IPasswordGenerator passwordGenerator,
        IBackOfficeEmailGenerator officeEmailGenerator,
        IStaffRepository staffRepository)
    {
        _userRepository = userRepository;
        _emailService = emailService;
        _tokenService = tokenService;
        _passwordEncoder = passwordEncoder;
        _passwordPolicy = passwordPolicy;
        _passwordGenerator = passwordGenerator;
        _officeEmailGenerator = officeEmailGenerator;
        _staffRepository = staffRepository;
    }

    public async Task<SystemUserDto> CreateUser(SystemUserRequestDto userRequestDto)
    {
        if (userRequestDto.Role.ToLower() == "patient")
            throw new InvalidOperationException("Admin can't create patient users'.");

        try
        {
            var user = CreateUserByRole(userRequestDto);

            var (userDto,token, errorMessage) = await StoreUserAndActivateAccountNotification(user);

           return userDto;
        }
        catch (InvalidOperationException ex)
        {
            throw new InvalidOperationException(ex.Message);
        }
        catch (Exception ex)
        {
            throw new Exception("Um erro inesperado ocorreu ao criar o usuário.", ex);
        }
    }

    private async Task<(SystemUserDto systemUserDto, string token, string errorMessage)> StoreUserAndActivateAccountNotification(SystemUser user)
    {
        
        await _userRepository.AddUserAsync(user);
        
        var token = _tokenService.GenerateJwtToken(SystemUserMapper.ToDto(user));

        await _emailService.ActivationAccount(user.EmailAddress.ToString(), token);

        return (SystemUserMapper.ToDto(user), token, "");
    }


    public async Task<SystemUser> GetUserByUsername(string userEmailAddress)
    {
        var patient = await _userRepository.GetUserByUsernameAsync(EmailAddress.Create(userEmailAddress));
        if (patient == null) throw new KeyNotFoundException("Patient not found");

        return patient;
    }

    public async Task<IEnumerable<SystemUser>> GetUsersByRole(UserRole role)
    {
        return await _userRepository.GetUsersByRoleAsync(role); // Use um método assíncrono
    }

    public async Task ActivateUserAccount(string userEmail, string password)
    {
        if (string.IsNullOrEmpty(userEmail))
            throw new EmailAddressException("O endereço de e-mail não pode ser nulo ou vazio.");

        if (!_passwordPolicy.isSatisfiedBy(password))
            throw new PasswordException("A senha não corresponde à política de senha.");

        var user = await _userRepository.GetUserByEmailAsync(
            EmailAddress.Create(userEmail)); // Use um método assíncrono

        if (user == null) throw new ArgumentException("User not found.");

        user.ActivateAccount();

        await ResetUserPassword(user, password); // Usando um método assíncrono
    }

    public async Task<SystemUser> GetUserByEmail(string userEmail)
    {
        try
        {
            var user = await _userRepository.GetUserByEmailAsync(EmailAddress.Create(userEmail));

            if (user == null) throw new InvalidOperationException("User does not exist.");

            return user;
        }
        catch (InvalidOperationException ex) when (ex.Message == "User does not exist.")
        {
            // Re-lança a exceção de "User does not exist." diretamente sem alterar a mensagem
            throw;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while retrieving the user.", ex);
        }
    }

    public async Task ResetUserPassword(SystemUser user, string password)
    {
        var hashedPassword = _passwordEncoder.Encode(password);

        user.ResetPassword(hashedPassword);

        await _userRepository.ActivateUserAccountAsync(user); // Use um método assíncrono para ativar a conta
    }

   
    public async Task<(bool IsSuccess, string Message)> DeleteUser(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return (false, "Username cannot be null or empty.");
        }

        try
        {
            var user = await _userRepository.GetUserByUsernameAsync(EmailAddress.Create(username));
            if (user == null)
            {
                return (false, "User not found.");
            }
            
            await _userRepository.RemoveUserAsync(user);
            
            return (true, "User deleted successfully.");
        }
        catch (ArgumentException argEx)
        {
            return (false, $"Invalid username: {argEx.Message}");
        }
        catch (Exception ex)
        {
            return (false, $"An unexpected error occurred while deleting the user: {ex.Message}");
        }
    }

    public async Task<(SystemUserDto userDto, string token , string errorMessage)> CreateUserFromIAM(ClaimsPrincipal claimsPrincipal)
    {
        var email = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentException("Email cannot be null or empty."); 
        }
        
        var userEmail = await _userRepository.GetUserByEmailAsync(EmailAddress.Create(email));

        if (userEmail != null)
        {
            
            return (null, null, "There is already an account with this email."); 
        }

        var role = claimsPrincipal.FindFirst(ClaimTypes.Role)?.Value;

        var systemUserDto = new SystemUserRequestDto() { EmailAddress = email, Role = role };

        var user = CreateUserByRole(systemUserDto);

        return await StoreUserAndActivateAccountNotification(user);
    }


    private SystemUser CreateUserByRole(SystemUserRequestDto userRequestDto)
    {
        try
        {
            var userEmail = EmailAddress.Create(userRequestDto.EmailAddress);

            var role = Enum.Parse<UserRole>(userRequestDto.Role);

            if (role == UserRole.Patient)
                return new SystemUserBuilder(_passwordEncoder, _passwordGenerator, _officeEmailGenerator)
                    .WithUsername(userEmail)
                    .WithEmail(userEmail)
                    .WithRole(role)
                    .GeneratedPassword()
                    .Build();

            var licenseNumber = _staffRepository.GetLicenseNumberByEmailAddressAsync(userEmail).Result;


            if (licenseNumber == null)
                throw new InvalidOperationException("License number could not be retrieved for the provided email.");

            return new SystemUserBuilder(_passwordEncoder, _passwordGenerator, _officeEmailGenerator)
                .WithUsernameAsStaffEmail(licenseNumber.ToString())
                .WithEmail(userEmail)
                .WithRole(role)
                .GeneratedPassword()
                .Build();
        }
        catch (ArgumentException ex)
        {
            throw new InvalidOperationException("Invalid role or email format provided.", ex);
        }
        catch (InvalidOperationException ex)
        {
            throw new InvalidOperationException(ex.Message, ex);
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred while creating the user.", ex);
        }
    }
    
    public async Task<(bool result, string message)> ValidateUserForDeletion(string email){
        try
        {
            var user = await GetUserByUsername(email);

            if (user == null) {return (false, "User not found.");}

            return user.DeletionStatus.IsToDelete ? (false, $"User is already marked for deletion.{user.DeletionStatus.DeletionDate.Value}") : (true, $"User {user.Username} is eligible for deletion. Are you sure you want to mark this user to be deleted?");
        }
        catch (Exception ex) {return (false, $"An error occurred: {ex.Message}");}
    }

    public async Task MarkUserForDeletion(string email)
    {
        try{
            var user = await _userRepository.GetUserByEmailAsync(EmailAddress.Create(email));

            if (user == null) throw new InvalidOperationException("User does not exist.");

            user.MarkForDeletion();

            await UpdateUserInRepository(user); 
        }
        catch (KeyNotFoundException ex) {throw new Exception("User not found.", ex);}
        catch (Exception ex) {throw new Exception("An error occurred while marking the user for deletion.", ex); }
    }
    
    private async Task UpdateUserInRepository(SystemUser user)
    {
        await _userRepository.UpdateUserDataAsync(user);
    }
}