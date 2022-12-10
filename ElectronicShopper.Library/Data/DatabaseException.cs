namespace ElectronicShopper.Library.Data;

/// <summary>
///     Represents an error that occur during database transaction.
/// </summary>
public class DatabaseException : Exception
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="DatabaseException" /> class.
    /// </summary>
    internal DatabaseException()
    {
    }

    /// <inheritdoc cref="DatabaseException()" />
    /// <param name="message">The error message that explains the reason for the exception.</param>
    internal DatabaseException(string message) : base(message)
    {
    }

    /// <inheritdoc cref="DatabaseException()" />
    /// <param name="message"> The error message that explains the reason for the exception.</param>
    /// <param name="inner">The exception that is the cause of the current exception.</param>
    internal DatabaseException(string message, Exception inner) : base(message, inner)
    {
    }
}