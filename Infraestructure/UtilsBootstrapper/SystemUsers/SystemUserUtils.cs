using App.Passsword.Encoder;
using App.Password.Generator;
using dddnet8.Domain.BackOfficeEmail;
using dddnet8.Domain.SystemUsers;
using SurgicalManagement.Domain.Domain;

namespace dddnet8.Infraestructure.UtilsBootstrapper.SystemUsers;

public class SystemUserUtils {
    
    private readonly IPasswordEncoder _passwordEncoder;
    private readonly IPasswordGenerator _passwordGenerator;
    private readonly ISystemUserRepository _systemUserRepository;
    private readonly IBackOfficeEmailGenerator _backOfficeEmailGenerator;
    
    


    public SystemUserUtils(IPasswordEncoder passwordEncoder, IPasswordGenerator passwordGenerator,
        ISystemUserRepository systemUserRepository, IBackOfficeEmailGenerator backOfficeEmailGenerator)
    {
        _passwordEncoder = passwordEncoder;
        _passwordGenerator = passwordGenerator;
        _systemUserRepository = systemUserRepository;
        _backOfficeEmailGenerator = backOfficeEmailGenerator;
        _systemUserRepository = systemUserRepository;
    }
    
    public async Task InitializeSystemUserAsync(){
        var users = await _systemUserRepository.GetAllAsync();

        if (!users.Any())
        {
            await SaveSystemUser(await CreateAndSaveUser("A0001@trelloHospital.com", UserRole.Admin, "AlejandroVieira1912@gmail.com" ));
            await SaveSystemUser(await CreateAndSaveUser("A0002@trelloHospital.com", UserRole.Admin, "joaopintojpgp@gmail.com" ));
            await SaveSystemUser(await CreateAndSaveUser("A0003@trelloHospital.com", UserRole.Admin, "undercoverspace18@gmail.com" ));
        } 
    }

    private async Task SaveSystemUser(SystemUser user)
    {
        await _systemUserRepository.AddUserAsync(user);
    }


    public async Task<SystemUser> CreateAndSaveUser(string staffEmail, UserRole role, string personalEmail)
    {
        var user = new SystemUserBuilder(_passwordEncoder, _passwordGenerator, _backOfficeEmailGenerator)
            .WithUsername(EmailAddress.Create(staffEmail))
            .WithEmail(new EmailAddress(personalEmail))
            .WithRole(role)
            .GeneratedPassword()
            .Build();

        user.ActivateAccount();
        user.ResetPassword(_passwordEncoder.Encode("Admin@2001"));

        if (!UserRole.Admin.Equals(role))
        {
            await _systemUserRepository.AddUserAsync(user);
        }

        return user;
    }
}