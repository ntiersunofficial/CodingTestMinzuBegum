namespace CountryAPP.Core.Contract.Persistence;

public interface IDataAccessHelper
{
    Task<List<T>> QueryData<T, U>(string storedProcedure, U parameters) where T : class;
    Task<int> ExecuteData<T>(string storedProcedure, T parameters);
}