namespace CookTheWeek.Services.Tests.UnitTests
{
    using CookTheWeek.Services.Data;
    using CookTheWeek.Services.Data.Interfaces;
    using CookTheWeek.Services.Data.Models.Ingredient;
    using CookTheWeek.Web.ViewModels.Ingredient;

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
            AllIngredientsQueryModel testModel = new AllIngredientsQueryModel()
            {
                SearchString = TestIngredient.Name,
                Category = TestIngredient.Category.Name,
                IngredientSorting = Web.ViewModels.Ingredient.Enums.IngredientSorting.NameAscending,
            };

            AllIngredientsFilteredAndPagedServiceModel expectedResult = new AllIngredientsFilteredAndPagedServiceModel()
            {
                TotalIngredientsCount = 1,
                Ingredients = new List<IngredientAllViewModel>()
                {
                    new IngredientAllViewModel()
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
            IngredientAddFormModel newIngredientModel = new IngredientAddFormModel()
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

            IngredientEditFormModel editIngredientModel = new IngredientEditFormModel()
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
    }
}
