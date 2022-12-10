namespace ElectronicShopper.Library.StoredProcedures;


/// <summary>
/// Represents a stored procedure in a database.
/// </summary>
public interface IStoredProcedure
{
    
    /// <summary>
    /// Get a name of stored procedure.
    /// </summary>
    string ProcedureName();
}