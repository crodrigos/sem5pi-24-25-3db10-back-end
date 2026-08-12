namespace App.Security;

public interface ILoginAttemptsService
{

    void RegisterFailedAttempt(string usernameId);

    bool IsUserLockedOut(string userId);

    void ResetLoginAttempts(string userId);
    
    int GetRemainingAttempts(string userId);
}
