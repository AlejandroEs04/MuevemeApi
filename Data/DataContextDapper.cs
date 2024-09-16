using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace MuevemeApi.Data;

class DataContextDapper 
{
    private readonly IConfiguration _config;
    public DataContextDapper(IConfiguration config)
    {
        _config = config;
    }

    public IEnumerable<T> FindMany<T>(string sql)
    {
        IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        return dbConnection.Query<T>(sql);
    }

    public T FindOne<T>(string sql)
    {
        IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        return dbConnection.QuerySingle<T>(sql);
    }

    public bool ExecuteSql(string sql)
    {
        IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        return dbConnection.Execute(sql) > 0;
    }

    public bool ExecuteSqlParameters(string sql, List<SqlParameter> parameters)
    {
        SqlCommand commandWithParams = new SqlCommand(sql);

        foreach(SqlParameter parameter in parameters)
        {
            commandWithParams.Parameters.Add(parameter);
        }

        SqlConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        dbConnection.Open();

        commandWithParams.Connection = dbConnection;

        int rowAffected = commandWithParams.ExecuteNonQuery();

        dbConnection.Close();

        return rowAffected > 0;
    }
}