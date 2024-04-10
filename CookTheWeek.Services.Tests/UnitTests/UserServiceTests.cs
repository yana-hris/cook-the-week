namespace CookTheWeek.Services.Tests
{
    using Data.Interfaces;
    using Interfaces;
    using Services.Data;
    using Unit_Tests;
    using Web.ViewModels.User;

    [TestFixture]
    public class UserTests : UnitTestBase
    {
        private IUserService userService;
        private IRecipeService recipeService;

        [OneTimeSetUp]
        public void SetUp()
        {
            this.recipeService = new RecipeService(data);
            this.userService = new UserService(data, this.recipeService);
        }

        [Test]
        public async Task UserExistsByIdAsyncShouldReturnTrueWhenUserExists()
        {
            string existingUserId = TestUser.Id.ToString();

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
                    Id = AdminUser.Id.ToString(),
                    Username = AdminUser.UserName,
                    Email = AdminUser.Email,
                    TotalRecipes = 1,
                    TotalMealPlans = 0
                },
                new UserViewModel()
                {
                    Id = TestUser.Id.ToString(),
                    Username = TestUser.UserName,
                    Email = TestUser.Email,
                    TotalRecipes = 0,
                    TotalMealPlans = 0
                },
            };

            // Act
            var actualResult = await this.userService.AllAsync();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actualResult, Is.Not.Null);
                Assert.That(actualResult, Is.InstanceOf<ICollection<UserViewModel>>());

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