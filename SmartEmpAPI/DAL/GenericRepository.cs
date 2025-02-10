using Microsoft.Data.SqlClient;

namespace SmartEmpAPI.DAL
{

    namespace YourProject.DataAccess
    {
        public class GenericRepository : IGenericRepository
        {
            private readonly string _connectionString;

            public GenericRepository(IConfiguration configuration)
            {
                _connectionString = configuration.GetConnectionString("DefaultConnection");
            }

            public async Task<List<T>> GetAllRecordsAsync<T>(string query, object parameters = null) where T : class
            {
                List<T> records = new List<T>();

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            var properties = parameters.GetType().GetProperties();
                            foreach (var prop in properties)
                            {
                                command.Parameters.AddWithValue($"@{prop.Name}", prop.GetValue(parameters) ?? DBNull.Value);
                            }
                        }

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var record = Activator.CreateInstance<T>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    var columnName = reader.GetName(i);
                                    var propertyInfo = record.GetType().GetProperty(columnName);
                                    if (propertyInfo != null && reader[columnName] != DBNull.Value)
                                    {
                                        propertyInfo.SetValue(record, reader[columnName]);
                                    }
                                }
                                records.Add(record);
                            }
                        }
                    }
                }

                return records;
            }
        }
    }

}
