

namespace ElectronicShopper.DataAccess.Data;

public class DatabaseException : Exception
{
    internal DatabaseException()
    {
        
    }
    
    internal DatabaseException(string message) : base(message)
    {
        
    }
    
    internal DatabaseException(string message, Exception inner) : base(message, inner)
    {
        
    }
}