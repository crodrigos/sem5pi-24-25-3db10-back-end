using System;

namespace dddnet8.Domain.SystemUsers
{
    
    public class InvalidUsernameException : Exception
    {
        public InvalidUsernameException() 
            : base("The username provided is invalid.")
        {
        }

        public InvalidUsernameException(string message) 
            : base(message)
        {
        }

        public InvalidUsernameException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }

    
    public class PasswordException : Exception
    {
        public PasswordException() 
            : base("The password provided is too weak.")
        {
        }

        public PasswordException(string message) 
            : base(message)
        {
        }

        public PasswordException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }

    // Exceção para contas já ativas
    public class AccountAlreadyActiveException : Exception
    {
        public AccountAlreadyActiveException() 
            : base("The account is already active.")
        {
        }

        public AccountAlreadyActiveException(string message) 
            : base(message)
        {
        }

        public AccountAlreadyActiveException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
    
    public class EmailAddressException : Exception
    {
        public EmailAddressException() 
            : base("The email address provided is invalid.")
        {
        }

        public EmailAddressException(string message) 
            : base(message)
        {
        }

        public EmailAddressException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}