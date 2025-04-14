using CountryAPP.Core.Contract.Persistence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CountryAPP.API.Persistence.Identity;

namespace CountryAPP.API.Persistence;

public class DataAccessHelper : IDataAccessHelper
{
    private readonly CountryDbContext _context;

    public DataAccessHelper(CountryDbContext context)
    {
        _context = context;
    }

    public async Task<List<T>> QueryData<T, U>(string storedProcedure, U parameters) where T : class
    {
        var props = parameters?.GetType().GetProperties();
        var placeholders = string.Join(", ", props.Select((_, index) => $"${index + 1}"));
        var sql = $"SELECT * FROM {storedProcedure}({placeholders})";
        var values = props?.Select(p => p.GetValue(parameters) ?? DBNull.Value).ToArray();

        return await _context.Set<T>().FromSqlRaw(sql, values).ToListAsync();
    }

    public async Task<int> ExecuteData<T>(string storedProcedure, T parameters)
    {
        var props = parameters?.GetType().GetProperties();
        var placeholders = string.Join(", ", props.Select((_, index) => $"${index + 1}"));
        var sql = $"CALL {storedProcedure}({placeholders})";
        var values = props?.Select(p => p.GetValue(parameters) ?? DBNull.Value).ToArray();

        return await _context.Database.ExecuteSqlRawAsync(sql, values);
    }
}
