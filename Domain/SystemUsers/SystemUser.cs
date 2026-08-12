using dddnet8.Domain.Shared;
using SurgicalManagement.Domain.Common;
using SurgicalManagement.Domain.Domain;

namespace dddnet8.Domain.SystemUsers;

/// <summary>
/// Represents a system user in the application.
/// </summary>
public class SystemUser : Entity<Guid>, IAggregateRoot
{
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
    /// Gets a value indicating whether the account is active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Gets the role of the system user.
    /// </summary>
    public UserRole Role { get; private set; }

    /// <summary>
    /// Gets the email address of the system user.
    /// </summary>
    public EmailAddress EmailAddress { get; private set; }

    /// <summary>
    /// Gets the deletion status of the system user.
    /// </summary>
    public DeletionStatus DeletionStatus { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SystemUser"/> class.
    /// </summary>
    /// <param name="username">The username of the system user.</param>
    /// <param name="emailAddress">The email address of the system user.</param>
    /// <param name="role">The role of the system user.</param>
    /// <param name="password">The password of the system user.</param>
    /// <param name="deletionStatus">Optional deletion status.</param>
    public SystemUser(EmailAddress username, EmailAddress emailAddress, UserRole role, string password, DeletionStatus? deletionStatus = null)
        : base(Guid.NewGuid())
    {
        Username = username ?? throw new ArgumentNullException(nameof(username), "Username cannot be null.");
        EmailAddress = emailAddress ?? throw new ArgumentNullException(nameof(emailAddress), "Email address cannot be null.");
        Password = password ?? throw new ArgumentNullException(nameof(password), "Password cannot be null.");
        Role = role;
        CreatedOn = DateTime.UtcNow;
        IsActive = false;
        DeletionStatus = deletionStatus == null ? DeletionStatus.Create(false) : deletionStatus;
    }

    // Parameterless constructor for ORM frameworks
    protected SystemUser() : base(Guid.NewGuid()) { }

    /// <summary>
    /// Changes the username of the system user.
    /// </summary>
    /// <param name="username">The new username.</param>
    /// <exception cref="InvalidUsernameException">Thrown when the username is null or empty.</exception>
    public void ChangeUsername(EmailAddress username)
    {
        if (username == null || string.IsNullOrWhiteSpace(username.ToString()))
        {
            throw new InvalidUsernameException("Username cannot be null or empty.");
        }

        Username = username;
    }

    /// <summary>
    /// Resets the password of the system user.
    /// </summary>
    /// <param name="encodedPassword">The new encoded password.</param>
    /// <exception cref="PasswordException">Thrown when the password is null or empty.</exception>
    public void ResetPassword(string encodedPassword)
    {
        if (string.IsNullOrWhiteSpace(encodedPassword))
        {
            throw new PasswordException("Password cannot be null or empty.");
        }

        Password = encodedPassword;
    }

    /// <summary>
    /// Activates the user's account.
    /// </summary>
    /// <exception cref="AccountAlreadyActiveException">Thrown when the account is already active.</exception>
    public void ActivateAccount()
    {
        if (IsActive)
        {
            throw new AccountAlreadyActiveException("Account is already active.");
        }

        IsActive = true;
    }

    /// <summary>
    /// Deactivates the user's account.
    /// </summary>
    public void DeactivateAccount()
    {
        IsActive = false;
    }

    /// <summary>
    /// Changes the user's role.
    /// </summary>
    /// <param name="newRole">The new user role.</param>
    public void ChangeRole(UserRole newRole)
    {
        Role = newRole;
    }

    /// <summary>
    /// Marks the user for deletion.
    /// </summary>
    public void MarkForDeletion()
    {
        DeletionStatus = DeletionStatus.Create(true, DateTime.UtcNow);
    }

    /// <summary>
    /// Determines whether the user can be deleted.
    /// </summary>
    /// <returns>True if the user can be deleted; otherwise, false.</returns>
    public bool CanDelete() 
    {
        return DeletionStatus.CanDelete();
    }
}
