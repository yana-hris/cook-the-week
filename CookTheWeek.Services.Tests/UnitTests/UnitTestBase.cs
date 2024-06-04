namespace CookTheWeek.Services.Tests.UnitTests
{
    using Microsoft.AspNetCore.Identity;

    using CookTheWeek.Data;
    using CookTheWeek.Data.Models;

    using static Common.GeneralApplicationConstants;

    public class UnitTestBase
    {
        protected CookTheWeekDbContext data;

        public ApplicationUser AdminUser { get; private set; }
        public ApplicationUser TestUser { get; private set; }   
        public Recipe TestRecipe { get; private set; }
        public ICollection<RecipeIngredient> TestRecipeRecipeIngredients { get; private set; }
        public Recipe NewRecipe { get; private set; }
        public Ingredient TestIngredient { get; private set; }
        public Ingredient NewIngredient { get; private set; }
        public RecipeIngredient NewRecipeIngredient { get; private set; }
        public ICollection<Specification> TestSpecifications { get; private set; }
        public ICollection<Measure> TestMeasures { get; private set; }
        public ICollection<Ingredient> TestIngredients { get; private set; }
        public ICollection<RecipeCategory> TestRecipeCategories { get; private set;}
        public ICollection<IngredientCategory> TestIngredientCategories { get; private set; }
        public ICollection<FavouriteRecipe> UserLikes { get; private set; }

        [SetUp]
        public void SetUpBase()
        {
            data = DatabaseMock.Instance;
            SeedDatabase();
        }

        [TearDown]
        public void TearDownBase() 
            => data.Dispose();

        private void SeedDatabase() 
        {
            // Users
            PasswordHasher<ApplicationUser> hasher = new();

            AdminUser = new ApplicationUser()
            {
                Id = Guid.Parse(AdminUserId),
                UserName = AdminUserUsername,
                NormalizedUserName = AdminUserUsername.ToUpper(),
                Email = AdminUserEmail,
                NormalizedEmail = AdminUserEmail.ToUpper(),
                ConcurrencyStamp = "caf271d7-0ba7-4ab1-8d8d-6d0e3711c27d",
                SecurityStamp = "ca32c787-626e-4234-a4e4-8c94d85a2b1c",
                EmailConfirmed = true,
            };

            AdminUser.PasswordHash = hasher.HashPassword(AdminUser, AdminUserPassword);

            TestUser = new ApplicationUser()
            {
                Id = Guid.Parse("65fc0e0d-6572-4ec6-a853-c633d9f28c9e"),
                UserName = "testUser",
                NormalizedUserName = "TESTUSER",
                Email = "user@user.bg",
                NormalizedEmail = "USER@USER.BG",
                ConcurrencyStamp = "8b51706e-f6e8-4dae-b240-54f856fb3004",
                SecurityStamp = "f6af46f5-74ba-43dc-927b-ad83497d0387",
                EmailConfirmed = true,
            };

            TestUser.PasswordHash = hasher.HashPassword(TestUser, "123123");

            data.Users.Add(AdminUser);
            data.Users.Add(TestUser);

            // Recipes
            TestRecipe = new Recipe()
            {
                Id = Guid.Parse("09bd36a4-1e9f-47e2-ad5e-abd474d580ba"),
                OwnerId = Guid.Parse(AdminUserId),
                Title = "Test Recipe Title",
                Description = "Test Recipe Description",
                //Instructions = "Test Recipe Instructions",
                Servings = 4,
                TotalTime = TimeSpan.FromMinutes(10.0),
                ImageUrl = "https://cdn.pixabay.com/photo/2014/06/03/19/38/board-361516_640.jpg",
                CategoryId = 1
            };

            data.Recipes.Add(TestRecipe);

            // User likes
            UserLikes = new List<FavouriteRecipe>()
            {
                new()
                {
                    RecipeId = Guid.Parse("09bd36a4-1e9f-47e2-ad5e-abd474d580ba"),
                    UserId = Guid.Parse(AdminUserId)
                }

            };

            data.FavoriteRecipes.AddRange(UserLikes);

            // Will not be added to th in-memory DB at first
            NewRecipe = new Recipe()
            {
                Title = "Test Newly Added Recipe Title",
                Description = "Test Newly Added Recipe Description",
                //Instructions = "Test Newly Added Recipe Instructions",
                Servings = 2,
                TotalTime = TimeSpan.FromMinutes(20.0),
                ImageUrl = "https://cdn.pixabay.com/photo/2014/06/03/19/38/road-sign-361514_960_720.png",
                CategoryId = 2
            };

            // Ingredients
            TestIngredient = new Ingredient()
            {
                Id = 1,
                Name = "Ingredient 1",
                CategoryId = 1
            };

            TestIngredients = new List<Ingredient>()
            {
                TestIngredient,
                new()
                {
                    Id = 2,
                    Name = "Ingredient 2",
                    CategoryId = 2
                },
                new()
                {
                    Id = 3,
                    Name = "Ingredient 3",
                    CategoryId = 3
                },
                new()
                {
                    Id = 4,
                    Name = "Ingredient to Delete",
                    CategoryId = 1
                },
            };

            data.Ingredients.AddRange(TestIngredients);

            // Recipe Ingredients
            TestRecipe.RecipesIngredients = new List<RecipeIngredient>()
            {               
                new()
                {
                    RecipeId = Guid.Parse("09bd36a4-1e9f-47e2-ad5e-abd474d580ba"),
                    IngredientId = 2,
                    Qty = 1,
                    MeasureId = 2,
                    SpecificationId = 2
                },
                new()
                {
                    RecipeId = Guid.Parse("09bd36a4-1e9f-47e2-ad5e-abd474d580ba"),
                    IngredientId = 3,
                    Qty = 1,
                    MeasureId = 3,
                    SpecificationId = 3
                }
            };
            // Save the Recipe Ingredients for easier reference - these will not be added to the DB
            TestRecipeRecipeIngredients = new List<RecipeIngredient>()
            {                
                new()
                {
                    RecipeId = Guid.Parse("09bd36a4-1e9f-47e2-ad5e-abd474d580ba"),
                    IngredientId = 2,
                    Qty = 1,
                    MeasureId = 2,
                    SpecificationId = 2
                },
                new()
                {
                    RecipeId = Guid.Parse("09bd36a4-1e9f-47e2-ad5e-abd474d580ba"),
                    IngredientId = 3,
                    Qty = 1,
                    MeasureId = 3,
                    SpecificationId = 3
                }
            };

            NewIngredient = new Ingredient
            {
                Id = 4,
                Name = "Ingredient 4",
                CategoryId = 1
            };
            // Will not be added to the DB initially
            NewRecipeIngredient = new RecipeIngredient()
            {
                IngredientId = 1,
                Qty = 10,
                MeasureId = 2,
                SpecificationId = 3,
            };

            //NewRecipe.RecipesIngredients.Add(NewRecipeIngredient);
            // Recipe Categories
            TestRecipeCategories = new List<RecipeCategory>()
            {
                new()
                {
                   Id = 1,
                   Name = "Test Recipe Category 1"
                },
                new()
                {
                   Id = 2,
                   Name = "Test Recipe Category 2"
                },
                new()
                {
                   Id = 3,
                   Name = "Test Recipe Category 3"
                },
                new()
                {
                   Id = 4,
                   Name = "Recipe Category Without Recipes"
                },
            };

            data.RecipeCategories.AddRange(TestRecipeCategories);

            // Ingredient Categories
            TestIngredientCategories = new List<IngredientCategory>()
            {
                new()
                {
                    Id = 1,
                    Name = "Test Ingredient Category 1"
                },
                new()
                {
                    Id = 2,
                    Name = "Test Ingredient Category 2"
                },
                new()
                {
                    Id = 3,
                    Name = "Test Ingredient Category 3"
                },
                new()
                {
                    Id = 4,
                    Name = "Ingredient Category Without Ingredients"
                }
               
            };
            data.IngredientCategories.AddRange(TestIngredientCategories);

            // Measures
            TestMeasures = new List<Measure>()
            {
                new()
                {
                   Id = 1,
                   Name = "Test Measure 1"
                },
                new()
                {
                   Id = 2,
                   Name = "Test Measure 2"
                },
                new()
                {
                   Id = 3,
                   Name = "Test Measure 3"
                },
                
            };
            data.Measures.AddRange(TestMeasures);

            // Specifications
            TestSpecifications = new List<Specification>()
            {
                new() {
                    Id = 1,
                    Description = "Test Specification 1"
                },
                new() {
                    Id = 2,
                    Description = "Test Specification 2"
                },
                new() {
                    Id = 3,
                    Description = "Test Specification 3"
                }
            };
            data.Specifications.AddRange(TestSpecifications);

            data.SaveChanges();

        }
    }
}
