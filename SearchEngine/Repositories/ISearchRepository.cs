using SearchEngine.Models;

namespace SearchEngine.Repositories
{
    public interface ISearchRepository
    {
        Task<List<SearchRecord>> getSearchRecords(SearchParams searchParams,SearchEngine.Models.SearchEngine searchEngine);
    }
}
