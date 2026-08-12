namespace dddnet8.Domain.Patients.DTO;

/// <summary>
///     DTO for representing contact information of a patient or staff member.
/// </summary>
public class ContactInfoDto
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ContactInfoDto" /> class.
    /// </summary>
    /// <param name="phoneNumber">The phone number associated with the contact.</param>
    /// <param name="emailAddress">The email address associated with the contact.</param>
    /// <exception cref="ArgumentNullException">Thrown if phoneNumber or emailAddress is null or empty.</exception>
    public ContactInfoDto(string phoneNumber, string emailAddress)
    {
        PhoneNumber = phoneNumber;
        EmailAddress = emailAddress;
    }

    /// <summary>
    ///     Gets the phone number associated with the contact.
    /// </summary>
    public string PhoneNumber { get; init; }

    /// <summary>
    ///     Gets the email address associated with the contact.
    /// </summary>
    public string EmailAddress { get; init; }
}