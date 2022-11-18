using ElectronicShopper.DataAccess.StoredProcedures;

namespace ElectronicShopper.DataAccess.Data;

public interface ISqlDataAccess
{
    Task<IEnumerable<TResult>> LoadData<TSource, TResult>(TSource parameters) where TSource : IStoredProcedure;
    Task<TResult> SaveData<TSource, TResult>(TSource parameters) where TSource : IStoredProcedure;
    Task SaveData<TSource>(TSource parameters) where TSource : IStoredProcedure;
    void StartTransaction(string connectionString);
    void CommitTransaction();
    void RollbackTransaction();
    void Dispose();
}