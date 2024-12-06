using HtmlAgilityPack;
using SearchEngine.Data;
using SearchEngine.Extenstions;
using SearchEngine.Models;
using System.Web;
namespace SearchEngine.Repositories
{
    public class SearchRepository : ISearchRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly ICacheRepository _cacheRepository;
        private readonly ILogRepository _logRepository;

        public SearchRepository(AppDbContext appDbContext,
            ICacheRepository cacheRepository,
            ILogRepository logRepository)
        {
            _cacheRepository = cacheRepository;
            _logRepository = logRepository;
            _appDbContext = appDbContext;
        }
        /// <summary>
        /// main function to retrive the data
        /// </summary>
        /// <param name="searchParams"></param>
        /// <param name="searchEngine"></param>
        /// <returns></returns>
        public async Task<List<SearchRecord>> getSearchRecords(SearchParams searchParams, Models.SearchEngine searchEngine)
        {
            List<SearchRecord> records = new List<SearchRecord>();
            var listFromGoogle = await getResultsFromGoogle(searchParams, Models.SearchEngine.Google);
            records.AddRange(listFromGoogle);
            var listFromBing = await getResultsFromBing(searchParams, Models.SearchEngine.Bing);
            records.AddRange(listFromBing);
            return records;

            
        }
        #region getResults
        private async Task<List<SearchRecord>> getResultsFromBing(SearchParams searchParams, Models.SearchEngine searchEngine)
        {
            try
            {
                ////cache key
                string cacheKeyName = GetCacheKeyNameBySearchParams(searchParams, searchEngine);
                ////try to get data by cache
                List<SearchRecord> records = await _cacheRepository.GetData<List<SearchRecord>>(cacheKeyName);
                if (records != null)
                {
                    return records;
                }
                ////get data from http
                List<SearchRecord> list = await getRecordsFromHttpBing(searchParams.SearchTxt, searchParams.PageNumber, searchParams.PageSize);
                ////set data into cache
                bool isCached = await _cacheRepository.SetData<List<SearchRecord>>(cacheKeyName, list, DateTime.Now);
                if (!isCached)
                {
                    await _logRepository.WriteToLog("cache set data failed");
                }
                ////write results to DB
                await addSearchRecordsToDB(list);
                

                return list;

            }
            catch (Exception ex)
            {
                await _logRepository.WriteToLog(ex.Message);

                return null;
            }
        }
        private async Task<List<SearchRecord>> getResultsFromGoogle(SearchParams searchParams,SearchEngine.Models.SearchEngine searchEngine)
        {
            try
            {
                string cacheKeyName = GetCacheKeyNameBySearchParams(searchParams, searchEngine);
                List<SearchRecord> records = await _cacheRepository.GetData<List<SearchRecord>>(cacheKeyName);
                if (records != null)
                {
                    return records;
                }

                List<SearchRecord> list = await getRecordsFromHttpGoogle(searchParams.SearchTxt, searchParams.PageNumber, searchParams.PageSize);
                bool isCached = await _cacheRepository.SetData<List<SearchRecord>>(cacheKeyName, list, DateTime.Now);

                await addSearchRecordsToDB(list);

                return list;

            }
            catch (Exception ex)
            {
                await _logRepository.WriteToLog(ex.Message);

                return null;
            }
        }
        #endregion



