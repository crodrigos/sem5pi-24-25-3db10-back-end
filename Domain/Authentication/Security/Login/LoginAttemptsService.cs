using System.Collections.Concurrent;
using App.Security;


public class LoginAttemptsService : ILoginAttemptsService
{
    private readonly ConcurrentDictionary<string, UserLoginAttempts> _loginAttempts = new();
    private const int MaxAttempts = 5;
    private readonly TimeSpan _lockoutDuration = TimeSpan.FromMinutes(1);

    public void RegisterFailedAttempt(string usernameId)
    {
        var attempts = _loginAttempts.GetOrAdd(usernameId, new UserLoginAttempts { username = usernameId });

        attempts.RegisterFailedAttempt(); // Incrementa a contagem de tentativas falhadas
        
        if (attempts.AccessFailedCount >= MaxAttempts)
        {
            attempts.LockUser(_lockoutDuration); // Bloqueia o usuário
        }
    }

    public int GetRemainingAttempts(string userId)
    {
        if (_loginAttempts.TryGetValue(userId, out var attempts))
        {
            return MaxAttempts - attempts.AccessFailedCount; // Calcula tentativas restantes
        }
        return MaxAttempts; 
    }

    public bool IsUserLockedOut(string userId)
    {
        // Tenta obter as tentativas de login do usuário
        if (_loginAttempts.TryGetValue(userId, out var attempts))
        {
            if (attempts.IsLockedOutExpired())
            {
                attempts.UnlockUser(); // Reseta as tentativas para 0 (ou para o valor máximo que você deseja)
                return false; // O usuário não está mais bloqueado
            }

            return attempts.IsLockedOut(); // Retorna se o usuário está bloqueado
        }

        return false; // Se não houver registros, o usuário não está bloqueado
    }

    public void ResetLoginAttempts(string userId)
    {
        _loginAttempts.TryRemove(userId, out _); // Limpa tentativas após login bem-sucedido
    }
}