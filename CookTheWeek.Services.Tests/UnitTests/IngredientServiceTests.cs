namespace CookTheWeek.Services.Tests.UnitTests
{
    using CookTheWeek.Services.Data;
    using CookTheWeek.Services.Data.Interfaces;
    using CookTheWeek.Services.Data.Models.Ingredient;
    using CookTheWeek.Services.Data.Models.RecipeIngredient;
    using CookTheWeek.Web.ViewModels.Ingredient;
    using System.Globalization;

    [TestFixture]
    public class IngredientServiceTests : UnitTestBase
    {
        private IIngredientService ingredientService;
        
        [SetUp]
        public void SetUp()
        {
            this.ingredientService = new IngredientService(data);
        }

        [Test]
        public async Task AllAsync_ShouldReturn_Correct_Model_AndData()
        {
            // Arrange
            AllIngredientsQueryModel testModel = new()
            {
                SearchString = TestIngredient.Name,
                Category = TestIngredient.Category.Name,
                IngredientSorting = Web.ViewModels.Ingredient.Enums.IngredientSorting.NameAscending,
            };

            AllIngredientsFilteredAndPagedServiceModel expectedResult = new()
            {
                TotalIngredientsCount = 1,
                Ingredients = new List<IngredientAllViewModel>()
                {
                    new()
                    {
                        Id = TestIngredient.Id,
                        Name = TestIngredient.Name
                    }
                }
            };

            // Act
            var actualResult = await this.ingredientService.AllAsync(testModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actualResult, Is.Not.Null);
                Assert.That(actualResult, Is.InstanceOf<AllIngredientsFilteredAndPagedServiceModel>());

                if (actualResult != null)
                {
                    Assert.That(expectedResult.TotalIngredientsCount, Is.EqualTo(actualResult.TotalIngredientsCount));
                    Assert.That(actualResult.Ingredients, Is.InstanceOf<ICollection<IngredientAllViewModel>>());
                    Assert.That(actualResult.Ingredients.Count, Is.EqualTo(expectedResult.Ingredients.Count));

                    IEnumerator<IngredientAllViewModel> expectedEnumerator = expectedResult.Ingredients.GetEnumerator();
                    IEnumerator<IngredientAllViewModel> actualEnumerator = actualResult.Ingredients.GetEnumerator();

                    while (expectedEnumerator.MoveNext() && actualEnumerator.MoveNext())
                    {
                        Assert.That(expectedEnumerator.Current.Id, Is.EqualTo(actualEnumerator.Current.Id));
                        Assert.That(expectedEnumerator.Current.Name, Is.EqualTo(actualEnumerator.Current.Name));
                    }
                }
            });
        }

        [Test]
        public async Task AddAsync_ShouldAdd_Ingredient_Correctly()
        {
            // Arrange
            IngredientAddFormModel newIngredientModel = new()
            {
                Name = "New Ingredient",
                CategoryId = 1,
            };
            bool alreadyExists = data.Ingredients.Any(i => i.Name.ToLower() == newIngredientModel.Name.ToLower());
            int ingredientsCountBeforeAdd = data.Ingredients.Count();

            // Act
            if(alreadyExists)
            {
                Assert.IsFalse(true, "Trying to add an already existing ingredient makes the test invalid!");
            }
            else
            {
                await this.ingredientService.AddAsync(newIngredientModel);
            }

            // Assert
            int ingredientsCountAfterAdd = data.Ingredients.Count();
            bool exists = data.Ingredients.Any(i => i.Name == newIngredientModel.Name);
            Assert.That(ingredientsCountAfterAdd, Is.EqualTo(ingredientsCountBeforeAdd + 1));
            Assert.IsTrue(exists);
        }

        [Test]
        public async Task EditAsync_ShouldEdit_Ingredient_Correctly()
        {
            // Arrange
            var ingredientToEdit = data.Ingredients.First();
            int id = ingredientToEdit.Id;
            string editedName = ingredientToEdit.Name + " Edited";
            int editedCategoryId = ingredientToEdit.CategoryId + 1;

            IngredientEditFormModel editIngredientModel = new()
            {
                Id = ingredientToEdit.Id,
                Name = editedName,
                CategoryId = editedCategoryId,
            };
            bool alreadyExists = data.Ingredients.Any(i => i.Name.ToLower() == editIngredientModel.Name.ToLower());
            
            await this.ingredientService.EditAsync(editIngredientModel);
            var editedIngredient = data.Ingredients.FirstOrDefault(i => i.Id == id);

            // Assert
            Assert.IsNotNull(editedIngredient);
            Assert.That(editedIngredient.Name, Is.EqualTo(editedName));
            Assert.That(editedIngredient.CategoryId, Is.EqualTo(editedCategoryId));
            
        }

        [Test]
        public async Task GenerateIngredientSuggestionNamesAsync_ShouldReturn_CorrectModel_And_Data()
        {
            // Arrange
            string testSearchString = "Ingredient".ToLower();

            ICollection<RecipeIngredientSuggestionServiceModel> expectedResult = data
                .Ingredients
                .Where(i => i.Name.ToLower().Contains(testSearchString))
                .Select(i => new RecipeIngredientSuggestionServiceModel()
                {
                    Id = i.Id,
                    Name = i.Name,
                }).ToList();

            // Act
            var actualResult = await this.ingredientService
                .GenerateIngredientSuggestionsAsync(testSearchString);

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.That(actualResult, Is.InstanceOf<ICollection<RecipeIngredientSuggestionServiceModel>>());
            Assert.That(actualResult.Count, Is.EqualTo((int)expectedResult.Count));

            IEnumerator<RecipeIngredientSuggestionServiceModel> expectedEnumerator = expectedResult.GetEnumerator();
            IEnumerator<RecipeIngredientSuggestionServiceModel> actualEnumerator = actualResult.GetEnumerator();

            while (expectedEnumerator.MoveNext() && actualEnumerator.MoveNext())
            {
                Assert.That(expectedEnumerator.Current.Id, Is.EqualTo(actualEnumerator.Current.Id));
                Assert.That(expectedEnumerator.Current.Name, Is.EqualTo(actualEnumerator.Current.Name));
            }
        }

        [Test]
        public async Task GetForEditByIdAsync_ShouldReturn_CorrectModel_And_Data()
        {
            // Arrange
            var testIngredient = data.Ingredients.First();
            int testId = testIngredient.Id;

            IngredientEditFormModel expectedResult = new()
            {
                Id = testId,
                Name = testIngredient.Name,
                CategoryId = testIngredient.CategoryId
            };

            // Act
            var actualResult = await this.ingredientService.GetForEditByIdAsync(testId);

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.That(actualResult, Is.InstanceOf<IngredientEditFormModel>());
            Assert.That(actualResult.Name, Is.EqualTo(expectedResult.Name));
            Assert.That(actualResult.CategoryId, Is.EqualTo(expectedResult.CategoryId));
        }

        [Test]
        public async Task ExistsByIdAsync_ShouldReturn_True_If_Exists()
        {
            // Arrange
            int testId = data.Ingredients.Select(i => i.Id).First();

            // Act
            bool result = await this.ingredientService.ExistsByIdAsync(testId);

            // Assert
            Assert.That(result, Is.InstanceOf<bool>());
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ExistsByNameAsync_ShouldReturn_True_If_Exists()
        {
            // Arrange
            string testName = data.Ingredients.Select(i => i.Name).First();

            // Act
            bool result = await this.ingredientService.ExistsByNameAsync(testName);

            // Assert
            Assert.That(result, Is.InstanceOf<bool>());
            Assert.IsTrue(result);
        }

        [Test]
        public async Task AllCountAsync_ShouldReturn_CorrectCount()
        {
            // Arrange
            int expectedCount = data.Ingredients.Count();

            // Act
            var actualCount = await this.ingredientService.AllCountAsync();

            // Assert
            Assert.IsNotNull(actualCount);
            Assert.That(actualCount, Is.InstanceOf<int>());
            Assert.That(actualCount, Is.EqualTo(expectedCount));
        }

        [Test]
        public async Task DeleteById_ShouldWork_Correctly()
        {
            // Arrange
            var ingredientToDelete = data.Ingredients.Last();
            int id = ingredientToDelete.Id;
            int ingredientsCountBefore = data.Ingredients.Count();

            // Act
            await this.ingredientService.DeleteById(id);

            // Assert
            int ingredientsCountAfter = data.Ingredients.Count();
            bool exists = data.Ingredients.Any(i => i.Id == id);

            Assert.That(ingredientsCountAfter, Is.EqualTo(ingredientsCountBefore - 1));
            Assert.IsFalse(exists);
        }

        [Test]
        public async Task GetIdByNameAsync_ShouldWork_Correctly()
        {
            // Arrange
            var testIngredient = data.Ingredients.First();
            int expectedId = testIngredient.Id;
            string testName = testIngredient.Name;

            // Act
            var result = await this.ingredientService.GetIdByNameAsync(testName);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result, Is.InstanceOf<int>());
            Assert.That(result, Is.EqualTo(expectedId));
        }
    }
}
