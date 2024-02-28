using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CookTheWeek.Data.Migrations
{
    /// <inheritdoc />
    public partial class Create : Migration
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
                });

            migrationBuilder.CreateTable(
                name: "IngredientCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngredientCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Measures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecipeCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Specifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specifications", x => x.Id);
                });

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
                name: "Ingredients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IngredientCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ingredients_IngredientCategories_IngredientCategoryId",
                        column: x => x.IngredientCategoryId,
                        principalTable: "IngredientCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Instructions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Servings = table.Column<int>(type: "int", nullable: false),
                    TotalTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    RecipeCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recipes_RecipeCategories_RecipeCategoryId",
                        column: x => x.RecipeCategoryId,
                        principalTable: "RecipeCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RecipesIngredients",
                columns: table => new
                {
                    RecipeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Ключ рецепта"),
                    IngredientId = table.Column<int>(type: "int", nullable: false, comment: "Ключ съставка"),
                    Qty = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "Количество на съставка в рецепта"),
                    MeasureId = table.Column<int>(type: "int", nullable: false),
                    SpecificationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipesIngredients", x => new { x.RecipeId, x.IngredientId });
                    table.ForeignKey(
                        name: "FK_RecipesIngredients_Ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipesIngredients_Measures_MeasureId",
                        column: x => x.MeasureId,
                        principalTable: "Measures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                        principalColumn: "Id");
                },
                comment: "Съставки към рецепти");

            migrationBuilder.InsertData(
                table: "IngredientCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Eggs, Milk and Diary products" },
                    { 2, "Meat, Ground Meat and Sausage" },
                    { 3, "Beans, Lentils and Legumes" },
                    { 4, "Flour and Baking Products" },
                    { 5, "Herbs and Spices" },
                    { 6, "Sweeteners" },
                    { 7, "Pasta" },
                    { 8, "Vegetables" },
                    { 9, "Fruits" },
                    { 10, "Fats and Oils" },
                    { 11, "Fish and Seafood" },
                    { 12, "Others" }
                });

            migrationBuilder.InsertData(
                table: "Measures",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "piece(s)" },
                    { 2, "clove(s)" },
                    { 3, "ml" },
                    { 4, "l" },
                    { 5, "g" },
                    { 6, "kg" },
                    { 7, "tsp" },
                    { 8, "tbsp" },
                    { 9, "cup" }
                });

            migrationBuilder.InsertData(
                table: "RecipeCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Breakfast" },
                    { 2, "Soup" },
                    { 3, "Salad" },
                    { 4, "Main Dish" },
                    { 5, "Appetizer" },
                    { 6, "Dessert" }
                });

            migrationBuilder.InsertData(
                table: "Specifications",
                columns: new[] { "Id", "Description" },
                values: new object[,]
                {
                    { 1, "frozen" },
                    { 2, "canned" },
                    { 3, "sliced" },
                    { 4, "pre-cooked" },
                    { 5, "grated" },
                    { 6, "fermented" },
                    { 7, "blended" },
                    { 8, "finely-chopped" },
                    { 9, "fresh" }
                });

            migrationBuilder.InsertData(
                table: "Ingredients",
                columns: new[] { "Id", "IngredientCategoryId", "Name" },
                values: new object[,]
                {
                    { 1, 2, "Beef" },
                    { 2, 2, "Pork" },
                    { 3, 2, "Lamb" },
                    { 4, 2, "Chicken" },
                    { 5, 2, "Goat" },
                    { 6, 2, "Turkey" },
                    { 7, 2, "Rabbit" },
                    { 8, 2, "Venison" },
                    { 9, 2, "Duck" },
                    { 10, 2, "Wild Game Meat" },
                    { 11, 2, "Minced Meat" },
                    { 12, 1, "Egg(s)" },
                    { 13, 1, "Milk" },
                    { 14, 1, "Yogurt" },
                    { 15, 1, "Cheese" },
                    { 16, 1, "Yellow cheese" },
                    { 17, 1, "Cheddar" },
                    { 18, 1, "Brie" },
                    { 19, 1, "Feta" },
                    { 20, 1, "Camembert" },
                    { 21, 1, "Gouda" },
                    { 22, 1, "Goat Cheese" },
                    { 23, 1, "Emmental" },
                    { 24, 1, "Parmesan" },
                    { 25, 1, "Ricotta" },
                    { 26, 1, "Gorgonzola" },
                    { 27, 1, "Cottage Cheese" },
                    { 28, 1, "Edam" },
                    { 29, 1, "Mozzarella" },
                    { 30, 1, "Mozzarella" },
                    { 31, 2, "Sausage(s)" },
                    { 32, 2, "Meatboll(s)" },
                    { 33, 3, "Lentils" },
                    { 34, 3, "Chickpeas" },
                    { 35, 3, "Green Peas" },
                    { 36, 3, "White Beans" },
                    { 37, 3, "Soybeans" },
                    { 38, 3, "Mung Beans" },
                    { 39, 3, "Red Lentils" },
                    { 40, 3, "Black Beans" },
                    { 41, 3, "Edamame" },
                    { 42, 3, "Green Lentils" },
                    { 43, 4, "White Flour" },
                    { 44, 4, "Whole Wheat Flour" },
                    { 45, 4, "Rye Flour" },
                    { 46, 4, "Spelt Flour" },
                    { 47, 4, "Almond Flour" },
                    { 48, 4, "Almond Flour" },
                    { 49, 4, "Spelled Flour" },
                    { 50, 4, "Corn Flour" },
                    { 51, 4, "Rice Flour" },
                    { 52, 5, "Salt" },
                    { 53, 5, "Black Pepper" },
                    { 54, 5, "Paprika (Red Pepper)" },
                    { 55, 5, "Turmeric" },
                    { 56, 5, "Thyme" },
                    { 57, 5, "Oregano" },
                    { 58, 5, "Rosemary" },
                    { 59, 5, "Mint" },
                    { 60, 5, "Cardamon" },
                    { 61, 5, "Curry Powder" },
                    { 62, 5, "Chilli powder" },
                    { 63, 5, "Ginger" },
                    { 64, 5, "Parsley" },
                    { 65, 5, "Bay leaf" },
                    { 66, 5, "Allspice" },
                    { 67, 5, "Basil" },
                    { 68, 5, "Clove" },
                    { 69, 5, "Cinnamon" },
                    { 70, 5, "Vanilla" },
                    { 71, 5, "Celery" },
                    { 72, 5, "Peppermint" },
                    { 73, 5, "Marjoram" },
                    { 74, 5, "Wild Garlic" },
                    { 75, 5, "Fennel (dill)" },
                    { 76, 5, "Coriander" },
                    { 77, 5, "Clary Sage" },
                    { 78, 5, "Cilantro" },
                    { 79, 5, "Chives" },
                    { 80, 6, "Sugar" },
                    { 81, 6, "Honey" },
                    { 82, 6, "Maple Syrup" },
                    { 83, 6, "Coconut Sugar" },
                    { 84, 6, "Coconut Sugar" },
                    { 85, 6, "Powdered Sugar" },
                    { 86, 6, "Brown Sugar" },
                    { 87, 6, "Molasses" },
                    { 88, 6, "Agave Syrup" },
                    { 89, 6, "Cane Sugar" },
                    { 90, 7, "Lasagna" },
                    { 91, 7, "Spaghetti" },
                    { 92, 7, "Tagliatelle" },
                    { 93, 7, "Fusilli" },
                    { 94, 7, "Gnocchi" },
                    { 95, 7, "Macaroni" },
                    { 96, 7, "Penne" },
                    { 97, 7, "Rigatoni" },
                    { 98, 8, "Beet(s)" },
                    { 99, 8, "Broccoli" },
                    { 100, 8, "Brussels Sprout" },
                    { 101, 8, "Cabbage" },
                    { 102, 8, "Celery" },
                    { 103, 8, "Kale" },
                    { 104, 8, "Lettuce" },
                    { 105, 8, "Spinach" },
                    { 106, 8, "Asparagus" },
                    { 107, 8, "Cauliflower" },
                    { 108, 8, "Eggplant" },
                    { 109, 8, "Mushrooms" },
                    { 110, 8, "Nettles" },
                    { 111, 8, "Leek" },
                    { 112, 8, "Garlic" },
                    { 113, 8, "Onion" },
                    { 114, 8, "Carrot" },
                    { 115, 8, "Celeriac" },
                    { 116, 8, "Ginger Root" },
                    { 117, 8, "Radish(es)" },
                    { 118, 8, "Potato(s)" },
                    { 119, 8, "Sweet Potato(s)" },
                    { 120, 8, "Sweet Corn" },
                    { 121, 8, "Zucchini" },
                    { 122, 8, "Tomato(s)" },
                    { 123, 8, "Cherry Tomato(s)" },
                    { 124, 8, "Green onion(s)" },
                    { 125, 8, "Cucumber(s)" },
                    { 126, 8, "Baby spinach" },
                    { 127, 8, "Red Bell Pepper" },
                    { 128, 8, "Green Bell Pepper" },
                    { 129, 8, "Red Onion(s)" },
                    { 130, 8, "Arugula" },
                    { 131, 8, "Parsnip" },
                    { 132, 9, "Apple(s)" },
                    { 133, 9, "Banana(s)" },
                    { 134, 9, "Avocado" },
                    { 135, 9, "Strawberries" },
                    { 136, 9, "Pear(s)" },
                    { 137, 9, "Cherries" },
                    { 138, 9, "Pineapple" },
                    { 139, 9, "Kiwi" },
                    { 140, 9, "Orange(s)" },
                    { 141, 9, "Lemon(s)" },
                    { 142, 9, "Grapes" },
                    { 143, 9, "Peaches" },
                    { 144, 9, "Mango" },
                    { 145, 9, "Raspberries" },
                    { 146, 9, "Blueberries" },
                    { 147, 9, "Plum(s)" },
                    { 148, 9, "Grapefruit" },
                    { 149, 9, "Lime" },
                    { 150, 9, "Prune(s)" },
                    { 151, 9, "Sour Cherries" },
                    { 152, 9, "Melon" },
                    { 153, 9, "Watermelon" },
                    { 154, 10, "Sunflower Oil" },
                    { 155, 10, "Olive Oil" },
                    { 156, 10, "Butter" },
                    { 157, 10, "Cream" },
                    { 158, 10, "Coconut Oil" },
                    { 159, 10, "Avocado Oil" },
                    { 160, 10, "Ghee" },
                    { 161, 10, "Lard" },
                    { 162, 10, "Mascarpone" },
                    { 163, 10, "Sour Cream" },
                    { 164, 10, "Whipped Cream" },
                    { 165, 10, "Coconut Cream" },
                    { 167, 10, "Diary Free Cream" },
                    { 168, 10, "Sesame Oil" },
                    { 169, 11, "Trout" },
                    { 170, 11, "Salmon" },
                    { 171, 11, "Tuna" },
                    { 172, 11, "Sardines" },
                    { 173, 11, "Mackerel" },
                    { 174, 11, "Cod" },
                    { 175, 11, "Mussels" },
                    { 176, 11, "Skip Jack" },
                    { 177, 11, "Shark" },
                    { 178, 11, "Shark" },
                    { 179, 11, "Silver Catfish (Pangasius)" },
                    { 180, 11, "Octopus" },
                    { 181, 11, "Squids" },
                    { 182, 11, "Hake fish" },
                    { 183, 11, "Salmon Trout" },
                    { 184, 11, "Sprat" },
                    { 185, 11, "Sardines" },
                    { 186, 12, "Lemon Juice" },
                    { 187, 12, "Tomatoe Paste" },
                    { 188, 12, "Mustard" }
                });

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
                name: "IX_Ingredients_IngredientCategoryId",
                table: "Ingredients",
                column: "IngredientCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_RecipeCategoryId",
                table: "Recipes",
                column: "RecipeCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipesIngredients_IngredientId",
                table: "RecipesIngredients",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipesIngredients_MeasureId",
                table: "RecipesIngredients",
                column: "MeasureId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipesIngredients_SpecificationId",
                table: "RecipesIngredients",
                column: "SpecificationId");
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
                name: "RecipesIngredients");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "Measures");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropTable(
                name: "Specifications");

            migrationBuilder.DropTable(
                name: "IngredientCategories");

            migrationBuilder.DropTable(
                name: "RecipeCategories");
        }
    }
}
