using ElectronicShopper.Library.StoredProcedures;

namespace ElectronicShopper.Library.Data;

/// <summary>
///     Provides access to SQL database.
/// </summary>
public interface ISqlDataAccess
{
    /// <summary>
    ///     Executes stored procedure associated with retrieving data.
    /// </summary>
    /// <param name="parameters">The parameters to use for this query.</param>
    /// <typeparam name="TSource">The type to stored procedure.</typeparam>
    /// <typeparam name="TResult">The type to return.</typeparam>
    /// <returns>A collection of <typeparamref name="TResult" /> retrieved from the database.</returns>
    Task<IEnumerable<TResult>> LoadData<TSource, TResult>(TSource parameters) where TSource : IStoredProcedure;


    /// <summary>
    ///     Executes the stored procedure associated with saving data.
    /// </summary>
    /// <param name="parameters">The parameters to use for this query.</param>
    /// <typeparam name="TSource">The type to stored procedure.</typeparam>
    /// <typeparam name="TResult">The type to return.</typeparam>
    Task<TResult> SaveData<TSource, TResult>(TSource parameters) where TSource : IStoredProcedure;

    /// <summary>
    ///     Executes the stored procedure associated with saving data.
    /// </summary>
    /// <param name="parameters">The parameters to use for this query.</param>
    /// <typeparam name="TSource">The type to stored procedure.</typeparam>
    Task SaveData<TSource>(TSource parameters) where TSource : IStoredProcedure;


    /// <summary>
    ///     Begins a database transaction.
    /// </summary>
    /// <param name="connectionString">The connection used to open the SQL database.</param>
    void StartTransaction(string connectionString);

    /// <summary>
    ///     Commits the database transaction and closes connection.
    /// </summary>
    void CommitTransaction();

    /// <summary>
    ///     Rolls back a transaction from a pending state and closes connection.
    /// </summary>
    void RollbackTransaction();

    /// <summary>
    ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    void Dispose();
}