namespace CookTheWeek.Services.Tests
{
    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data;
    using CookTheWeek.Services.Interfaces;
    using Data.Interfaces;
    using Services.Data;

    using static DbSeeder;

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
            this.dbContext = new CookTheWeekDbContext(this.dbOptions);

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
            const int usersCount = 2;

            int result = await this.userService.AllUsersCountAsync();

            Assert.AreEqual(usersCount, result);
        }

    }
}