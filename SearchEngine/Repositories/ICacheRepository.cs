using SearchEngine.Models;

namespace SearchEngine.Repositories
{
    public interface ICacheRepository
    {
        Task<T> GetData<T>(string key) where T : class;
        Task<bool> SetData<T>(string key, T value, DateTime expirationTime);
    }
}
