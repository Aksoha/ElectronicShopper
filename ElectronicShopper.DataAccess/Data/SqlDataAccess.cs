using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ElectronicShopper.DataAccess.Data;

public class SqlDataAccess : IDisposable, ISqlDataAccess
{
    private IDbConnection? _connection;
    private IDbTransaction? _transaction;
    private bool _isClosed;

    public async Task<List<T>> LoadData<T, U>(string storedProcedure, U parameters)
    {
        var rows = await _connection.QueryAsync<T>(storedProcedure, parameters, transaction: _transaction,
            commandType: CommandType.StoredProcedure);
        return rows.ToList();
    }

    public async Task<U> SaveData<T, U>(string storedProcedure, T parameters)
    {
        var result = await _connection.QuerySingleOrDefaultAsync<U>(storedProcedure, parameters,
            transaction: _transaction,
            commandType: CommandType.StoredProcedure);
        return result;
    }

    public async Task SaveData<T>(string storedProcedure, T parameters)
    {
        var result = await _connection.ExecuteAsync(storedProcedure, parameters, transaction: _transaction,
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