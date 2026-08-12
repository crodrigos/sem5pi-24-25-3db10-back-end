using System;

namespace dddnet8.Infraestructure.Shared.Exceptions;

public class MultipleArgumentException : Exception
{

    public string[] _messages { get; private set; }

    public MultipleArgumentException(string[] messages) : base(string.Join(", ", messages))
    {
        _messages = messages;
    }
    

}
