namespace HangmanMVC.Web.ViewModels
{
    public class ResultsViewModel
    {
        public string UserName { get; set; }

        public int Guesses { get; set; }

        public int FullWordGuesses { get; set; }

        public int GamesPlayed { get; set; }

        public int GamesWon { get; set; }

        public int GamesLost { get; set; }
    }
}