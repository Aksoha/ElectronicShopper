namespace ElectronicShopper.DataAccess.Data;

public interface ISqlDataAccess
{
    Task<List<T>> LoadData<T, U>(string storedProcedure, U parameters);
    Task<U> SaveData<T, U>(string storedProcedure, T parameters);
    Task SaveData<T>(string storedProcedure, T parameters);
    void StartTransaction(string connectionStringName);
    void CommitTransaction();
    void RollbackTransaction();
    void Dispose();
}