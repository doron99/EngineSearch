using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SearchEngine.Models
{

    public class SearchRecordToReturnDto : SearchRecord
    {
        public SearchEngine SearchEngine { get; set; }
        public string Title { get; set; }
        public string EnteredDateFormatted { get; set; }
    }
}
