namespace CookTheWeek.Services.Tests.UnitTests
{
    using CookTheWeek.Services.Data;
    using CookTheWeek.Services.Data.Interfaces;
    using CookTheWeek.Web.ViewModels.Category;

    [TestFixture]
    public class CategoryServiceTests : UnitTestBase
    {
        private ICategoryService categoryService;

        [SetUp]
        public void SetUp()
        {
            this.categoryService = new CategoryService(data);
        }

        // Recipe Category Service Tests
        [Test]
        public async Task AllRecipeCategoriesAsync_ShouldReturn_Correct_RecipeCategoies()
        {
            // Arrange
            List<RecipeCategorySelectViewModel> expectedResult = TestRecipeCategories
                .Select(rc =>
                        new RecipeCategorySelectViewModel()
                        {
                            Id = rc.Id,
                            Name = rc.Name,
                        }).ToList();

            // Act
            var actualResult = await this.categoryService.AllRecipeCategoriesAsync();

            // Assert
            Assert.That(actualResult, Is.InstanceOf<ICollection<RecipeCategorySelectViewModel>>());
            Assert.That(actualResult.Count, Is.EqualTo(expectedResult.Count));

            IEnumerator<RecipeCategorySelectViewModel> expectedEnumerator = expectedResult.GetEnumerator();
            IEnumerator<RecipeCategorySelectViewModel> actualEnumerator = actualResult.GetEnumerator();

            while (expectedEnumerator.MoveNext() && actualEnumerator.MoveNext())
            {
                Assert.That(expectedEnumerator.Current.Id, Is.EqualTo(actualEnumerator.Current.Id));
                Assert.That(expectedEnumerator.Current.Name, Is.EqualTo(actualEnumerator.Current.Name));
            }
        }

        [Test]
        public async Task AllRecipeCategoryNamesAsync_ShouldReturn_CorrectCategoryNames()
        {
            // Arrange
            List<string> expectedResultNames = TestRecipeCategories
                .Select(rc => rc.Name)
                .ToList();

            // Act
            var actualResultNames = await this.categoryService.AllRecipeCategoryNamesAsync();

            // Assert
            Assert.That(actualResultNames, Is.InstanceOf<ICollection<string>>());
            Assert.That(actualResultNames.Count, Is.EqualTo(expectedResultNames.Count));

            IEnumerator<string> expectedEnumerator = expectedResultNames.GetEnumerator();
            IEnumerator<string> actualEnumerator = actualResultNames.GetEnumerator();

            while (expectedEnumerator.MoveNext() && actualEnumerator.MoveNext())
            {
                Assert.That(expectedEnumerator.Current, Is.EqualTo(actualEnumerator.Current));
            }

        }

        [Test]
        public async Task AddRecipeCategoryAsync_ShouldAdd_RecipeCategory_Correctly()
        {
            // Arrange
            int categoriesCountBeforeAdd = data.RecipeCategories.Count();
            RecipeCategoryAddFormModel newCategory = new()
            {
                Name = "New Recipe Category"
            };

            // Act
            await this.categoryService.AddRecipeCategoryAsync(newCategory);

            // Assert
            int categoriesCountAfterAdd = data.RecipeCategories.Count();
            bool exists = data.RecipeCategories.Any(rc => rc.Name == newCategory.Name);

            Assert.That(categoriesCountAfterAdd, Is.EqualTo(categoriesCountBeforeAdd + 1));
            Assert.IsTrue(exists);
        }

        [Test]
        public async Task EditRecipeCategoryAsync_ShouldEdit_RecipeCategory_Correctly()
        {
            // Arrange 
            var categoryToEdit = data.RecipeCategories.First();
            string oldName = categoryToEdit.Name;
            string newName = "Edited Recipe Category";


            RecipeCategoryEditFormModel editedModel = new()
            {
                Id = categoryToEdit.Id,
                Name = newName
            };

            // Act
            await this.categoryService.EditRecipeCategoryAsync(editedModel);

            // Assert
            bool newNameExists = data.RecipeCategories.Any(rc => rc.Name ==  editedModel.Name);
            bool oldNameExists = data.RecipeCategories.Any(rc => rc.Name == oldName);
            int editedCategoryId = data.RecipeCategories.Where(rc => rc.Name == editedModel.Name).Select(rc => rc.Id).FirstOrDefault();

            Assert.IsFalse(oldNameExists);
            Assert.IsTrue(newNameExists);
            Assert.IsNotNull(editedCategoryId);
            Assert.That(editedCategoryId, Is.EqualTo(categoryToEdit.Id));
        }

