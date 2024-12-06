namespace SearchEngine.Models
{
    public class SearchParams
    {
        public string SearchTxt { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }
}
