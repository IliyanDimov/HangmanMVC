namespace HangmanMVC.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            this.SeedCategoriesWithWords(context);
            this.SeedUsers(context);
        }

        private void SeedUsers(ApplicationDbContext context)
        {
            if (context.Users.Any())
            {
                return;
            }

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            // User 1
            var email1 = $"Iliyan@hangman.com";
            var user1 = new ApplicationUser
            {
                Email = email1,
                UserName = email1
            };
            userManager.Create(user1, "123456");

            // User 2
            var email2 = $"Dimov@hangman.com";
            var user2 = new ApplicationUser
            {
                Email = email2,
                UserName = email2
            };
            userManager.Create(user2, "123456");

            context.SaveChanges();
        }

        private void SeedCategoriesWithWords(ApplicationDbContext context)
        {
            if (context.WordCategories.Any())
            {
                return;
            }

            var animalsCategory = new WordCategory { Name = "Животни" };
            animalsCategory.Words.Add(new Word { Value = "Сурикат", Clue = "Малко. Стои изправено и живее в дупки в пустинята." });
            animalsCategory.Words.Add(new Word { Value = "Мечка", Clue = "Дебело и тромаво. Спи зимен сън и обича мед." });
            animalsCategory.Words.Add(new Word { Value = "Миеща мечка", Clue = "Другото име на Енот." });            
            context.WordCategories.Add(animalsCategory);

            var carsCategory = new WordCategory { Name = "Атомобили" };
            carsCategory.Words.Add(new Word { Value = "Тойота", Clue = "Един от моделите на марката е най - продаваният в света." });
            carsCategory.Words.Add(new Word { Value = "Фолксваген", Clue = "Народният автомобил на Германия." });
            carsCategory.Words.Add(new Word { Value = "Алфа Ромео", Clue = "Емблематичен италиански автомобил." });
            context.WordCategories.Add(carsCategory);

            var moviesCategory = new WordCategory { Name = "Филми" };
            moviesCategory.Words.Add(new Word { Value = "Мъже в черно", Clue = "Известен филм с Уил Смит" });
            moviesCategory.Words.Add(new Word { Value = "Оркестър без име", Clue = "Известен български филм с Георги Мамалев" });
            context.WordCategories.Add(moviesCategory);

            context.SaveChanges();
        }
    }
}
