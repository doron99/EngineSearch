using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SearchEngine.Models
{
    public enum SearchEngine
    {
        Google = 0,
        Bing = 1
    }
    public class SearchRecord
    {
        [Key]
        public int SearchRecordId { get; set; }
        public SearchEngine SearchEngine { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string Title { get; set; }
        public DateTime EnteredDate { get; set; }

        public override string ToString()
        {
            string strToReturn =
                "SearchEngine:" + SearchEngine.ToString()
                + "Title:" + Title
                + "EnteredDate:" + EnteredDate.ToString("yyyy-MM-dd HH:mm");
            return strToReturn;
        }
    }
}
