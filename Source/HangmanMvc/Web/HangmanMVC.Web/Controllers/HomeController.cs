using HangmanMVC.Data;
using HangmanMVC.Data.Common;
using HangmanMVC.Data.Models;
using System.Web.Mvc;
using System.Linq;

namespace HangmanMVC.Web.Controllers
{
    public class HomeController : Controller
    {
        private IDbRepository<Word> words;
        private IDbRepository<WordCategory> wordCategories;

        private string fullWordToGuess = string.Empty;
        private string clue = string.Empty;

        private string wordInProgress = string.Empty;

        public HomeController(IDbRepository<Word> words, IDbRepository<WordCategory> wordCategories)
        {
            this.words = words;
            this.wordCategories = wordCategories;            
        }

        public ActionResult Index()
        {
            return View();
        }        

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}