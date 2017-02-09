using System.Collections.Generic;
using System.Linq;

using HangmanMVC.Web.ViewModels;

namespace HangmanMVC.Web.GameBase
{
    public class GameCoordinator
    {
        private static GameViewModel viewModel = new GameViewModel();
        private static ResultsViewModel resultsViewModel = new ResultsViewModel();

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

        public void StartGame(string userId, string fullWord, List<string> wordInProgress, string clue, 
            List<string> categories, string selectedCategory)
        {
            // Initialize view model
            viewModel = new GameViewModel
            {
                Id = userId,
                FullWord = fullWord,
                WordInProgress = wordInProgress,
                Clue = clue,
                GuessedChars = 0,
                MistakenChars = 0,
                GameStatus = GameStatus.InProgress,
                ImageUrl = "/Content/Images/0.png",
                Categories = categories,
                SelectedCategory = selectedCategory
        };

            ProcessInitialWord();
        }

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
                    viewModel.GameStatus = (viewModel.MistakenChars == 6) ? GameStatus.Lose : GameStatus.InProgress;
                }

                viewModel.ImageUrl = "/Content/Images/" + viewModel.MistakenChars + ".png";
            }
        }

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