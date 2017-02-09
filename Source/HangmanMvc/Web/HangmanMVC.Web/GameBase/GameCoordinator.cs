using System.Collections.Generic;
using System.Linq;

using HangmanMVC.Web.ViewModels;
using HangmanMVC.Data.Common;

namespace HangmanMVC.Web.GameBase
{
    /// <summary>
    /// Coordinator class, which handles all the logic during the game
    /// </summary>
    public class GameCoordinator
    {
        private static GameViewModel viewModel = new GameViewModel();
        private static ResultsViewModel resultsViewModel = new ResultsViewModel();

        /// <summary>
        /// Gets/sets the view model
        /// </summary>
        public GameViewModel ViewModel
        {
            get
            {
                return viewModel;
            }
            set
            {
                viewModel = value;
            }
        }

        /// <summary>
        /// Gets/sets the results view model
        /// </summary>
        public ResultsViewModel ResultsViewModel
        {
            get
            {
                return resultsViewModel;
            }
            set
            {
                resultsViewModel = value;
            }
        }

        /// <summary>
        /// Starts the game and initializes needed data.
        /// </summary>
        /// <param name="userId">Id of the current user</param>
        /// <param name="fullWord">The full word to guess</param>
        /// <param name="clue">Description of the word</param>
        /// <param name="categories">Word categories</param>
        /// <param name="selectedCategory">Currently selected word category</param>
        public void StartGame(string userId, string fullWord, string clue, 
            List<string> categories, string selectedCategory)
        {
            // Initialize view model
            viewModel = new GameViewModel
            {
                Id = userId,
                FullWord = fullWord,
                WordInProgress = new List<string>(),
                Clue = clue,
                GuessedChars = 0,
                MistakenChars = 0,
                GameStatus = GameStatus.InProgress,
                ImageUrl = UtilityConstants.InitialImageUrl,
                Categories = categories,
                SelectedCategory = selectedCategory
        };

            ProcessInitialWord();
        }

        /// <summary>
        /// Processes a letter guess
        /// </summary>
        /// <param name="guess">Letter from user input</param>
        public void ProcessGuess(string guess)
        {
            if (viewModel.GameStatus != GameStatus.Win && viewModel.GameStatus != GameStatus.Lose)
            {
                bool isGuessed = false;

                for (int i = 1; i < viewModel.FullWord.Count() - 1; i++)
                {
                    if (viewModel.FullWord[i].ToString().ToLower() == guess && viewModel.WordInProgress[i] != guess)
                    {
                        viewModel.WordInProgress[i] = guess;
                        isGuessed = true;
                    }
                }

                if (isGuessed)
                {
                    viewModel.GuessedChars += 1;
                    viewModel.GameStatus = (viewModel.WordInProgress.Where(x => x.Contains("_")).Count() == 0) ?
                        GameStatus.Win : GameStatus.InProgress;
                }
                else
                {
                    viewModel.MistakenChars += 1;
                    viewModel.GameStatus = (viewModel.MistakenChars == UtilityConstants.AllowedMistakes)
                        ? GameStatus.Lose : GameStatus.InProgress;
                }

                viewModel.ImageUrl = "/Content/Images/" + viewModel.MistakenChars + ".png";
            }
        }

        /// <summary>
        /// Processes full word guess
        /// </summary>
        /// <param name="guess">Full word from user input</param>
        public void ProcessFullGuess(string guess)
        {
            if (viewModel.GameStatus != GameStatus.Win && viewModel.GameStatus != GameStatus.Lose)
            {
                if (viewModel.FullWord.ToLower() == guess.ToLower())
                {
                    viewModel.GameStatus = GameStatus.Win;
                }
                else
                {
                    viewModel.GameStatus = GameStatus.Lose;
                }
            }
        }

        /// <summary>
        /// Processes the initial state of the word. Adds only first and last letters and fills
        /// the rest with empty spaces.
        /// </summary>
        private void ProcessInitialWord()
        {            
            viewModel.WordInProgress.Add(viewModel.FullWord[0].ToString());

            for (int i = 1; i < viewModel.FullWord.Count() - 1; i++)
            {
                if (viewModel.FullWord[i].ToString() == " ")
                {
                    // We have found the pause between multiple words. 
                    // Add last character of previous word.
                    viewModel.WordInProgress[i - 1] = viewModel.FullWord[i - 1].ToString();

                    // Add empty space between words and add first character of next word.
                    viewModel.WordInProgress.Add(" ");
                    viewModel.WordInProgress.Add(viewModel.FullWord[i + 1].ToString());
                }

                else if ((i == viewModel.FullWord.Count() - 2) && (viewModel.FullWord[i - 1] != ' '))
                {
                    viewModel.WordInProgress.Add(" _ ");

                }
                else if (viewModel.FullWord[i - 1] != ' ')
                {
                    viewModel.WordInProgress.Add(" _");
                }
            }
            viewModel.WordInProgress.Add(viewModel.FullWord.Last().ToString());
        }
    }
}