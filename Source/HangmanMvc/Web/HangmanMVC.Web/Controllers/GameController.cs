using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;

using HangmanMVC.Data.Models;
using HangmanMVC.Web.ViewModels;
using HangmanMVC.Data;
using HangmanMVC.Web.GameBase;
using HangmanMVC.Data.Common;

namespace HangmanMVC.Web.Controllers
{
    public class GameController : Controller
    {
        private IDbGenericRepository<ApplicationUser, string> users;
        private IDbRepository<Word> words;
        private IDbRepository<WordCategory> categories;

        public GameController(IDbGenericRepository<ApplicationUser, string> userRepo, IDbRepository<Word> wordsRepo,
            IDbRepository<WordCategory> categoriesRepo)
        {
            users = userRepo;
            words = wordsRepo;
            categories = categoriesRepo;
        }

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
                new List<string>(), wordQuery.Clue, categories.All().Select(x => x.Name).ToList(), selValue);

            return View("../Game/Game", gameCoordinator.ViewModel);
        }

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
        
        public ActionResult ProcessWord(string guess)
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

        private void FinishGame(GameCoordinator coordinator)
        {
            GameViewModel model = coordinator.ViewModel;
            // Get current player record so we can update the statistics
            var user = users.GetById(model.Id);

            if (model.GameStatus == GameStatus.Win)
            {
                model.ImageUrl = "/Content/Images/success.png";

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
                model.ImageUrl = "/Content/Images/game_over.png";

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