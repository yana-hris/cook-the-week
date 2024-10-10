using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookTheWeek.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                },
                comment: "The Application User");

            migrationBuilder.CreateTable(
                name: "IngredientCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Key identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Ingredient Category Name")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngredientCategories", x => x.Id);
                },
                comment: "Ingredient Category");

            migrationBuilder.CreateTable(
                name: "Measures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Key Identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Measure Name")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measures", x => x.Id);
                },
                comment: "Measure of Recipe-Ingredient");

            migrationBuilder.CreateTable(
                name: "RecipeCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Key Identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Recipe Category Name")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeCategories", x => x.Id);
                },
                comment: "Recipes Category");

            migrationBuilder.CreateTable(
                name: "Specifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Key Identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Specification Description")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specifications", x => x.Id);
                },
                comment: "Specification of Recipe-Ingredient");

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MealPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Meal Plan Key Identifier"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Meal Plan Name"),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Meal Plan Owner Key Identifier"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()", comment: "Meal Plan Start Date"),
                    IsFinished = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Meal Plan Completion Identifier")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MealPlans_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Meal Plan");

            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Key Identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Ingredient Name"),
                    CategoryId = table.Column<int>(type: "int", nullable: false, comment: "Ingredient Category Key Identifier")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ingredients_IngredientCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "IngredientCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Ingredient");

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Key Indetifier"),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Recipe Creator"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Recipe Title"),
                    Description = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: true, comment: "Recipe Description"),
                    Servings = table.Column<int>(type: "int", nullable: false, comment: "Recipe Serving Size"),
                    TotalTime = table.Column<TimeSpan>(type: "time", nullable: false, comment: "Recipe Cooking Time"),
                    ImageUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false, comment: "Recipe Image Link"),
                    CategoryId = table.Column<int>(type: "int", nullable: false, comment: "Recipe Category Key Identifier"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()", comment: "Date and Time of a Recipe Creation"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Soft Delete the Recipe"),
                    IsSiteRecipe = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Indicator for Recipe Ownership")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recipes_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Recipes_RecipeCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "RecipeCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Recipe");

            migrationBuilder.CreateTable(
                name: "FavoriteRecipes",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "User Key Identifier"),
                    RecipeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Recipe Key Identifier"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Soft Delete the Recipe Like when the Recipe is deleted")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteRecipes", x => new { x.UserId, x.RecipeId });
                    table.ForeignKey(
                        name: "FK_FavoriteRecipes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteRecipes_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Users` Favourite Recipes (likes)");

            migrationBuilder.CreateTable(
                name: "Meals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Meal Key Identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Recipe Key Identifier"),
                    ServingSize = table.Column<int>(type: "int", nullable: false, comment: "Meal Serving Size"),
                    CookDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Meal Cook Date"),
                    IsCooked = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Meal completion Identifier"),
                    MealPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Meal Plan Key Identifier"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Soft Delete the Meal when the Recipe is deleted")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Meals_MealPlans_MealPlanId",
                        column: x => x.MealPlanId,
                        principalTable: "MealPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Meals_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Meal");

            migrationBuilder.CreateTable(
                name: "RecipesIngredients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Unique Recipe Ingredient Key identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Key Identifier for Recipe"),
                    IngredientId = table.Column<int>(type: "int", nullable: false, comment: "Key Identifier for Ingredient"),
                    Qty = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "Quantity of Ingredient in Recipe"),
                    MeasureId = table.Column<int>(type: "int", nullable: false, comment: "Measure Key Identifier"),
                    SpecificationId = table.Column<int>(type: "int", nullable: true, comment: "Key identifier for Specification"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Soft Delete the RecipeIngredient when the Recipe is deleted")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipesIngredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipesIngredients_Ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecipesIngredients_Measures_MeasureId",
                        column: x => x.MeasureId,
                        principalTable: "Measures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecipesIngredients_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipesIngredients_Specifications_SpecificationId",
                        column: x => x.SpecificationId,
                        principalTable: "Specifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                },
                comment: "Recipe Ingredient");

            migrationBuilder.CreateTable(
                name: "Steps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Step Key Identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Recipe Key Identifier"),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false, comment: "Cooking Step Instructions"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Soft Delete the Step upon Recipe Deletion")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Steps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Steps_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Cooking Step");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteRecipes_RecipeId",
                table: "FavoriteRecipes",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_CategoryId",
                table: "Ingredients",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MealPlans_OwnerId",
                table: "MealPlans",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Meals_MealPlanId",
                table: "Meals",
                column: "MealPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Meals_RecipeId",
                table: "Meals",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_CategoryId",
                table: "Recipes",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_OwnerId",
                table: "Recipes",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipesIngredients_IngredientId",
                table: "RecipesIngredients",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipesIngredients_MeasureId",
                table: "RecipesIngredients",
                column: "MeasureId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipesIngredients_RecipeId_IngredientId_MeasureId",
                table: "RecipesIngredients",
                columns: new[] { "RecipeId", "IngredientId", "MeasureId" },
                unique: true,
                filter: "[SpecificationId] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RecipesIngredients_RecipeId_IngredientId_MeasureId_SpecificationId",
                table: "RecipesIngredients",
                columns: new[] { "RecipeId", "IngredientId", "MeasureId", "SpecificationId" },
                unique: true,
                filter: "[SpecificationId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RecipesIngredients_SpecificationId",
                table: "RecipesIngredients",
                column: "SpecificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Steps_RecipeId",
                table: "Steps",
                column: "RecipeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "FavoriteRecipes");

            migrationBuilder.DropTable(
                name: "Meals");

            migrationBuilder.DropTable(
                name: "RecipesIngredients");

            migrationBuilder.DropTable(
                name: "Steps");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "MealPlans");

            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "Measures");

            migrationBuilder.DropTable(
                name: "Specifications");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropTable(
                name: "IngredientCategories");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "RecipeCategories");
        }
    }
}
