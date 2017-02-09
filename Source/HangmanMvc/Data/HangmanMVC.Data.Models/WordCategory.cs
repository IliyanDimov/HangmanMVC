using HangmanMVC.Data.Common.Models;
using System.Collections.Generic;

namespace HangmanMVC.Data.Models
{
    public class WordCategory : BaseModel<int>
    {
        public WordCategory()
        {
            this.Words = new HashSet<Word>();
        }

        public string Name { get; set; }

        public virtual ICollection<Word> Words { get; set; }
    }
}
