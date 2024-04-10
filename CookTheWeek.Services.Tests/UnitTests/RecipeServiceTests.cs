namespace CookTheWeek.Services.Tests.UnitTests
{
    using CookTheWeek.Data.Models;
    using Data.Interfaces;
    using Data.Models.Recipe;
    using Services.Data;
    using Web.ViewModels.Category;
    using Web.ViewModels.Recipe;
    using Web.ViewModels.RecipeIngredient;

    using static Common.GeneralApplicationConstants;

    [TestFixture]
    public class RecipeServiceTests : UnitTestBase
    {
        private IRecipeService recipeService;

        [OneTimeSetUp]
        public void SetUp()
        {
            this.recipeService = new RecipeService(data);
        }

        [Test]
        public async Task ExistsByIdAsyncShouldReturnTrueIfExists()
        {
            string expectedId = TestRecipe.Id.ToString();

            bool result = await this.recipeService.ExistsByIdAsync(expectedId);

            Assert.True(result);
        }

        [Test]
        public async Task ExistsByIdAsyncShouldReturnFalseIfDoesntExists()
        {
            string expectedId = Guid.NewGuid().ToString();

            bool result = await this.recipeService.ExistsByIdAsync(expectedId);

            Assert.False(result);
        }

        [Test]
        public async Task GetForEditByIdAsyncShouldReturnCorrectData()
        {
            // Arrange
            RecipeEditFormModel expctedResult = new RecipeEditFormModel()
            {
                Id = TestRecipe.Id.ToString(),
                Title = TestRecipe.Title,
                Description = TestRecipe.Description,
                Instructions = TestRecipe.Instructions,
                Servings = TestRecipe.Servings,
                CookingTimeMinutes = (int)TestRecipe.TotalTime.TotalMinutes,
                ImageUrl = TestRecipe.ImageUrl,
                RecipeCategoryId = TestRecipe.RecipeCategoryId,
            };

            // Act
            var actualResult = await this.recipeService.GetForEditByIdAsync(TestRecipe.Id.ToString());

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actualResult, Is.Not.Null);
                Assert.That(actualResult, Is.InstanceOf<RecipeEditFormModel>());

                if(actualResult != null)
                {
                    Assert.That(expctedResult.Id, Is.EqualTo(actualResult.Id));
                    Assert.That(expctedResult.Title, Is.EqualTo(actualResult.Title));
                    Assert.That(expctedResult.Description, Is.EqualTo(actualResult.Description));
                    Assert.That(expctedResult.Instructions, Is.EqualTo(actualResult.Instructions));
                    Assert.That(expctedResult.Servings, Is.EqualTo(actualResult.Servings));
                    Assert.That(expctedResult.CookingTimeMinutes, Is.EqualTo(actualResult.CookingTimeMinutes));
                    Assert.That(expctedResult.ImageUrl, Is.EqualTo(actualResult.ImageUrl));
                    Assert.That(expctedResult.RecipeCategoryId, Is.EqualTo(actualResult.RecipeCategoryId));
                }
            });
        }

        [Test]
        public async Task GetForDeleteByIdAsyncShouldReturnCorrectData()
        {
            // Arrange
            RecipeDeleteViewModel expctedResult = new RecipeDeleteViewModel()
            {
                Id = TestRecipe.Id.ToString(),
                Title = TestRecipe.Title,
                ImageUrl = TestRecipe.ImageUrl,
                Servings = TestRecipe.Servings,
                TotalTime = (int)TestRecipe.TotalTime.TotalMinutes,
                CreatedOn = TestRecipe.CreatedOn.ToString("dd-MM-yyyy"),
                CategoryName = TestRecipe.RecipeCategory.Name
            };

            // Act
            var actualResult = await this.recipeService.GetForDeleteByIdAsync(TestRecipe.Id.ToString());

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actualResult, Is.Not.Null);
                Assert.That(actualResult, Is.InstanceOf<RecipeDeleteViewModel>());

                if (actualResult != null)
                {
                    Assert.That(expctedResult.Id, Is.EqualTo(actualResult.Id));
                    Assert.That(expctedResult.Title, Is.EqualTo(actualResult.Title));
                    Assert.That(expctedResult.ImageUrl, Is.EqualTo(actualResult.ImageUrl));
                    Assert.That(expctedResult.Servings, Is.EqualTo(actualResult.Servings));
                    Assert.That(expctedResult.TotalTime, Is.EqualTo(actualResult.TotalTime));
                    Assert.That(expctedResult.CreatedOn, Is.EqualTo(actualResult.CreatedOn));
                    Assert.That(expctedResult.CategoryName, Is.EqualTo(actualResult.CategoryName));
                }
            });
        }

        [Test]
        public async Task AllAsyncShouldReturnCorrectModel()
        {
            // Arrange
            AllRecipesQueryModel testModel = new AllRecipesQueryModel()
            {
                SearchString = TestRecipe.Title,
                Category = TestRecipe.RecipeCategory.Name,
                RecipeSorting = Web.ViewModels.Recipe.Enums.RecipeSorting.Newest,
            };

            AllRecipesFilteredAndPagedServiceModel expectedResult = new AllRecipesFilteredAndPagedServiceModel()
            {
                TotalRecipesCount = data.Recipes.Where(r => r.Title == TestRecipe.Title).Count(),
                Recipes = new List<RecipeAllViewModel>()
                {
                    new RecipeAllViewModel()
                    {
                        Id = TestRecipe.Id.ToString(),
                        ImageUrl = TestRecipe.ImageUrl,
                        Title = TestRecipe.Title,
                        Description = TestRecipe.Description,
                        Category = new RecipeCategorySelectViewModel()
                        {
                            Id = TestRecipe.RecipeCategoryId,
                            Name = TestRecipe.RecipeCategory.Name
                        },
                        Servings = TestRecipe.Servings,
                        CookingTime = String.Format(@"{0}h {1}min", TestRecipe.TotalTime.Hours.ToString(), TestRecipe.TotalTime.Minutes.ToString()),
                    }

                }              
            };

            // Act
            var actualResult = await this.recipeService.AllAsync(testModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actualResult, Is.Not.Null);
                Assert.That(actualResult, Is.InstanceOf<AllRecipesFilteredAndPagedServiceModel>());

                if (actualResult != null)
                {
                    Assert.That(expectedResult.TotalRecipesCount, Is.EqualTo(actualResult.TotalRecipesCount));
                    Assert.That(actualResult.Recipes, Is.InstanceOf<ICollection<RecipeAllViewModel>>());
                    Assert.That(actualResult.Recipes.Count, Is.EqualTo(expectedResult.Recipes.Count));

                    IEnumerator<RecipeAllViewModel> expectedEnumerator = expectedResult.Recipes.GetEnumerator();
                    IEnumerator<RecipeAllViewModel> actualEnumerator = actualResult.Recipes.GetEnumerator();

                    while (expectedEnumerator.MoveNext() && actualEnumerator.MoveNext())
                    {
                        Assert.That(expectedEnumerator.Current.Id, Is.EqualTo(actualEnumerator.Current.Id));
                        Assert.That(expectedEnumerator.Current.ImageUrl, Is.EqualTo(actualEnumerator.Current.ImageUrl));
                        Assert.That(expectedEnumerator.Current.Title, Is.EqualTo(actualEnumerator.Current.Title));
                        Assert.That(expectedEnumerator.Current.Description, Is.EqualTo(actualEnumerator.Current.Description));
                        Assert.That(expectedEnumerator.Current.Category.Id, Is.EqualTo(actualEnumerator.Current.Category.Id));
                        Assert.That(expectedEnumerator.Current.Category.Name, Is.EqualTo(actualEnumerator.Current.Category.Name));
                        Assert.That(expectedEnumerator.Current.Servings, Is.EqualTo(actualEnumerator.Current.Servings));
                        Assert.That(expectedEnumerator.Current.CookingTime, Is.EqualTo(actualEnumerator.Current.CookingTime));
                    }
                }
            });
        }

        [Test]
        public async Task AddAsyncShouldAddRecipe()
        {
            // Arrange
            int recipesInDbBefore = data.Recipes.Count();

            RecipeAddFormModel newRecipeTestModel = new RecipeAddFormModel()
            {
                Title = NewRecipe.Title,
                Description = NewRecipe.Description,
                Instructions = NewRecipe.Instructions,
                Servings = NewRecipe.Servings,
                CookingTimeMinutes = NewRecipe.TotalTime.Minutes,
                ImageUrl = NewRecipe.ImageUrl,
                RecipeCategoryId = NewRecipe.RecipeCategoryId,
                RecipeIngredients = new List<RecipeIngredientFormViewModel>()
                {
                    new RecipeIngredientFormViewModel()
                    {
                        Name = NewIngredient.Name,
                        Qty = NewRecipeIngredient.Qty,
                        MeasureId = NewRecipeIngredient.MeasureId,
                        SpecificationId = NewRecipeIngredient.SpecificationId,
                    }
                }
            };

            // Act
            string recipeOwner = TestUser.Id.ToString();
            string newRecipeId = await this.recipeService.AddAsync(newRecipeTestModel, recipeOwner);
            
            // Assert
            int recipesInDbAfter = data.Recipes.Count();
            var newrecipeInDb = data.Recipes.Find(Guid.Parse(newRecipeId));
            Assert.That(recipesInDbAfter, Is.EqualTo(recipesInDbBefore + 1));
            Assert.That(newrecipeInDb.Id.ToString(), Is.EqualTo(newRecipeId));
            Assert.That(newrecipeInDb.Title, Is.EqualTo(NewRecipe.Title));
            Assert.That(newrecipeInDb.Description, Is.EqualTo(NewRecipe.Description));
            Assert.That(newrecipeInDb.Instructions, Is.EqualTo(NewRecipe.Instructions));
            Assert.That(newrecipeInDb.Servings, Is.EqualTo(NewRecipe.Servings));
            Assert.That(newrecipeInDb.TotalTime, Is.EqualTo(NewRecipe.TotalTime));
            Assert.That(newrecipeInDb.ImageUrl, Is.EqualTo(NewRecipe.ImageUrl));
            Assert.That(newrecipeInDb.RecipeCategoryId, Is.EqualTo(NewRecipe.RecipeCategoryId));

            
            Assert.That(newrecipeInDb.RecipesIngredients.Count, Is.EqualTo(NewRecipe.RecipesIngredients.Count));
            Assert.That(newrecipeInDb.RecipesIngredients, Is.InstanceOf<ICollection<RecipeIngredient>>());

            IEnumerator<RecipeIngredient> expectedEnumerator = NewRecipe.RecipesIngredients.GetEnumerator();
            IEnumerator<RecipeIngredient> actualEnumerator = newrecipeInDb.RecipesIngredients.GetEnumerator();

            while (expectedEnumerator.MoveNext() && actualEnumerator.MoveNext())
            {
                //Assert.That(expectedEnumerator.Current.RecipeId, Is.EqualTo(actualEnumerator.Current.RecipeId));
                Assert.That(expectedEnumerator.Current.IngredientId, Is.EqualTo(actualEnumerator.Current.IngredientId));
                Assert.That(expectedEnumerator.Current.Qty, Is.EqualTo(actualEnumerator.Current.Qty));
                Assert.That(expectedEnumerator.Current.MeasureId, Is.EqualTo(actualEnumerator.Current.MeasureId));
                Assert.That(expectedEnumerator.Current.SpecificationId, Is.EqualTo(actualEnumerator.Current.SpecificationId));
            }
           
        }

        [Test]
        public async Task EditAsyncShouldEditRecipeCorrectly()
        {
            // Arrange 
            var recipeToEdit = TestRecipe;

            string editedTitle = "Test Recipe Title Edited!";
            string editedDescrption = "Test Recipe Description Edited";
            string editedInstructions = "Test Recipe Instructions EDITED";
            int editedServings = 10;
            int editedCookingTime = 15;
            string editedUrl = "https://picsum.photos/536/354";
            int editedRecipeCategory = 2;

            // Here wi change the data including the recipe-ingredients
            RecipeEditFormModel recipeModelToEdit = new RecipeEditFormModel()
            {
                Id = TestRecipe.Id.ToString(),
                Title = editedTitle,
                Description = editedDescrption,
                Instructions = editedInstructions,
                Servings = editedServings,
                CookingTimeMinutes = editedCookingTime,
                ImageUrl = editedUrl,
                RecipeCategoryId = editedRecipeCategory,
                RecipeIngredients = new List<RecipeIngredientFormViewModel>()
                { 
                    new RecipeIngredientFormViewModel()
                    {
                        Name = NewIngredient.Name,
                        Qty = NewRecipeIngredient.Qty,
                        MeasureId = NewRecipeIngredient.MeasureId,
                        SpecificationId = NewRecipeIngredient.SpecificationId
                    }
                }
            };
           
            // Act            
            await this.recipeService.EditAsync(recipeModelToEdit);

            // Assert              
            Assert.That(recipeToEdit.Title, Is.EqualTo(editedTitle));
            Assert.That(recipeToEdit.Description, Is.EqualTo(editedDescrption));
            Assert.That(recipeToEdit.Instructions, Is.EqualTo(editedInstructions));
            Assert.That(recipeToEdit.Servings, Is.EqualTo(editedServings));
            Assert.That(recipeToEdit.TotalTime, Is.EqualTo(TimeSpan.FromMinutes(editedCookingTime)));
            Assert.That(recipeToEdit.ImageUrl, Is.EqualTo(editedUrl));
            Assert.That(recipeToEdit.RecipeCategoryId, Is.EqualTo(editedRecipeCategory));

            if (recipeToEdit.RecipesIngredients.Any())
            {
                Assert.That(recipeToEdit.RecipesIngredients.Count, Is.EqualTo(recipeModelToEdit.RecipeIngredients.Count));
                Assert.That(recipeToEdit.RecipesIngredients, Is.InstanceOf<ICollection<RecipeIngredient>>());

                IEnumerator<RecipeIngredientFormViewModel> expectedEnumerator = recipeModelToEdit.RecipeIngredients.GetEnumerator();
                IEnumerator<RecipeIngredient> actualEnumerator = recipeToEdit.RecipesIngredients.GetEnumerator();

                while (expectedEnumerator.MoveNext() && actualEnumerator.MoveNext())
                {
                    Assert.That(expectedEnumerator.Current.Name, Is.EqualTo(actualEnumerator.Current.Ingredient.Name));
                    Assert.That(expectedEnumerator.Current.Qty, Is.EqualTo(actualEnumerator.Current.Qty));
                    Assert.That(expectedEnumerator.Current.MeasureId, Is.EqualTo(actualEnumerator.Current.MeasureId));
                    Assert.That(expectedEnumerator.Current.SpecificationId, Is.EqualTo(actualEnumerator.Current.SpecificationId));
                }
            } 
            

        }

        [Test]
        public async Task DetailsByIdAsyncShouldReturnCorrectModelAndData()
        {
            // Arrange
            var recipe = TestRecipe;

            RecipeDetailsViewModel expectedModel = new RecipeDetailsViewModel()
            {
                Id = recipe.Id.ToString(),
                Title = recipe.Title,
                Description = recipe.Description,
                Instructions = recipe.Instructions,
                Servings = recipe.Servings,
                TotalTime = recipe.TotalTime.ToString(@"hh\:mm"),
                ImageUrl = recipe.ImageUrl,
                CreatedOn = recipe.CreatedOn.ToString("dd-MM-yyyy"),
                CategoryName = recipe.RecipeCategory.Name,
                MainIngredients = recipe.RecipesIngredients
                        .OrderBy(ri => ri.Ingredient.IngredientCategoryId)
                        .ThenBy(ri => ri.Ingredient.Name)
                        .Where(ri => MainIngredientsCategories.Contains(ri.Ingredient.IngredientCategoryId))
                        .Select(ri => new RecipeIngredientDetailsViewModel()
                        {
                            Name = ri.Ingredient.Name,
                            Qty = ri.Qty,
                            Measure = ri.Measure.Name,
                            Specification = ri.Specification.Description,
                        }).ToList(),
                SecondaryIngredients = recipe.RecipesIngredients
                        .OrderBy(ri => ri.Ingredient.IngredientCategoryId)
                        .ThenBy(ri => ri.Ingredient.Name)
                        .Where(ri => SecondaryIngredientsCategories.Contains(ri.Ingredient.IngredientCategoryId))
                        .Select(ri => new RecipeIngredientDetailsViewModel()
                        {
                            Name = ri.Ingredient.Name,
                            Qty = ri.Qty,
                            Measure = ri.Measure.Name,
                            Specification = ri.Specification.Description,
                        }).ToList(),
                AdditionalIngredients = recipe.RecipesIngredients
                        .OrderBy(ri => ri.Ingredient.IngredientCategoryId)
                        .ThenBy(ri => ri.Ingredient.Name)
                        .Where(ri => AdditionalIngredientsCategories.Contains(ri.Ingredient.IngredientCategoryId))
                        .Select(ri => new RecipeIngredientDetailsViewModel()
                        {
                            Name = ri.Ingredient.Name,
                            Qty = ri.Qty,
                            Measure = ri.Measure.Name,
                            Specification = ri.Specification.Description,
                        }).ToList(),
            };

            // Act
            var resultModel = await this.recipeService.DetailsByIdAsync(recipe.Id.ToString());

            // Assert
            Assert.That(resultModel, Is.InstanceOf<RecipeDetailsViewModel>());  

            Assert.That(resultModel.Id, Is.EqualTo(recipe.Id.ToString()));
            Assert.That(resultModel.Title, Is.EqualTo(recipe.Title));
            Assert.That(resultModel.Description, Is.EqualTo(recipe.Description));
            Assert.That(resultModel.Instructions, Is.EqualTo(recipe.Instructions));
            Assert.That(resultModel.Servings, Is.EqualTo(recipe.Servings));
            Assert.That(resultModel.TotalTime, Is.EqualTo(recipe.TotalTime.ToString(@"hh\:mm")));
            Assert.That(resultModel.ImageUrl, Is.EqualTo(recipe.ImageUrl));
            Assert.That(resultModel.CreatedOn, Is.EqualTo(recipe.CreatedOn.ToString("dd-MM-yyyy")));
            Assert.That(resultModel.CategoryName, Is.EqualTo(recipe.RecipeCategory.Name));
        }

        public async Task DeleteByIdShouldWorkCorrect()
        {

        }
    }
}
