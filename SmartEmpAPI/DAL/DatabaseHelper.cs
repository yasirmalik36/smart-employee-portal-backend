

using Microsoft.Data.SqlClient;
using SmartEmpAPI.DTOs;
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
        public Response ExecuteSPResponse(string procedureName, params SqlParameter[] inputParameters)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand(procedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add input parameters
                        if (inputParameters != null)
                            command.Parameters.AddRange(inputParameters);

                        // Auto-add standard output parameters if not already included
                        var outputParams = new Dictionary<string, SqlParameter>
                {
                    { "@Message", new SqlParameter("@Message", SqlDbType.NVarChar, 100) { Direction = ParameterDirection.Output } },
                    { "@Code", new SqlParameter("@Code", SqlDbType.NVarChar, 2) { Direction = ParameterDirection.Output } },
                    { "@Description", new SqlParameter("@Description", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output } }
                };

                        foreach (var param in outputParams)
                        {
                            if (!command.Parameters.Contains(param.Key))
                                command.Parameters.Add(param.Value);
                        }

                        // Execute SP
                        command.ExecuteNonQuery();

                        // Retrieve output values
                        var response = new Response
                        {
                            Message = command.Parameters["@Message"].Value != DBNull.Value ? command.Parameters["@Message"].Value.ToString() : null,
                            Code = command.Parameters["@Code"].Value != DBNull.Value ? command.Parameters["@Code"].Value.ToString() : null,
                            Description = command.Parameters["@Description"].Value != DBNull.Value ? command.Parameters["@Description"].Value.ToString() : null
                        };

                        return response;
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

        public (DataSet, Dictionary<string, object>) ExecuteStoredProcedurewithOutput(string procedureName, params SqlParameter[] parameters)
        {
         
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand(procedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        if (parameters != null)
                            command.Parameters.AddRange(parameters);

                        // Identify output parameters
                        var outputParams = new Dictionary<string, object>();
                        foreach (SqlParameter param in command.Parameters)
                        {
                            if (param.Direction == ParameterDirection.Output || param.Direction == ParameterDirection.InputOutput)
                            {
                                outputParams[param.ParameterName] = null; // Placeholder
                            }
                        }

                        // Execute and fill DataSet
                        var dataSet = new DataSet();
                        using (var dataAdapter = new SqlDataAdapter(command))
                        {
                            dataAdapter.Fill(dataSet);
                        }

                        // Retrieve output parameters' values
                        foreach (var paramName in outputParams.Keys.ToList())
                        {
                            outputParams[paramName] = command.Parameters[paramName].Value;
                        }

                        return (dataSet, outputParams);
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
        public (DataSet, Response) ExecuteSPWithGenericOutput(string procedureName, params SqlParameter[] inputParameters)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand(procedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add input parameters
                        if (inputParameters != null)
                            command.Parameters.AddRange(inputParameters);

                        // Define and add standard output parameters
                        var outputParams = new Dictionary<string, SqlParameter>
                {
                    { "@TotalRecords", new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output } },
                    { "@Message", new SqlParameter("@Message", SqlDbType.NVarChar, 100) { Direction = ParameterDirection.Output } },
                    { "@Code", new SqlParameter("@Code", SqlDbType.NVarChar, 2) { Direction = ParameterDirection.Output } },
                    { "@Description", new SqlParameter("@Description", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output } }
                };

                        foreach (var param in outputParams.Values)
                        {
                            command.Parameters.Add(param);
                        }

                        // Execute and fill DataSet
                        var dataSet = new DataSet();
                        using (var dataAdapter = new SqlDataAdapter(command))
                        {
                            dataAdapter.Fill(dataSet);
                        }

                        // Retrieve output values
                        var response = new Response
                        {
                            TotalRecords = outputParams["@TotalRecords"].Value != DBNull.Value ? outputParams["@TotalRecords"].Value.ToString() : "0",
                            Message = outputParams["@Message"].Value?.ToString(),
                            Code = outputParams["@Code"].Value?.ToString(),
                            Description = outputParams["@Description"].Value?.ToString()
                        };

                        return (dataSet, response);
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
