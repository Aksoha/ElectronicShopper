using System.Data;
using Dapper;
using ElectronicShopper.DataAccess.StoredProcedures;
using Microsoft.Data.SqlClient;

namespace ElectronicShopper.DataAccess.Data;

public class SqlDataAccess : IDisposable, ISqlDataAccess
{
    private IDbConnection? _connection;
    private IDbTransaction? _transaction;
    private bool _isClosed;
    
    public async Task<IEnumerable<TResult>> LoadData<TSource, TResult>(TSource parameters) where TSource : IStoredProcedure
    {
        var rows = await _connection.QueryAsync<TResult>(parameters.ProcedureName(), parameters, transaction: _transaction,
            commandType: CommandType.StoredProcedure);
        return rows;
    }
    
    public async Task<TResult> SaveData<TSource, TResult>(TSource parameters)  where TSource : IStoredProcedure
    {
        var result = await _connection.QuerySingleOrDefaultAsync<TResult>(parameters.ProcedureName(), parameters,
            transaction: _transaction,
            commandType: CommandType.StoredProcedure);
        return result;
    }
    
    public async Task SaveData<TSource>(TSource parameters) where TSource : IStoredProcedure
    {
        var result = await _connection.ExecuteAsync(parameters.ProcedureName(), parameters, transaction: _transaction,
            commandType: CommandType.StoredProcedure);
    }
    
    public void StartTransaction(string connectionString)
    {
        _connection = new SqlConnection(connectionString);
        _connection.Open();
        _transaction = _connection.BeginTransaction();

        _isClosed = false;
    }

    public void CommitTransaction()
    {
        _transaction?.Commit();
        _connection?.Close();
        _isClosed = true;
    }

    public void RollbackTransaction()
    {
        _transaction?.Rollback();
        _connection?.Close();
        _isClosed = true;
    }

    public void Dispose()
    {
        if (_isClosed == false)
        {
            CommitTransaction();
        }

        _transaction = null;
        _connection = null;
    }
}