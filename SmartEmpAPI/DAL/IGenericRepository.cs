namespace SmartEmpAPI.DAL
{
    public interface IGenericRepository
    {
        Task<List<T>> GetAllRecordsAsync<T>(string query, object parameters = null) where T : class;
    }
}
