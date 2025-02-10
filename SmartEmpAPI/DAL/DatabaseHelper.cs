

using Microsoft.Data.SqlClient;
using System.Data;

namespace SmartEmpAPI.DAL
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public DataSet ExecuteStoredProcedure(string procedureName, params SqlParameter[] parameters)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open(); 

                    using (var command = new SqlCommand(procedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(parameters);

                        var dataSet = new DataSet();
                        var dataAdapter = new SqlDataAdapter(command);
                        dataAdapter.Fill(dataSet);

                        return dataSet;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Exception: {sqlEx.Message}");
                throw; 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                throw; 
            }
        }
        public DataSet ExecuteStoredProcedurewithoutParam(string procedureName)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand(procedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        var dataSet = new DataSet();
                        var dataAdapter = new SqlDataAdapter(command);
                        dataAdapter.Fill(dataSet);

                        return dataSet;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Exception: {sqlEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                throw;
            }
        }

    }
}
