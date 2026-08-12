namespace App.Security;

public class UserLoginAttempts
{
    public string username { get; set; } // Adiciona o ID do usuário
    public int AccessFailedCount { get; set; } = 0;
    public DateTime? LockoutEnd { get; set; } = null;

    
    public void RegisterFailedAttempt()
    {
        AccessFailedCount++;
        LockoutEnd = null; 
    }

   
    public bool IsLockedOut()
    {
        return AccessFailedCount >= 5 && LockoutEnd > DateTime.UtcNow;
    }

    
    public void UnlockUser()
    {
        AccessFailedCount = 0; 
        LockoutEnd = null; 
    }

    
    public void LockUser(TimeSpan duration)
    {
        LockoutEnd = DateTime.UtcNow.Add(duration); // Define a data de bloqueio
    }
    
    public bool IsLockedOutExpired()
    {
        return LockoutEnd.HasValue && LockoutEnd.Value <= DateTime.UtcNow;
    }
}
