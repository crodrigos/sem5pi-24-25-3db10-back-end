namespace App.Domain.SystemUser;

public class SystemUserDto
{
    public string Username { get; set; }
    public string Role { get; set; }
    
    public string EmailAddress { get; set; }


    public SystemUserDto(string username, string emailAddress, string role)
    {
        Username = username;
        EmailAddress = emailAddress;
        Role = role;
    }
}