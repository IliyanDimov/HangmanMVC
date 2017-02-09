using HangmanMVC.Data.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace HangmanMVC.Web.ViewModels
{
    public class GameViewModel
    {
        private string url;

        public string Id { get; set; }

        public string FullWord { get; set; }

        public List<string> WordInProgress { get; set; }

        public string Clue { get; set; }

        public int GuessedChars { get; set; }

        public int MistakenChars { get; set; }

        public GameStatus GameStatus { get; set; }

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

        public List<string> Categories { get; set; }

        public string SelectedCategory { get; set; }
    }

    public enum GameStatus
    {
        Win,
        Lose,
        InProgress
    }
}