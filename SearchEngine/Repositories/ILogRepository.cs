using SearchEngine.Models;

namespace SearchEngine.Repositories
{
    public interface ILogRepository
    {
        
        Task WriteToLog(string txt);
    }
}
