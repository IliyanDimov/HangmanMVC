using System.Collections.Generic;

namespace HangmanMVC.Web.ViewModels
{
    /// <summary>
    /// This view model is used to store data, which is needed during the game.
    /// </summary>
    public class GameViewModel
    {
        private string url;

        /// <summary>
        /// Current user id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Full word to guess
        /// </summary>
        public string FullWord { get; set; }

        /// <summary>
        /// Word in progress - contains only guessed letters + empty spaces ( _ )
        /// </summary>
        public List<string> WordInProgress { get; set; }

        /// <summary>
        /// Word description
        /// </summary>
        public string Clue { get; set; }

        /// <summary>
        /// Number of guessed letters
        /// </summary>
        public int GuessedChars { get; set; }

        /// <summary>
        /// Number of mistaken letters
        /// </summary>
        public int MistakenChars { get; set; }

        /// <summary>
        /// Current game status
        /// </summary>
        public GameStatus GameStatus { get; set; }

        /// <summary>
        /// Game image url
        /// </summary>
        public string ImageUrl
        {
            get
            {
                return url;
            }
            set
            {
                url = value;
            }
        }

        /// <summary>
        /// Word categories
        /// </summary>
        public List<string> Categories { get; set; }

        /// <summary>
        /// Currently selected category
        /// </summary>
        public string SelectedCategory { get; set; }
    }

    /// <summary>
    /// Status of the game
    /// </summary>
    public enum GameStatus
    {
        Win,
        Lose,
        InProgress
    }
}