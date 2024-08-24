namespace CookTheWeek.Services.Tests.UnitTests
{
    using CookTheWeek.Data.Models;
    using CookTheWeek.Web.ViewModels.Admin.UserAdmin;
    using Data.Interfaces;
    using Microsoft.AspNetCore.Identity;
    using Services.Data;

    [TestFixture]
    public class UserTests : UnitTestBase
    {
        private IUserService userService;
        private IRecipeService recipeService;
        private IUserAdminService userAdminService;
        private IMealPlanService mealPlanService;
        private UserManager<ApplicationUser> userManager;

        [SetUp]
        public void SetUp(UserManager<ApplicationUser> userManager)
        {
            
            this.recipeService = new RecipeService(data);
            this.userManager = userManager;
            this.mealPlanService = new MealPlanService(data);
            this.userAdminService = new UserAdminService(data, recipeService, userManager);
            this.userService = new UserService(userManager, data, recipeService, mealPlanService);
        }

        [Test]
        public async Task ExistsByIdAsync_ShouldReturn_True_If_UserExists()
        {
            string existingUserId = TestUser.Id.ToString();

            bool result = await this.userService.ExistsByIdAsync(existingUserId);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task ExistsByIdAsync_ShouldReturn_False_If_UserDoesNotExists()
        {
            string nonExistingUserId = Guid.NewGuid().ToString();

            bool result = await this.userService.ExistsByIdAsync(nonExistingUserId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task AllCountAsync_ShouldReturn_Correct_UsersCount()
        {
            // Assert
            const int usersCount = 2;

            // Act
            int result = await this.userAdminService.AllCountAsync();

            // Arrange
            Assert.AreEqual(usersCount, result);
        }

        [Test]
        public async Task AllAsync_ShouldReturn_Correct_UsersInfo()
        {
            // Arrange
            ICollection<UserAllViewModel> expectedResult = new HashSet<UserAllViewModel>()
            {
                new()
                {
                    Id = AdminUser.Id.ToString(),
                    Username = AdminUser.UserName,
                    Email = AdminUser.Email,
                    TotalRecipes = 1,
                    TotalMealPlans = 0
                },
                new()
                {
                    Id = TestUser.Id.ToString(),
                    Username = TestUser.UserName,
                    Email = TestUser.Email,
                    TotalRecipes = 0,
                    TotalMealPlans = 0
                },
            };

            // Act
            var actualResult = await this.userAdminService.AllAsync();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actualResult, Is.Not.Null);
                Assert.That(actualResult, Is.InstanceOf<ICollection<UserAllViewModel>>());

                if(actualResult != null)
                {
                    Assert.That(expectedResult.Count, Is.EqualTo(actualResult.Count));

                    IEnumerator<UserAllViewModel> expectedEnumerator = expectedResult.GetEnumerator();
                    IEnumerator<UserAllViewModel> actualEnumerator = actualResult.GetEnumerator();

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

        [Test]
        public async Task IsOwnerByRecipeIdAsync_ShouldReturn_True_If_IsOwner()
        {
            // Arrange
            string userId = TestRecipe.OwnerId.ToString();
            string recipeId = TestRecipe.Id.ToString();

            // Act
            bool result = await userService.IsOwnerByRecipeIdAsync(recipeId, userId);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task IsOwnerByRecipeIdAsync_ShouldReturn_False_If_IsNotOwner()
        {
            // Arrange
            string userId = Guid.NewGuid().ToString();  
            string recipeId = TestRecipe.Id.ToString();

            // Act
            bool result = await userService.IsOwnerByRecipeIdAsync(recipeId, userId);

            // Assert
            Assert.IsFalse(result);
        }
    }
}