        [Test]
        public async Task DeleteRecipeCategoryById_ShouldDelete_RecipeCategory()
        {
            // Arrange
            var categoryToDelete = data.RecipeCategories
                .Where(rc => rc.Name == "Recipe Category Without Recipes")
                .First();

            int categorisCountBeforeDelete = data.RecipeCategories.Count();

            // Act
            await this.categoryService.DeleteRecipeCategoryById(categoryToDelete.Id);

            // Assert
            int categoriesCountAfterDelete = data.RecipeCategories.Count();
            bool exists = data.RecipeCategories.Any(rc => rc.Id == categoryToDelete.Id);

            Assert.That(categoriesCountAfterDelete, Is.EqualTo(categorisCountBeforeDelete - 1));
            Assert.IsFalse(exists);
        }

        [Test]
        public async Task GetRecipeCategoryForEditByIdAsync_ShouldReturn_CorrectModel()
        {
            // Assert
            var categoryToEdit = data.RecipeCategories.First();
            RecipeCategoryEditFormModel expectedModel = new()
            {
                Id = categoryToEdit.Id,
                Name = categoryToEdit.Name,
            };

            // Act
            var resultModel = await this.categoryService.GetRecipeCategoryForEditByIdAsync(categoryToEdit.Id);

            // Assert
            Assert.IsNotNull(resultModel);
            Assert.That(resultModel, Is.InstanceOf<RecipeCategoryEditFormModel>());
            Assert.That(resultModel.Id, Is.EqualTo(expectedModel.Id));
            Assert.That(resultModel.Name, Is.EqualTo(expectedModel.Name));
        }

        [Test]
        public async Task GetRecipeCategoryIdByNameAsync_ShouldReturn_CorrectId()
        {
            // Arrange
            var testedCategory = data.RecipeCategories.First();
            int expectedId = testedCategory.Id;
            string name = testedCategory.Name;

            // Act
            var resultId = await this.categoryService.GetRecipeCategoryIdByNameAsync(name);

            // Assert
            Assert.IsNotNull(resultId);
            Assert.That(resultId, Is.InstanceOf<int>());
            Assert.That(resultId, Is.EqualTo(expectedId));
        }

        [Test]
        public async Task RecipeCategoryExistsByIdAsync_ShouldReturn_True_If_Exists()
        {
            // Arrange
            var testedCategory = data.RecipeCategories.First();
            int existingCategoryId = testedCategory.Id;

            // Act
            var result = await this.categoryService.RecipeCategoryExistsByIdAsync(existingCategoryId);

            // Assert
            Assert.That(result, Is.InstanceOf<bool>());
            Assert.IsTrue(result);
        }

        [Test]
        public async Task RecipeCategoryExistsByIdAsync_ShouldReturn_False_If_DoesNotExist()
        {
            // Arrange
            Random generator = new();
            int randomCategoryId = generator.Next(data.RecipeCategories.Count(), int.MaxValue);

            // Act
            var result = await this.categoryService.RecipeCategoryExistsByIdAsync(randomCategoryId);

            // Assert
            Assert.That(result, Is.InstanceOf<bool>());
            Assert.IsFalse(result);
        }

        [Test]
        public async Task RecipeCategoryExistsByNameAsync_ShouldReturn_True_If_Exists()
        {
            // Arrange
            var testedCategory = data.RecipeCategories.First();
            string existingCategoryName = testedCategory.Name;

            // Act
            var result = await this.categoryService.RecipeCategoryExistsByNameAsync(existingCategoryName);

            // Assert
            Assert.That(result, Is.InstanceOf<bool>());
            Assert.IsTrue(result);
        }

        [Test]
        public async Task RecipeCategoryExistsByNameAsync_ShouldReturn_False_If_DoesNotExist()
        {
            // Arrange
            string nonExistingName = "No such Category";

            // Act
            var result = await this.categoryService.RecipeCategoryExistsByNameAsync(nonExistingName);

            // Assert
            Assert.That(result, Is.InstanceOf<bool>());
            Assert.IsFalse(result);
        }

        [Test]
        public async Task AllRecipeCategoriesCountAsync_ShouldReturn_CorrectCount()
        {
            // Arrange
            int expectedCount = data.RecipeCategories.Count();

            // Act
            var actualCount = await this.categoryService.AllRecipeCategoriesCountAsync();

            // Assert
            Assert.That(actualCount, Is.InstanceOf<int>());
            Assert.That(actualCount, Is.EqualTo(expectedCount));
        }


