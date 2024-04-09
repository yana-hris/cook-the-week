namespace CookTheWeek.Services.Tests
{
    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data;
    using CookTheWeek.Services.Interfaces;
    using Data.Interfaces;
    using Services.Data;

    using static DbSeeder;
    using CookTheWeek.Web.ViewModels.User;
    using Microsoft.IdentityModel.Tokens;

    [TestFixture]
    public class UserTests
    {
        private DbContextOptions<CookTheWeekDbContext> dbOptions;
        private CookTheWeekDbContext dbContext;

        private IUserService userService;
        private IRecipeService recipeService;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            this.dbOptions = new DbContextOptionsBuilder<CookTheWeekDbContext>()
                .UseInMemoryDatabase("CookTheWeekInMemory" + Guid.NewGuid().ToString())
                .Options;
            this.dbContext = new CookTheWeekDbContext(this.dbOptions, false);

            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Database.EnsureCreated();
            SeedDatabase(this.dbContext);

            this.recipeService = new RecipeService(this.dbContext);
            this.userService = new UserService(dbContext, recipeService);
        }

        [Test]
        public async Task UserExistsByIdAsyncShouldReturnTrueWhenUserExists()
        {
            string existingUserId = AppUser.Id.ToString();

            bool result = await this.userService.ExistsByIdAsync(existingUserId);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task UserExistsByIdAsyncShouldReturnFalseWhenUserDoesNotExists()
        {
            string nonExistingUserId = Guid.NewGuid().ToString();

            bool result = await this.userService.ExistsByIdAsync(nonExistingUserId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task AllUsersCountAsyncShouldReturnTwoIfUsersCountIsCorrect()
        {
            // Assert
            const int usersCount = 2;

            // Act
            int result = await this.userService.AllUsersCountAsync();

            // Arrange
            Assert.AreEqual(usersCount, result);
        }

        [Test]
        public async Task AllAsyncShouldReturnCorrectInfoAboutTheTwoSeededUsers()
        {
            // Arrange
            ICollection<UserViewModel> expectedResult = new HashSet<UserViewModel>()
            {
                new UserViewModel()
                {
                    Id = "72ed6dd1-7c97-4af7-ab79-fc72e4a53b16",
                    Username = "adminUser",
                    Email = "admin@gmail.com",
                    TotalRecipes = 0,
                    TotalMealPlans = 0
                },
                new UserViewModel()
                {
                    Id = "65fc0e0d-6572-4ec6-a853-c633d9f28c9e",
                    Username = "testUser",
                    Email = "user@user.bg",
                    TotalRecipes = 0,
                    TotalMealPlans = 0
                },
            };

            // Act
            var actualResult = await this.userService.AllAsync();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(expectedResult, Is.Not.Null);
                Assert.That(expectedResult, Is.InstanceOf<ICollection<UserViewModel>>());

                if(actualResult != null)
                {
                    Assert.That(expectedResult.Count, Is.EqualTo(actualResult.Count));

                    IEnumerator<UserViewModel> expectedEnumerator = expectedResult.GetEnumerator();
                    IEnumerator<UserViewModel> actualEnumerator = actualResult.GetEnumerator();

                    while (expectedEnumerator.MoveNext() && actualEnumerator.MoveNext())
                    {
                        Assert.That(expectedEnumerator.Current.Id, Is.EqualTo(actualEnumerator.Current.Id));
                        Assert.That(expectedEnumerator.Current.Username, Is.EqualTo(actualEnumerator.Current.Username));
                        Assert.That(expectedEnumerator.Current.Email, Is.EqualTo(actualEnumerator.Current.Email));
                        Assert.That(expectedEnumerator.Current.TotalRecipes, Is.EqualTo(actualEnumerator.Current.TotalRecipes));
                        Assert.That(expectedEnumerator.Current.TotalMealPlans, Is.EqualTo(actualEnumerator.Current.TotalMealPlans));
                    }
                }
                
            });

        }

    }
}