        #region helper functions
        private async Task addSearchRecordsToDB(List<SearchRecord> records)
        {
            for (int i = 0; i < records.Count(); i++)
            {
                _appDbContext.Add(records[i]);
                if (await _appDbContext.SaveChangesAsync() > 0)
                {
                    
                }
                else
                {
                    await _logRepository.WriteToLog("object " + records[i].ToString() + " not added to db");
                }
            }
        }
        private string GetCacheKeyNameBySearchParams(SearchParams searchParams, SearchEngine.Models.SearchEngine searchEngine)
        {
            return searchParams.SearchTxt + "-" + searchParams.PageNumber + "-" + searchParams.PageSize + "-" + searchEngine;
        }
        //get html as string
        private async Task<string> getHttpClientResult(string url)
        {
            HttpClient httpClient = new HttpClient();
            //for debug
            //await _logRepository.WriteToLog("http url getString: " + url);
            string html = await httpClient.GetStringAsync(url);
            return html;
        }
        private HtmlNodeCollection? ExtractTitlesToHtmlNodeCollection(string html, string xpathNodes)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            HtmlNodeCollection? resultNodes = htmlDocument.DocumentNode.SelectNodes(xpathNodes);
            return resultNodes;
        }
        #endregion

        #region getRecordFromHttp
        private async Task<List<SearchRecord>> getRecordsFromHttpBing(string searchTxt, int pageNumber, int pageSize)
        {
            //bing
            //https://www.bing.com/search?q=%D7%A9%D7%9C%D7%95%D7%9D&count=5&first=1
            //https://www.bing.com/search?q=%D7%A9%D7%9C%D7%95%D7%9D&count=5&first=6
            //https://www.bing.com/search?q=%D7%A9%D7%9C%D7%95%D7%9D&count=5&first=11
            try
            {
                string url = $@"https://www.bing.com/search?q={searchTxt}&count={pageSize}&first={pageNumber}";
                string html = await getHttpClientResult(url);
                HtmlNodeCollection? resultNodes = ExtractTitlesToHtmlNodeCollection(html,Consts.BING_XPATH_NODES);

                List<SearchRecord> list = new List<SearchRecord>();

                if (resultNodes != null)
                {
                    SearchRecord record;

                    foreach (var resultNode in resultNodes)
                    {
                        var titleNode = resultNode.SelectSingleNode(Consts.BING_XPATH_INTERNAL_SINGLE_NODE);

                        if (titleNode != null)
                        {
                            string title = HttpUtility.HtmlDecode(titleNode?.InnerText);
                            if (!string.IsNullOrEmpty(title))
                            {
                                record = new SearchRecord
                                {
                                    SearchEngine = Models.SearchEngine.Bing,
                                    EnteredDate = DateTime.Now,
                                    Title = title
                                };
                                list.Add(record);
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No results found.");
                }


                return list;


                
            } catch (Exception ex)
            {
                await _logRepository.WriteToLog("ex: " + ex.Message);
                return null;
            }

            
        }
        private async Task<List<SearchRecord>> getRecordsFromHttpGoogle(string searchTxt, int pageNumber, int pageSize)
        {
            try
            {
                //https://www.google.com/search?q=hello&num=5&start=25&
                string url = $@"https://www.google.com/search?q={searchTxt}&num={pageSize}&start={pageNumber}";
                string html = await getHttpClientResult(url);
                HtmlNodeCollection? resultNodes = ExtractTitlesToHtmlNodeCollection(html, Consts.GOOGLE_XPATH_NODES);

                List<SearchRecord> list = new List<SearchRecord>();
                if (resultNodes == null)
                {
                    //todo - log not found
                    return list;
                } else { 
                    SearchRecord record;
                    foreach (var resultNode in resultNodes)
                    {
                        // Extract the title and URL from each result
                        var linkNode = resultNode.SelectSingleNode(Consts.GOOGLE_XPATH_INTERNAL_SINGLE_NODE);

                        string title = HttpUtility.HtmlDecode(linkNode?.InnerText);
                        if (!string.IsNullOrEmpty(title))
                        {
                            record = new SearchRecord
                            {
                                SearchEngine = Models.SearchEngine.Google,
                                EnteredDate = DateTime.Now,
                                Title = title
                            };
                            list.Add(record);
                        }
                    }
                    if (list.Count == 0)
                    {
                        await _logRepository.WriteToLog("url: " + url + " - 0 results");

                    }
                    return list;
                }
                
            } catch (Exception ex)
            {
                await _logRepository.WriteToLog("ex:" + ex.Message);
                return null;
            }
            
        }
        #endregion

    }
}
