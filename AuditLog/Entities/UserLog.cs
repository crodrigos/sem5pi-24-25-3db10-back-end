using dddnet8.Domain.SystemUsers;
using SurgicalManagement.Domain.Domain;
using YourNamespace.GDPR.Entities;

namespace dddnet8.AuditLog.Entities;

public class UserLog : LogEntry
{
    protected UserLog() : base("action", "entitytype") {}
    
    public UserLog(string action, string entityType, Guid id, EmailAddress username, string password, DateTime createdOn, UserRole role, EmailAddress emailAddress) : base(action, entityType)
    {
        Id = id;
        Username = username;
        Password = password;
        CreatedOn = createdOn;
        Role = role;
        EmailAddress = emailAddress;
    }
    
    /// <summary>
    /// Gets the unique identifier of the system user.
    /// </summary>
    public Guid Id { get; protected set; }

    /// <summary>
    /// Gets the username of the system user.
    /// </summary>
    public EmailAddress Username { get; private set; }

    /// <summary>
    /// The encoded password of the system user.
    /// </summary>
    public string Password;

    /// <summary>
    /// Gets the date and time when the user was created.
    /// </summary>
    public DateTime CreatedOn { get; private set; }

    /// <summary>
    /// Gets the role of the system user.
    /// </summary>
    public UserRole Role { get; private set; }

    /// <summary>
    /// Gets the email address of the system user.
    /// </summary>
    public EmailAddress EmailAddress { get; private set; }
}