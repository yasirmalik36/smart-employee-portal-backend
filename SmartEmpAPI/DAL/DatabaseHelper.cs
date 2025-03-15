

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
        //public (DataSet, Dictionary<string, object>) ExecuteStoredProcedureWithOutput(string procedureName, params SqlParameter[] parameters)
        //{
        //    try
        //    {
        //        using (var connection = new SqlConnection(_connectionString))
        //        {
        //            connection.Open();

        //            using (var command = new SqlCommand(procedureName, connection))
        //            {
        //                command.CommandType = CommandType.StoredProcedure;

        //                // Convert parameters to list to add output parameters
        //                var paramList = parameters.ToList();

        //                // Define output parameters
        //                var outputParams = new Dictionary<string, object>();
        //                var outputParameters = new[]
        //                 {
        //                      new SqlParameter("@Message", SqlDbType.NVarChar, 100) { Direction = ParameterDirection.Output },
        //                      new SqlParameter("@Code", SqlDbType.NVarChar, 2) { Direction = ParameterDirection.Output },
        //                      new SqlParameter("@Description", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output }
        //                 };

        //                // Add output parameters to the list
        //                paramList.AddRange(outputParameters);
        //                command.Parameters.AddRange(paramList.ToArray());

        //                // Execute and fill DataSet
        //                var dataSet = new DataSet();
        //                using (var dataAdapter = new SqlDataAdapter(command))
        //                {
        //                    dataAdapter.Fill(dataSet);
        //                }

        //                // Retrieve output parameters' values
        //                foreach (var param in outputParameters)
        //                {
        //                    outputParams[param.ParameterName] = command.Parameters[param.ParameterName].Value;
        //                }

        //                return (dataSet, outputParams);
        //            }
        //        }
        //    }
        //    catch (SqlException sqlEx)
        //    {
        //        Console.WriteLine($"SQL Exception: {sqlEx.Message}");
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Exception: {ex.Message}");
        //        throw;
        //    }
        //}


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

    }
}
