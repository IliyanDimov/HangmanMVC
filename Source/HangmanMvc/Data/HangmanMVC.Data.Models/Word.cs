using HangmanMVC.Data.Common.Models;

namespace HangmanMVC.Data.Models
{
    public class Word : BaseModel<int>
    {
        public string Value { get; set; }

        public string Clue { get; set; }

        public int CategoryId { get; set; }

        public virtual WordCategory Category { get; set; }
    }
}
