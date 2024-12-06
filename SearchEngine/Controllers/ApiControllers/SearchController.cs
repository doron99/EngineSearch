using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SearchEngine.Models;
using SearchEngine.Repositories;
using System.Collections;
using System.Reflection;

namespace SearchEngine.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/Search")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchRepository _searchRepository;

        public SearchController(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }
        [HttpGet("DoSearch/{searchTxt}/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> DoSearch([FromRoute] SearchParams searchParams)
        {
            
            List<SearchRecord> results = await _searchRepository.getSearchRecords(searchParams, Models.SearchEngine.Bing);
            
            List<SearchRecordToReturnDto> resultsDto = results.Select(item => new SearchRecordToReturnDto
            {
                SearchEngine = item.SearchEngine,
                EnteredDateFormatted = item.EnteredDate.ToString("MM/dd/yyyy HH:mm"),
                Title = item.Title
                
            }).ToList();
            return Ok(resultsDto);
        }
       
    }
}
