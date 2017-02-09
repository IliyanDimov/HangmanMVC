using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using System.Linq;
using System;

using HangmanMVC.Data.Models;
using HangmanMVC.Web.ViewModels;
using HangmanMVC.Web.GameBase;
using HangmanMVC.Data.Common;

namespace HangmanMVC.Web.Controllers
{
    /// <summary>
    /// This is the main controller for the game. It is responsible for all needed game functions.
    /// </summary>
    public class GameController : Controller
    {
        private IDbGenericRepository<ApplicationUser, string> users;
        private IDbRepository<Word> words;
        private IDbRepository<WordCategory> categories;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userRepo">Repo to help interact with the users db records</param>
        /// <param name="wordsRepo">Repo to help intercat with the words db records</param>
        /// <param name="categoriesRepo">Repo to help interact with the categories db records</param>
        public GameController(IDbGenericRepository<ApplicationUser, string> userRepo, IDbRepository<Word> wordsRepo,
            IDbRepository<WordCategory> categoriesRepo)
        {
            users = userRepo;
            words = wordsRepo;
            categories = categoriesRepo;
        }

        /// <summary>
        /// This method is reponsible for starting the game
        /// </summary>
        /// <param name="selValue">Value, indicating the selected word category in the dropdown list on the main screen</param>
        /// <returns></returns>
        public ActionResult StartGame(string selValue)
        {      
            GameCoordinator gameCoordinator = new GameCoordinator();

            Word wordQuery;

            if (selValue != null)
            {
                // Get random word from the provided Category
                wordQuery = words.All().Where(x => x.Category.Name == selValue).OrderBy(x => Guid.NewGuid()).First();
            }
            else
            {
                // Get the first category from the database and get random word from this category
                selValue = categories.All().Select(x => x.Name).ToList().First();
                wordQuery = words.All().Where(x => x.Category.Name == selValue)
                    .OrderBy(x => Guid.NewGuid()).First();                
            }

            gameCoordinator.StartGame(this.User.Identity.GetUserId(), wordQuery.Value,
                wordQuery.Clue, categories.All().Select(x => x.Name).ToList(), selValue);

            return View("../Game/Game", gameCoordinator.ViewModel);
        }

        /// <summary>
        /// Gathers the game results for the current user and sends them to the view
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowResults()
        {
            GameCoordinator gameCoordinator = new GameCoordinator();
            ApplicationUser currentUser = users.GetById(this.User.Identity.GetUserId());

            gameCoordinator.ResultsViewModel.UserName = currentUser.UserName;
            gameCoordinator.ResultsViewModel.FullWordGuesses = currentUser.FullWordGuesses;
            gameCoordinator.ResultsViewModel.GamesWon = currentUser.GamesWon;
            gameCoordinator.ResultsViewModel.GamesLost = currentUser.GamesLost;
            gameCoordinator.ResultsViewModel.GamesPlayed = currentUser.GamesPlayed;
            gameCoordinator.ResultsViewModel.Guesses = currentUser.Guesses;

            return View("../Game/Results", gameCoordinator.ResultsViewModel);
        }
        
        /// <summary>
        /// Processes a letter guess
        /// </summary>
        /// <param name="guess">A single letter, which has been chosen by the user</param>
        /// <returns></returns>
        public ActionResult WordGuess(string guess)
        {
            GameCoordinator gameCoordinator = new GameCoordinator();
            GameViewModel model = gameCoordinator.ViewModel;           

            gameCoordinator.ProcessGuess(guess);

            if (model.GameStatus == GameStatus.Win || model.GameStatus == GameStatus.Lose)
            {
                FinishGame(gameCoordinator);
            }

            return View("Game", model);
        }

        /// <summary>
        /// Processes a full word guess
        /// </summary>
        /// <param name="word">The full word, provided by the user</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FullWordGuess(string word)
        {
            GameCoordinator gameCoordinator = new GameCoordinator();
            GameViewModel model = gameCoordinator.ViewModel;

            gameCoordinator.ProcessFullGuess(word);

            if (model.GameStatus == GameStatus.Win || model.GameStatus == GameStatus.Lose)
            { 
                FinishGame(gameCoordinator);
            }    

            return View("Game", model);
        }

        /// <summary>
        /// Finishes the current game by updating the game results, depending on the win/lost state of the game
        /// </summary>
        /// <param name="coordinator"></param>
        private void FinishGame(GameCoordinator coordinator)
        {
            GameViewModel model = coordinator.ViewModel;
            // Get current player record so we can update the statistics
            var user = users.GetById(model.Id);

            if (model.GameStatus == GameStatus.Win)
            {
                model.ImageUrl = UtilityConstants.WinImageUrl;

                model.WordInProgress.Clear();
                model.WordInProgress.Add(model.FullWord);

                // Update statistics
                user.GamesWon += 1;
                if (model.GuessedChars == 0)
                {
                    user.FullWordGuesses += 1;
                }
            }
            else if (model.GameStatus == GameStatus.Lose)
            {
                model.ImageUrl = UtilityConstants.LoseImageUrl;

                // Update statistics
                user.GamesLost += 1;
            }

            // Update the completed games and number of guesses made
            user.GamesPlayed += 1;
            user.Guesses = model.GuessedChars + model.MistakenChars;

            users.Save();
        }
    }
}