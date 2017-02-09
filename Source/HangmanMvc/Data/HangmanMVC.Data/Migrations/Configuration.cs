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

            var animalsCategory = new WordCategory { Name = "�������" };
            animalsCategory.Words.Add(new Word { Value = "�������", Clue = "�����. ���� ��������� � ����� � ����� � ���������." });
            animalsCategory.Words.Add(new Word { Value = "�����", Clue = "������ � �������. ��� ����� ��� � ����� ���." });
            animalsCategory.Words.Add(new Word { Value = "����� �����", Clue = "������� ��� �� ����." });            
            context.WordCategories.Add(animalsCategory);

            var carsCategory = new WordCategory { Name = "���������" };
            carsCategory.Words.Add(new Word { Value = "������", Clue = "���� �� �������� �� ������� � ��� - ����������� � �����." });
            carsCategory.Words.Add(new Word { Value = "����������", Clue = "��������� ��������� �� ��������." });
            carsCategory.Words.Add(new Word { Value = "���� �����", Clue = "������������ ���������� ���������." });
            context.WordCategories.Add(carsCategory);

            var moviesCategory = new WordCategory { Name = "�����" };
            moviesCategory.Words.Add(new Word { Value = "���� � �����", Clue = "�������� ���� � ��� ����" });
            moviesCategory.Words.Add(new Word { Value = "�������� ��� ���", Clue = "�������� ��������� ���� � ������ �������" });
            context.WordCategories.Add(moviesCategory);

            context.SaveChanges();
        }
    }
}
