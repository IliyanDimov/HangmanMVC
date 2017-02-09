namespace HangmanMVC.Web.ViewModels
{
    /// <summary>
    /// View model to store game results data
    /// </summary>
    public class ResultsViewModel
    {
        /// <summary>
        /// Current user name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Number of guesses
        /// </summary>
        public int Guesses { get; set; }

        /// <summary>
        /// Number of full word guesses
        /// </summary>
        public int FullWordGuesses { get; set; }

        /// <summary>
        /// Number of games played
        /// </summary>
        public int GamesPlayed { get; set; }

        /// <summary>
        /// Number of games won
        /// </summary>
        public int GamesWon { get; set; }

        /// <summary>
        /// Number of games lost
        /// </summary>
        public int GamesLost { get; set; }
    }
}