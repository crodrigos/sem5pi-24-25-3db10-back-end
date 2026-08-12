using App.Passsword.Encoder;
using App.Password.Generator;
using dddnet8.Domain.BackOfficeEmail;
using dddnet8.Domain.SystemUsers;
using SurgicalManagement.Domain.Domain;

public class SystemUserBuilder
{
    private EmailAddress username;
    private EmailAddress email;
    private UserRole role;
    private string password;

    private readonly IPasswordEncoder passwordEncoder;
    private readonly IPasswordGenerator passwordGenerator;
    private readonly IBackOfficeEmailGenerator backOfficeEmailGenerator;
    public SystemUserBuilder(IPasswordEncoder passwordEncoder, IPasswordGenerator passwordGenerator, IBackOfficeEmailGenerator backOfficeEmailGenerator)
    {
        this.passwordEncoder = passwordEncoder ?? throw new ArgumentNullException(nameof(passwordEncoder));
        this.passwordGenerator = passwordGenerator ?? throw new ArgumentNullException(nameof(passwordGenerator));
        this.backOfficeEmailGenerator = backOfficeEmailGenerator ?? throw new ArgumentNullException(nameof(backOfficeEmailGenerator));
    }

    public SystemUserBuilder GeneratedPassword()
    {
        password = passwordGenerator.GeneratePassword();
        return this;
    }

    public SystemUserBuilder WithUsername(EmailAddress username)
    {
        this.username = username;
        return this;
    }

    public SystemUserBuilder WithEmail(EmailAddress email)
    {
        this.email = email;
        return this;
    }

    public SystemUserBuilder WithRole(UserRole role)
    {
        this.role = role;
        return this;
    }
    
    public SystemUserBuilder WithUsernameAsStaffEmail(string licenseNumber)
    {
        var staffEmail = backOfficeEmailGenerator.GenerateStaffEmail(licenseNumber);
        username = staffEmail;
        return this;
    }


    public SystemUser Build()
    {
        var encodedPassword = passwordEncoder.Encode(password); // Codifica a senha antes de criar o usuário
        return new SystemUser(username, email, role, encodedPassword);
    }
}