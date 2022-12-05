namespace System.Diagnostics;

// TODO: when moving to .NET 7 replace with exception with System.Diagnostics.UnreachableException.
/// <summary>
/// The exception that is thrown when the program executes an instruction that was thought to be unreachable.
/// </summary>
public class UnreachableException : Exception
{
    public UnreachableException()
    {
        
    }

    public UnreachableException(string? message) : base(message)
    {
        
    }

    public UnreachableException(string? message, Exception? innerException) : base(message, innerException)
    {
        
    }
}