        // Ingredient Category Service Tests
        [Test]
        public async Task AllIngredientCategoriesAsync_ShouldReturn_Correct_IngredientCategoies()
        {
            // Arrange
            List<IngredientCategorySelectViewModel> expectedResult = TestIngredientCategories
                .Select(rc =>
                        new IngredientCategorySelectViewModel()
                        {
                            Id = rc.Id,
                            Name = rc.Name,
                        }).ToList();

            // Act
            var actualResult = await this.categoryService.AllIngredientCategoriesAsync();

            // Assert
            Assert.That(actualResult, Is.InstanceOf<ICollection<IngredientCategorySelectViewModel>>());
            Assert.That(actualResult.Count, Is.EqualTo(expectedResult.Count));

            IEnumerator<IngredientCategorySelectViewModel> expectedEnumerator = expectedResult.GetEnumerator();
            IEnumerator<IngredientCategorySelectViewModel> actualEnumerator = actualResult.GetEnumerator();

            while (expectedEnumerator.MoveNext() && actualEnumerator.MoveNext())
            {
                Assert.That(expectedEnumerator.Current.Id, Is.EqualTo(actualEnumerator.Current.Id));
                Assert.That(expectedEnumerator.Current.Name, Is.EqualTo(actualEnumerator.Current.Name));
            }
        }

        [Test]
        public async Task AllIngredientCategoryNamesAsync_ShouldReturn_CorrectCategoryNames()
        {
            // Arrange
            List<string> expectedResultNames = TestIngredientCategories
                .Select(rc => rc.Name)
                .ToList();

            // Act
            var actualResultNames = await this.categoryService.AllIngredientCategoryNamesAsync();

            // Assert
            Assert.That(actualResultNames, Is.InstanceOf<ICollection<string>>());
            Assert.That(actualResultNames.Count, Is.EqualTo(expectedResultNames.Count));

            IEnumerator<string> expectedEnumerator = expectedResultNames.GetEnumerator();
            IEnumerator<string> actualEnumerator = actualResultNames.GetEnumerator();

            while (expectedEnumerator.MoveNext() && actualEnumerator.MoveNext())
            {
                Assert.That(expectedEnumerator.Current, Is.EqualTo(actualEnumerator.Current));
            }
        }

        [Test]
        public async Task AddIngredientCategoryAsync_ShouldAdd_IngredientCategory_Correctly()
        {
            // Arrange
            int categoriesCountBeforeAdd = data.IngredientCategories.Count();
            IngredientCategoryAddFormModel newCategory = new()
            {
                Name = "New Recipe Category"
            };

            // Act
            await this.categoryService.AddIngredientCategoryAsync(newCategory);

            // Assert
            int categoriesCountAfterAdd = data.IngredientCategories.Count();
            bool exists = data.IngredientCategories.Any(rc => rc.Name == newCategory.Name);

            Assert.That(categoriesCountAfterAdd, Is.EqualTo(categoriesCountBeforeAdd + 1));
            Assert.IsTrue(exists);
        }

        [Test]
        public async Task EditIngredientCategoryAsync_ShouldEdit_IngredientCategory_Correctly()
        {
            // Arrange 
            var categoryToEdit = data.IngredientCategories.First();
            string oldName = categoryToEdit.Name;
            string newName = "Edited Ingredient Category";


            IngredientCategoryEditFormModel editedModel = new()
            {
                Id = categoryToEdit.Id,
                Name = newName
            };

            // Act
            await this.categoryService.EditIngredientCategoryAsync(editedModel);

            // Assert
            bool newNameExists = data.IngredientCategories.Any(rc => rc.Name == editedModel.Name);
            bool oldNameExists = data.IngredientCategories.Any(rc => rc.Name == oldName);
            int editedCategoryId = data.IngredientCategories.Where(rc => rc.Name == editedModel.Name).Select(rc => rc.Id).FirstOrDefault();

            Assert.IsFalse(oldNameExists);
            Assert.IsTrue(newNameExists);
            Assert.IsNotNull(editedCategoryId);
            Assert.That(editedCategoryId, Is.EqualTo(categoryToEdit.Id));
        }

