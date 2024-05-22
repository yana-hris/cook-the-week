namespace CookTheWeek.Services.Tests.UnitTests
{
    using Microsoft.EntityFrameworkCore;

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

        [SetUp]
        public void SetUp()
        {
            this.recipeService = new RecipeService(data);
        }

        [Test]
        public async Task ExistsByIdAsync_ShouldReturn_True_If_Exists()
        {
            string expectedId = TestRecipe.Id.ToString();

            bool result = await this.recipeService.ExistsByIdAsync(expectedId);

            Assert.True(result);
        }

        [Test]
        public async Task ExistsByIdAsync_ShouldReturn_False_If_DoesntExists()
        {
            string expectedId = Guid.NewGuid().ToString();

            bool result = await this.recipeService.ExistsByIdAsync(expectedId);

            Assert.False(result);
        }

        [Test]
        public async Task GetForEditByIdAsync_ShouldReturn_CorrectData()
        {
            // Arrange
            RecipeEditFormModel expctedResult = new()
            {
                Id = TestRecipe.Id.ToString(),
                Title = TestRecipe.Title,
                Description = TestRecipe.Description,
                //Instructions = TestRecipe.Instructions,
                Servings = TestRecipe.Servings,
                CookingTimeMinutes = (int)TestRecipe.TotalTime.TotalMinutes,
                ImageUrl = TestRecipe.ImageUrl,
                RecipeCategoryId = TestRecipe.CategoryId,
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
                    //Assert.That(expctedResult.Instructions, Is.EqualTo(actualResult.Instructions));
                    Assert.That(expctedResult.Servings, Is.EqualTo(actualResult.Servings));
                    Assert.That(expctedResult.CookingTimeMinutes, Is.EqualTo(actualResult.CookingTimeMinutes));
                    Assert.That(expctedResult.ImageUrl, Is.EqualTo(actualResult.ImageUrl));
                    Assert.That(expctedResult.RecipeCategoryId, Is.EqualTo(actualResult.RecipeCategoryId));
                }
            });
        }

        [Test]
        public async Task GetForDeleteByIdAsync_ShouldReturn_CorrectData()
        {
            // Arrange
            RecipeDeleteViewModel expctedResult = new()
            {
                Id = TestRecipe.Id.ToString(),
                Title = TestRecipe.Title,
                ImageUrl = TestRecipe.ImageUrl,
                Servings = TestRecipe.Servings,
                TotalTime = (int)TestRecipe.TotalTime.TotalMinutes,
                CreatedOn = TestRecipe.CreatedOn.ToString("dd-MM-yyyy"),
                CategoryName = TestRecipe.Category.Name
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
        public async Task AllAsync_ShouldReturn_CorrectModel()
        {
            // Arrange
            AllRecipesQueryModel testModel = new()
            {
                SearchString = TestRecipe.Title,
                Category = TestRecipe.Category.Name,
                RecipeSorting = Web.ViewModels.Recipe.Enums.RecipeSorting.Newest,
            };

            AllRecipesFilteredAndPagedServiceModel expectedResult = new()
            {
                TotalRecipesCount = data.Recipes.Where(r => r.Title == TestRecipe.Title).Count(),
                Recipes = new List<RecipeAllViewModel>()
                {
                    new()
                    {
                        Id = TestRecipe.Id.ToString(),
                        ImageUrl = TestRecipe.ImageUrl,
                        Title = TestRecipe.Title,
                        Description = TestRecipe.Description,
                        Category = new RecipeCategorySelectViewModel()
                        {
                            Id = TestRecipe.CategoryId,
                            Name = TestRecipe.Category.Name
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
        public async Task AddAsync_ShouldAdd_RecipeCorrectly()
        {
            // Arrange
            int recipesInDbBefore = data.Recipes.Count();

            RecipeAddFormModel newRecipeTestModel = new()
            {
                Title = NewRecipe.Title,
                Description = NewRecipe.Description,
                //Instructions = NewRecipe.Instructions,
                Servings = NewRecipe.Servings,
                CookingTimeMinutes = NewRecipe.TotalTime.Minutes,
                ImageUrl = NewRecipe.ImageUrl,
                RecipeCategoryId = NewRecipe.CategoryId,
                RecipeIngredients = new List<RecipeIngredientFormViewModel>()
                {
                    new()
                    {
                        Name = NewIngredient.Name,
                        Qty = new RecipeIngredientQtyFormModel() {QtyDecimal = NewRecipeIngredient.Qty },
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
            //Assert.That(newrecipeInDb.Instructions, Is.EqualTo(NewRecipe.Instructions));
            Assert.That(newrecipeInDb.Servings, Is.EqualTo(NewRecipe.Servings));
            Assert.That(newrecipeInDb.TotalTime, Is.EqualTo(NewRecipe.TotalTime));
            Assert.That(newrecipeInDb.ImageUrl, Is.EqualTo(NewRecipe.ImageUrl));
            Assert.That(newrecipeInDb.CategoryId, Is.EqualTo(NewRecipe.CategoryId));

            
            Assert.That(newrecipeInDb.RecipesIngredients.Count, Is.EqualTo(NewRecipe.RecipesIngredients.Count));
            Assert.That(newrecipeInDb.RecipesIngredients, Is.InstanceOf<ICollection<RecipeIngredient>>());

            IEnumerator<RecipeIngredient> expectedEnumerator = NewRecipe.RecipesIngredients.GetEnumerator();
            IEnumerator<RecipeIngredient> actualEnumerator = newrecipeInDb.RecipesIngredients.GetEnumerator();

            while (expectedEnumerator.MoveNext() && actualEnumerator.MoveNext())
            {
                Assert.That(expectedEnumerator.Current.IngredientId, Is.EqualTo(actualEnumerator.Current.IngredientId));
                Assert.That(expectedEnumerator.Current.Qty, Is.EqualTo(actualEnumerator.Current.Qty));
                Assert.That(expectedEnumerator.Current.MeasureId, Is.EqualTo(actualEnumerator.Current.MeasureId));
                Assert.That(expectedEnumerator.Current.SpecificationId, Is.EqualTo(actualEnumerator.Current.SpecificationId));
            }
           
        }

        [Test]
        public async Task EditAsync_ShouldEdit_RecipeCorrectly()
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
            RecipeEditFormModel recipeModelToEdit = new()
            {
                Id = TestRecipe.Id.ToString(),
                Title = editedTitle,
                Description = editedDescrption,
                //Instructions = editedInstructions,
                Servings = editedServings,
                CookingTimeMinutes = editedCookingTime,
                ImageUrl = editedUrl,
                RecipeCategoryId = editedRecipeCategory,
                RecipeIngredients = new List<RecipeIngredientFormViewModel>()
                { 
                    new()
                    {
                        Name = NewIngredient.Name,
                        Qty = new RecipeIngredientQtyFormModel()
                        {
                            QtyDecimal = NewRecipeIngredient.Qty,

                        },
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
            //Assert.That(recipeToEdit.Instructions, Is.EqualTo(editedInstructions));
            Assert.That(recipeToEdit.Servings, Is.EqualTo(editedServings));
            Assert.That(recipeToEdit.TotalTime, Is.EqualTo(TimeSpan.FromMinutes(editedCookingTime)));
            Assert.That(recipeToEdit.ImageUrl, Is.EqualTo(editedUrl));
            Assert.That(recipeToEdit.CategoryId, Is.EqualTo(editedRecipeCategory));

            if (recipeToEdit.RecipesIngredients.Any())
            {
                Assert.That(recipeToEdit.RecipesIngredients.Count, Is.EqualTo(recipeModelToEdit.RecipeIngredients.Count));
                Assert.That(recipeToEdit.RecipesIngredients, Is.InstanceOf<ICollection<RecipeIngredient>>());

                IEnumerator<RecipeIngredientFormViewModel> expectedEnumerator = recipeModelToEdit.RecipeIngredients.GetEnumerator();
                IEnumerator<RecipeIngredient> actualEnumerator = recipeToEdit.RecipesIngredients.GetEnumerator();

                while (expectedEnumerator.MoveNext() && actualEnumerator.MoveNext())
                {
                    Assert.That(expectedEnumerator.Current.Name, Is.EqualTo(actualEnumerator.Current.Ingredient.Name));
                    Assert.That(expectedEnumerator.Current.Qty.QtyDecimal, Is.EqualTo(actualEnumerator.Current.Qty));
                    Assert.That(expectedEnumerator.Current.MeasureId, Is.EqualTo(actualEnumerator.Current.MeasureId));
                    Assert.That(expectedEnumerator.Current.SpecificationId, Is.EqualTo(actualEnumerator.Current.SpecificationId));
                }
            } 
            

        }

        [Test]
        public async Task DetailsByIdAsync_ShouldReturn_CorrectModelAndData()
        {
            // Arrange
            var recipe = TestRecipe;

            RecipeDetailsViewModel expectedModel = new()
            {
                Id = recipe.Id.ToString(),
                Title = recipe.Title,
                Description = recipe.Description,
                //Instructions = recipe.Instructions,
                Servings = recipe.Servings,
                TotalTime = recipe.TotalTime,
                ImageUrl = recipe.ImageUrl,
                CreatedOn = recipe.CreatedOn.ToString("dd-MM-yyyy"),
                CategoryName = recipe.Category.Name,
                DiaryMeatSeafood = recipe.RecipesIngredients
                        .OrderBy(ri => ri.Ingredient.CategoryId)
                        .ThenBy(ri => ri.Ingredient.Name)
                        .Where(ri => DiaryMeatSeafoodIngredientCategories.Contains(ri.Ingredient.CategoryId))
                        .Select(ri => new RecipeIngredientDetailsViewModel()
                        {
                            Name = ri.Ingredient.Name,
                            Qty = ri.Qty,
                            Measure = ri.Measure.Name,
                            Specification = ri.Specification.Description,
                        }).ToList(),
                Produce = recipe.RecipesIngredients
                        .OrderBy(ri => ri.Ingredient.CategoryId)
                        .ThenBy(ri => ri.Ingredient.Name)
                        .Where(ri => ProduceIngredientCategories.Contains(ri.Ingredient.CategoryId))
                        .Select(ri => new RecipeIngredientDetailsViewModel()
                        {
                            Name = ri.Ingredient.Name,
                            Qty = ri.Qty,
                            Measure = ri.Measure.Name,
                            Specification = ri.Specification.Description,
                        }).ToList(),
                Legumes = recipe.RecipesIngredients
                        .OrderBy(ri => ri.Ingredient.CategoryId)
                        .ThenBy(ri => ri.Ingredient.Name)
                        .Where(ri => NutsSeedsAndOthersIngredientCategories.Contains(ri.Ingredient.CategoryId))
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

            Assert.That(resultModel.Id, Is.EqualTo(expectedModel.Id.ToString()));
            Assert.That(resultModel.Title, Is.EqualTo(expectedModel.Title));
            Assert.That(resultModel.Description, Is.EqualTo(expectedModel.Description));
            //Assert.That(resultModel.Instructions, Is.EqualTo(expectedModel.Instructions));
            Assert.That(resultModel.Servings, Is.EqualTo(expectedModel.Servings));
            Assert.That(resultModel.TotalTime, Is.EqualTo(expectedModel.TotalTime));
            Assert.That(resultModel.ImageUrl, Is.EqualTo(expectedModel.ImageUrl));
            Assert.That(resultModel.CreatedOn, Is.EqualTo(expectedModel.CreatedOn));
            Assert.That(resultModel.CategoryName, Is.EqualTo(expectedModel.CategoryName));
        }

        [Test]
        public async Task DeleteById_ShouldWork_Correctly()
        {
            // Arrange: get the current recipes count
            int totalRecipesBeforeDelete = data.Recipes.Where(r => r.IsDeleted == false).Count();

            // Act
            await this.recipeService.DeleteByIdAsync(TestRecipe.Id.ToString());

            // Assert the recipe is SOFT-deleted
            Assert.That(data.Recipes.Where(r => r.IsDeleted == false).Count(), Is.EqualTo(totalRecipesBeforeDelete - 1));
            var recipeInDb = await data.Recipes.FirstOrDefaultAsync(r => r.IsDeleted == false && r.Id == TestRecipe.Id);
            Assert.IsNull(recipeInDb);

            // Assert all recipe-ingredients are correctly erased
            foreach (var ri in TestRecipeRecipeIngredients)
            {
                int ingredientId = ri.IngredientId;
                var recipeIngredientInDb = await data
                    .RecipesIngredients
                    .FirstOrDefaultAsync(ri => ri.IngredientId == ingredientId && ri.RecipeId == TestRecipe.Id);

                Assert.IsNull(recipeIngredientInDb);
            }

            // Assert the user-likes are erased for this Recipe
            var likes = await data.FavoriteRecipes.AnyAsync(fr => fr.RecipeId == TestRecipe.Id);
            Assert.IsFalse(likes);

            // Assert user meals are deleted as well
            var meals = await data.Meals.AnyAsync(m => m.RecipeId == TestRecipe.Id);
            Assert.IsFalse(meals);
        }

        [Test]
        public async Task AllAddedByUserAsync_ShouldReturn_CorrectData()
        {
            // Arrange
            string userId = TestRecipe.OwnerId;
            int expectedUserRecipesCount = data.Recipes.Where(r => r.OwnerId == userId).Count();

            ICollection<RecipeAllViewModel> expectedResult = data.Recipes.Where(r => r.OwnerId == userId)
                .Select(r => new RecipeAllViewModel()
                {
                    Id = r.Id.ToString(),
                    ImageUrl = r.ImageUrl,
                    Title = r.Title,
                    Description = r.Description,
                    Category = new RecipeCategorySelectViewModel()
                    {
                        Id = r.CategoryId,
                        Name = r.Category.Name
                    },
                    Servings = r.Servings,
                    CookingTime = String.Format(@"{0}h {1}min", r.TotalTime.Hours.ToString(), r.TotalTime.Minutes.ToString()),
                }).ToList();
            {
                
            };

            // Act
            var actualResult = await this.recipeService.AllAddedByUserAsync(userId);

            // Assert
            Assert.That(expectedUserRecipesCount, Is.EqualTo(actualResult.Count));
            Assert.That(actualResult, Is.InstanceOf<ICollection<RecipeAllViewModel>>());

            IEnumerator<RecipeAllViewModel> expectedEnumerator = expectedResult.GetEnumerator();
            IEnumerator<RecipeAllViewModel> actualEnumerator = actualResult.GetEnumerator();

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

        [Test]
        public async Task MineCountAsync_ShouldReturn_CorrectCount()
        {
            // Arrange the result for Admin User (Admin => Mine)
            string ownerId = AdminUserId;
            int expectedCount = data.Recipes.Where(r => r.OwnerId == ownerId).Count();

            // Act
            int actualCount = await this.recipeService.MineCountAsync(ownerId);

            // Assert
            Assert.That(expectedCount, Is.EqualTo(actualCount));
        }

        [Test]
        public async Task AllCountAsync_ShouldReturn_CorrectCount()
        {
            // Arrange 
            int expectedCount = data.Recipes.Count();

            // Act
            int actualCount = await this.recipeService.AllCountAsync();

            // Assert
            Assert.That(expectedCount, Is.EqualTo(actualCount));
        }

        
    }
}
