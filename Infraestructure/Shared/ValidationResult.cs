using System;

namespace dddnet8.Infraestructure.Shared;

public class ValidationResult
{
    public ValidationResult()
    {
        errorMessages = new List<string>();
        IsValid = true;
    }

    public bool IsValid { get; private set; }

    private List<string> errorMessages;

    public IEnumerable<string> ErrorMessages
    {
        get { return errorMessages; }
    }

    public void AddErrorMessage(string errorMessage)
    {
        IsValid = false;
        errorMessages.Add(errorMessage);
    }
}