        [Test]
        public async Task DeleteIngredientCategoryById_ShouldDelete_IngredientCategory()
        {
            // Arrange
            var categoryToDelete = data.IngredientCategories
                .Where(rc => rc.Name == "Ingredient Category Without Ingredients")
                .First();

            int categorisCountBeforeDelete = data.IngredientCategories.Count();

            // Act
            await this.categoryService.DeleteIngredientCategoryById(categoryToDelete.Id);

            // Assert
            int categoriesCountAfterDelete = data.IngredientCategories.Count();
            bool exists = data.IngredientCategories.Any(rc => rc.Id == categoryToDelete.Id);

            Assert.That(categoriesCountAfterDelete, Is.EqualTo(categorisCountBeforeDelete - 1));
            Assert.IsFalse(exists);
        }

        [Test]
        public async Task GetIngredientCategoryForEditByIdAsync_ShouldReturn_CorrectModel()
        {
            // Assert
            var categoryToEdit = data.IngredientCategories.First();
            IngredientCategoryEditFormModel expectedModel = new()
            {
                Id = categoryToEdit.Id,
                Name = categoryToEdit.Name,
            };

            // Act
            var resultModel = await this.categoryService.GetIngredientCategoryForEditByIdAsync(categoryToEdit.Id);

            // Assert
            Assert.IsNotNull(resultModel);
            Assert.That(resultModel, Is.InstanceOf<IngredientCategoryEditFormModel>());
            Assert.That(resultModel.Id, Is.EqualTo(expectedModel.Id));
            Assert.That(resultModel.Name, Is.EqualTo(expectedModel.Name));
        }

        [Test]
        public async Task GetIngredientCategoryIdByNameAsync_ShouldReturn_CorrectId()
        {
            // Arrange
            var testedCategory = data.IngredientCategories.First();
            int expectedId = testedCategory.Id;
            string name = testedCategory.Name;

            // Act
            var resultId = await this.categoryService.GetIngredientCategoryIdByNameAsync(name);

            // Assert
            Assert.IsNotNull(resultId);
            Assert.That(resultId, Is.InstanceOf<int>());
            Assert.That(resultId, Is.EqualTo(expectedId));
        }

        [Test]
        public async Task IngredientCategoryExistsByIdAsync_ShouldReturn_True_If_Exists()
        {
            // Arrange
            var testedCategory = data.IngredientCategories.First();
            int existingCategoryId = testedCategory.Id;

            // Act
            var result = await this.categoryService.IngredientCategoryExistsByIdAsync(existingCategoryId);

            // Assert
            Assert.That(result, Is.InstanceOf<bool>());
            Assert.IsTrue(result);
        }

        [Test]
        public async Task IngredientCategoryExistsByIdAsync_ShouldReturn_False_If_DoesNotExist()
        {
            // Arrange
            Random generator = new();
            int randomCategoryId = generator.Next(data.IngredientCategories.Count(), int.MaxValue);

            // Act
            var result = await this.categoryService.IngredientCategoryExistsByIdAsync(randomCategoryId);

            // Assert
            Assert.That(result, Is.InstanceOf<bool>());
            Assert.IsFalse(result);
        }

        [Test]
        public async Task IngredientCategoryExistsByNameAsync_ShouldReturn_True_If_Exists()
        {
            // Arrange
            var testedCategory = data.IngredientCategories.First();
            string existingCategoryName = testedCategory.Name;

            // Act
            var result = await this.categoryService.IngredientCategoryExistsByNameAsync(existingCategoryName);

            // Assert
            Assert.That(result, Is.InstanceOf<bool>());
            Assert.IsTrue(result);
        }

        [Test]
        public async Task IngredientCategoryExistsByNameAsync_ShouldReturn_False_If_DoesNotExist()
        {
            // Arrange
            string nonExistingName = "No such Category";

            // Act
            var result = await this.categoryService.IngredientCategoryExistsByNameAsync(nonExistingName);

            // Assert
            Assert.That(result, Is.InstanceOf<bool>());
            Assert.IsFalse(result);
        }

        [Test]
        public async Task AllIngredientCategoriesCountAsync_ShouldReturn_CorrectCount()
        {
            // Arrange
            int expectedCount = data.IngredientCategories.Count();

            // Act
            var actualCount = await this.categoryService.AllIngredientCategoriesCountAsync();

            // Assert
            Assert.That(actualCount, Is.InstanceOf<int>());
            Assert.That(actualCount, Is.EqualTo(expectedCount));
        }

    }
}
