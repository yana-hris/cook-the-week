namespace CookTheWeek.Services.Tests.UnitTests
{
    using Moq;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Data;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Services.Data.Models.FavouriteRecipe;
    using CookTheWeek.Services.Data.Services;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Services.Tests.TestHelpers;

    using static CookTheWeek.Common.GeneralApplicationConstants;

    [TestFixture]
    public class FavouriteRecipeServiceTests 
    {
        private FavouriteRecipeService favouriteRecipeService;

        private Mock<IFavouriteRecipeRepository> mockFavouriteRecipeRepository;
        private Mock<IRecipeValidationService> mockRecipeValidator;
        private Mock<ILogger<FavouriteRecipeService>> mockLogger;

        private Mock<IUserContext> mockUserContext;
        private Guid userId;
        private Guid recipeId;

        [SetUp]
        public void SetUp()
        {
            mockFavouriteRecipeRepository = new Mock<IFavouriteRecipeRepository>();
            mockRecipeValidator = new Mock<IRecipeValidationService>();
            mockUserContext = new Mock<IUserContext>();
            mockLogger = new Mock<ILogger<FavouriteRecipeService>>();

            // Set up userId
            userId = Guid.NewGuid();
            recipeId = Guid.NewGuid();

            mockUserContext.SetupGet(uc => uc.UserId).Returns(userId);

            favouriteRecipeService = new FavouriteRecipeService(
                mockFavouriteRecipeRepository.Object,
                mockUserContext.Object,
                mockLogger.Object,
                mockRecipeValidator.Object
                );
        }

        [Test]
        public async Task TryToggleLikes_ThrowsException_WhenRecipeNotFound()
        {
            // Arrange
            var model = new FavouriteRecipeServiceModel
            {
                RecipeId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            // Mock the validation service to throw a RecordNotFoundException when a recipe doesn't exist
            mockRecipeValidator
                .Setup(x => x.ValidateRecipeExistsAsync(It.IsAny<Guid>()))
                .ThrowsAsync(new RecordNotFoundException("Recipe not found.", null));

            // Act & Assert
            var exception = Assert.ThrowsAsync<RecordNotFoundException>(async () =>
                await favouriteRecipeService.TryToggleLikesAsync(model));

            Assert.That(exception.Message, Is.EqualTo("Recipe not found."));
            mockFavouriteRecipeRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(),
                It.IsAny<Guid>()), Times.Never);

        }

        [Test]
        public async Task TryToggleLikes_AddsALike_WhenRecipeHasNotBeenLikedByTheUser()
        {
            // Simulate successful validation of recipeId
            mockRecipeValidator
                .Setup(v => v.ValidateRecipeExistsAsync(recipeId))
                .Returns(Task.CompletedTask); 

            // Simulate no like exists
            mockFavouriteRecipeRepository
                .Setup(repo => repo.GetByIdAsync(userId, recipeId))
                .ReturnsAsync((FavouriteRecipe)null);

            var model = new FavouriteRecipeServiceModel
            {
                UserId = userId,
                RecipeId = recipeId,
            };

            // Act
            await favouriteRecipeService.TryToggleLikesAsync(model);

            // Assert
            // Verify that AddAsync was called to add the like
            mockFavouriteRecipeRepository
                .Verify(repo => repo.AddAsync(It.IsAny<FavouriteRecipe>()), Times.Once);
        }

        [Test]
        public async Task TryToggleLikes_ThrowsUnauthorizedException_WhenServiceUserIdIsDifferenetFromTheCurrentlyLoggedInUser()
        {
            // Arrange
            mockUserContext.SetupGet(p => p.UserId).Returns(Guid.Parse(AppUserId));

            var model = new FavouriteRecipeServiceModel
            {
                RecipeId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            mockRecipeValidator
                .Setup(m => m.ValidateRecipeExistsAsync(It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);
            
            // Act & Assert
            var exception = Assert.ThrowsAsync<UnauthorizedUserException>(async () =>
            await favouriteRecipeService.TryToggleLikesAsync(model));

            Assert.That(exception, Is.InstanceOf(typeof(UnauthorizedUserException)));   
            Assert.That(exception.Message.Equals("You need to be logged in to proceed."));

            // that like was never serached for after exception is thrown
            mockFavouriteRecipeRepository
                .Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), 
                Times.Never);

            // that methods for add and delete were never called
            mockFavouriteRecipeRepository
                .Verify(repo => repo.AddAsync(It.IsAny<FavouriteRecipe>()), Times.Never);
            mockFavouriteRecipeRepository
                .Verify(repo => repo.DeleteAsync(It.IsAny<FavouriteRecipe>()), Times.Never);
        }

        [Test]
        public async Task TryToggleLikes_DeletesALike_WhenRecipeIsLikedByTheUser()
        {
            // Arrange
            mockRecipeValidator
                .Setup(m => m.ValidateRecipeExistsAsync(recipeId))
                .Returns(Task.CompletedTask);

            mockUserContext.SetupGet(uc => uc.UserId)
                .Returns(userId);

            mockFavouriteRecipeRepository
                .Setup(m => m.GetByIdAsync(userId, recipeId))
                .ReturnsAsync(new FavouriteRecipe { UserId = userId, RecipeId = recipeId});

            
            var model = new FavouriteRecipeServiceModel
            {
                UserId = userId,
                RecipeId = recipeId,
            };

            //Act 
            await favouriteRecipeService.TryToggleLikesAsync(model);

            // Assert
            // That method DeleteAsync was called once
            mockFavouriteRecipeRepository
                .Verify(repo => repo.DeleteAsync(It.IsAny<FavouriteRecipe>()),
                Times.Once);

            // That method recipe validation method was called once
            mockRecipeValidator
                .Verify(val => val.ValidateRecipeExistsAsync(It.IsAny<Guid>()),
                Times.Once);

            // That validation of like existence was performed
            mockFavouriteRecipeRepository
                .Verify(repo => repo.GetByIdAsync(userId, recipeId),
                Times.Once);    

            // That method for adding a like was never called
            mockFavouriteRecipeRepository
                .Verify(repo => repo.AddAsync(It.IsAny<FavouriteRecipe>()),
                Times.Never);

            // That no exceptions were thrown
            Assert.DoesNotThrowAsync(async () => await favouriteRecipeService
            .TryToggleLikesAsync(model));

        }

        [Test]
        public async Task GetAllRecipeIdsLikedByCurrentUserAsync_ReturnsLikedRecipeIds_WhenUserHasLikedRecipes()
        {
            // Arrange
            var likedRecipes = new List<FavouriteRecipe>
            {
                new FavouriteRecipe { UserId = userId, RecipeId = Guid.NewGuid() },
                new FavouriteRecipe { UserId = userId, RecipeId = Guid.NewGuid() }
            };

            var mockSet = CreateMockDbSet(likedRecipes);

            // Mock the ToListAsync method to return the list of recipe IDs
            var expectedRecipeIds = likedRecipes.Where(fr => fr.UserId == userId)
                                                .Select(fr => fr.RecipeId.ToString())
                                                .ToList();
            mockFavouriteRecipeRepository
                .Setup(repo => repo.GetAllQuery())
                .Returns(mockSet.Object);

            // Act
            var result = await favouriteRecipeService.GetAllRecipeIdsLikedByCurrentUserAsync();

            // Assert
            Assert.That(result.Count.Equals(2));
            Assert.That(result.First(), Is.InstanceOf<string>());
            Assert.That(result, Is.EquivalentTo(expectedRecipeIds));
        }

        [Test]
        public async Task GetRecipeTotalLikesAsync_ReturnsTotalLikes()
        {
           
            // Arrange: Set up an in-memory database just for this test
            var options = new DbContextOptionsBuilder<CookTheWeekDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var context = new CookTheWeekDbContext(options);

            // Seed data into the in-memory database
            context.FavoriteRecipes.AddRange(new List<FavouriteRecipe>
            {
                new FavouriteRecipe { RecipeId = recipeId, UserId = userId },
                new FavouriteRecipe { RecipeId = recipeId, UserId = Guid.NewGuid() },
                new FavouriteRecipe { RecipeId = Guid.NewGuid(), UserId = Guid.NewGuid() } // Not part of the test recipe
            });
            await context.SaveChangesAsync();


            // Use the in-memory repository for this test
            var favouriteRecipeRepository = new FavouriteRecipeRepository(context);

            // Reuse the already mocked services for the rest
            var service = new FavouriteRecipeService(
                favouriteRecipeRepository,
                mockUserContext.Object,  // Mocked
                mockLogger.Object,       // Mocked
                mockRecipeValidator.Object // Mocked
            );


            // Act: Call the service method
            var totalLikes = await service.GetRecipeTotalLikesAsync(recipeId);


            // Assert: The total likes should be 2, as there are two entries with the testRecipeId
            Assert.That(totalLikes.Equals(2));
        }

        [Test]
        public async Task GetRecipeLikeIfExistsAsync_ReturnsRecipeLike_WhenLikeExists()
        {
            // Arrange: 
            var expectedLike = new FavouriteRecipe { RecipeId = recipeId, UserId = userId };

            mockFavouriteRecipeRepository
                .Setup(repo => repo.GetByIdAsync(userId, recipeId))
                .ReturnsAsync(expectedLike);

            // Act: 
            var result = await favouriteRecipeService.GetRecipeLikeIfExistsAsync(recipeId);

            // Assert:
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(expectedLike.RecipeId, Is.EqualTo(result.RecipeId));
                Assert.That(expectedLike.UserId, Is.EqualTo(result.UserId));
            });

            // Verify that the repository's GetByIdAsync was called with the correct parameters
            mockFavouriteRecipeRepository
                .Verify(repo => repo.GetByIdAsync(userId, recipeId), Times.Once);
        }

        [Test]
        public async Task GetRecipeLikeIfExistsAsync_ReturnsNull_WhenLikeDoesNotExist()
        {
            // Arrange: Mock the repository to return null when the like does not exist
            mockFavouriteRecipeRepository
                .Setup(repo => repo.GetByIdAsync(userId, recipeId))
                .ReturnsAsync((FavouriteRecipe?)null);

            // Act: Call the service method
            var result = await favouriteRecipeService.GetRecipeLikeIfExistsAsync(recipeId);

            // Assert: Verify the result is null
            Assert.That(result, Is.Null);

            // Verify that the repository's GetByIdAsync was called with the correct parameters
            mockFavouriteRecipeRepository
                .Verify(repo => repo.GetByIdAsync(userId, recipeId), Times.Once);
        }

        [Test]
        public async Task SoftDeleteAllByRecipeIdAsync_SoftDeletesLikes_WhenLikesExist()
        {
            // Arrange: Mock a list of FavouriteRecipes for the given recipeId
            var likes = new List<FavouriteRecipe>
            {
                new FavouriteRecipe { RecipeId = recipeId, UserId = Guid.NewGuid(), IsDeleted = false },
                new FavouriteRecipe { RecipeId = recipeId, UserId = Guid.NewGuid(), IsDeleted = false }
            };

            // Create a mock DbSet for IQueryable support
            var mockSet = CreateMockDbSet(likes);

            // Mock GetAllQuery to return the mocked DbSet
            mockFavouriteRecipeRepository
                .Setup(repo => repo.GetAllQuery())
                .Returns(mockSet.Object);

            // Act: Call the service method
            await favouriteRecipeService.SoftDeleteAllByRecipeIdAsync(recipeId);

            // Assert: Verify that all likes were marked as deleted
            Assert.That(likes.All(like => like.IsDeleted));

            // Verify that UpdateRangeAsync was called with the updated list of likes
            mockFavouriteRecipeRepository
                .Verify(repo => repo.UpdateRangeAsync(It.Is<ICollection<FavouriteRecipe>>(x => x.All(like => like.IsDeleted))), Times.Once);
        }

        [Test]
        public async Task SoftDeleteAllByRecipeIdAsync_DoesNotCallUpdate_WhenNoLikesExist()
        {
            // Arrange: Mock an empty list of FavouriteRecipes for the given recipeId
            var likes = new List<FavouriteRecipe>();

            // Create a mock DbSet for IQueryable support
            var mockSet = CreateMockDbSet(likes);

            // Mock GetAllQuery to return the mocked DbSet
            mockFavouriteRecipeRepository
                .Setup(repo => repo.GetAllQuery())
                .Returns(mockSet.Object);

            // Act: Call the service method
            await favouriteRecipeService.SoftDeleteAllByRecipeIdAsync(recipeId);

            // Assert: Verify that UpdateRangeAsync was never called
            mockFavouriteRecipeRepository
                .Verify(repo => repo.UpdateRangeAsync(It.IsAny<ICollection<FavouriteRecipe>>()), Times.Never);
        }




        /// <summary>
        /// Helper method, used to simulate and mock IQueryable collection
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static Mock<DbSet<FavouriteRecipe>> CreateMockDbSet(IEnumerable<FavouriteRecipe> data)
        {
            var queryableData = data.AsQueryable();

            var mockSet = new Mock<DbSet<FavouriteRecipe>>();

            mockSet.As<IAsyncEnumerable<FavouriteRecipe>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<FavouriteRecipe>(queryableData.GetEnumerator()));

            mockSet.As<IQueryable<FavouriteRecipe>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<FavouriteRecipe>(queryableData.Provider));

            mockSet.As<IQueryable<FavouriteRecipe>>()
                .Setup(m => m.Expression)
                .Returns(queryableData.Expression);

            mockSet.As<IQueryable<FavouriteRecipe>>()
                .Setup(m => m.ElementType)
                .Returns(queryableData.ElementType);

            mockSet.As<IQueryable<FavouriteRecipe>>()
                .Setup(m => m.GetEnumerator())
                .Returns(queryableData.GetEnumerator());

            return mockSet;
        }

    }
}

