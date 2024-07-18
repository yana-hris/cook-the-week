namespace CookTheWeek.Services.Tests.UnitTests
{
    using CookTheWeek.Data.Models;
    using CookTheWeek.Web.ViewModels.RecipeIngredient;
    using Data;
    using Data.Interfaces;

    public class RecipeIngredientServiceTests : UnitTestBase
    {
        private IRecipeIngredientService recipeIngredientService;

        [SetUp]
        public void SetUp()
        {
            this.recipeIngredientService = new RecipeIngredientService(data);
        }

        [Test]
        public async Task GetRecipeIngredientMeasuresAsync_ShouldReturn_Correct_ModelAndData()
        {
            // Arrange
            ICollection<RecipeIngredientSelectMeasureViewModel> expectedResult = data.Measures
                .Select(m => new RecipeIngredientSelectMeasureViewModel()
                {
                    Id = m.Id,
                    Name = m.Name,
                }).ToList();

            // Act
            var actualResult = await this.recipeIngredientService.GetRecipeIngredientMeasuresAsync();

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.That(actualResult, Is.InstanceOf<ICollection<RecipeIngredientSelectMeasureViewModel>>());
            Assert.That(actualResult.Count, Is.EqualTo(expectedResult.Count));

            IEnumerator<RecipeIngredientSelectMeasureViewModel> expectedEnumerator = expectedResult.GetEnumerator();
            IEnumerator<RecipeIngredientSelectMeasureViewModel> actualEnumerator = actualResult.GetEnumerator();

            while (expectedEnumerator.MoveNext() && actualEnumerator.MoveNext())
            {
                Assert.That(expectedEnumerator.Current.Id, Is.EqualTo(actualEnumerator.Current.Id));
                Assert.That(expectedEnumerator.Current.Name, Is.EqualTo(actualEnumerator.Current.Name));
            }
        }

        [Test]
        public async Task GetRecipeIngredientSpecificationsAsync_ShouldReturn_Correct_ModelAndData()
        {
            // Arrange
            ICollection<RecipeIngredientSelectSpecificationViewModel> expectedResult = data.Specifications
                .Select(sp => new RecipeIngredientSelectSpecificationViewModel()
                {
                    Id = sp.Id,
                    Description = sp.Description,
                }).ToList();

            // Act
            var actualResult = await this.recipeIngredientService.GetRecipeIngredientSpecificationsAsync();

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.That(actualResult, Is.InstanceOf<ICollection<RecipeIngredientSelectSpecificationViewModel>>());
            Assert.That(actualResult.Count, Is.EqualTo(expectedResult.Count));

            IEnumerator<RecipeIngredientSelectSpecificationViewModel> expectedEnumerator = expectedResult.GetEnumerator();
            IEnumerator<RecipeIngredientSelectSpecificationViewModel> actualEnumerator = actualResult.GetEnumerator();

            while (expectedEnumerator.MoveNext() && actualEnumerator.MoveNext())
            {
                Assert.That(expectedEnumerator.Current.Id, Is.EqualTo(actualEnumerator.Current.Id));
                Assert.That(expectedEnumerator.Current.Description, Is.EqualTo(actualEnumerator.Current.Description));
            }
        }

        [Test]
        public async Task AddAsync_ShouldAdd_RecipeIngredient_Correctly()
        {
            // Arrange
            var testRecipeId = TestRecipe.Id.ToString();
            int recipeIngredientsCountBeforeAdd = TestRecipe.RecipesIngredients.Count;
           
            var testRecipeIngredientModel = new RecipeIngredientFormModel()
            {
                Name = TestIngredient.Name,
                Qty = new RecipeIngredientQtyFormModel() { QtyDecimal = 10},
                MeasureId = data.Measures.First().Id,
                SpecificationId = data.Specifications.First().Id,
            };            

            // Act
            await this.recipeIngredientService.AddAsync(testRecipeIngredientModel, testRecipeId);

            // Assert
            int recipeIngredientsCountAfterAdd = data.RecipesIngredients.Where(ri => ri.RecipeId.ToString() == testRecipeId).Count();
            bool exists = data.RecipesIngredients.Any(ri => ri.RecipeId.ToString() == testRecipeId && ri.IngredientId == TestIngredient.Id);
            
            Assert.That(recipeIngredientsCountAfterAdd, Is.EqualTo(recipeIngredientsCountBeforeAdd + 1));
            Assert.IsTrue(exists);
        }

        //[Test]
        //public async Task IsAlreadyAddedAsync_ShouldReturn_True_If_IsAdded()
        //{
        //    // Arrange

        //}
    }
}
