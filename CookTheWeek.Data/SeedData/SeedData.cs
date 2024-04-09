﻿namespace CookTheWeek.Data.SeedData
{
    using Microsoft.AspNetCore.Identity;

    using CookTheWeek.Data.Models;

    using static Common.GeneralApplicationConstants;

    internal class SeedData
    {
        public SeedData()
        {
            SeedUsers();
            SeedRecipeCategories();
            SeedIngredients();
            SeedMeasures();
            SeedSpecifications();
            SeedRecipes();
            SeedRecipeIngredients();
        }

        internal ICollection<ApplicationUser> SeedUsers()
        {
            ICollection<ApplicationUser> users = new HashSet<ApplicationUser>();

            PasswordHasher<ApplicationUser> hasher = new PasswordHasher<ApplicationUser>();

            ApplicationUser AppUser = new ApplicationUser()
            {
                Id = Guid.Parse(AppUserId),
                UserName = AppUserUsername,
                NormalizedUserName = AppUserUsername.ToUpper(),
                Email = AppUserEmail,
                NormalizedEmail = AppUserEmail.ToUpper(),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };

            AppUser.PasswordHash = hasher.HashPassword(AppUser, AppUserPassword);

            ApplicationUser AdminUser = new ApplicationUser()
            {
                Id = Guid.Parse(AdminUserId),
                UserName = AdminUserUsername,
                NormalizedUserName = AdminUserUsername.ToUpper(),
                Email = AdminUserEmail,
                NormalizedEmail = AdminUserEmail.ToUpper(),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };

            AdminUser.PasswordHash = hasher.HashPassword(AdminUser, AdminUserPassword);

            users.Add(AdminUser);
            users.Add(AppUser);

            return users;
        }
        internal ICollection<RecipeCategory> SeedRecipeCategories()
        {
            return new HashSet<RecipeCategory>()
            {
                new RecipeCategory()
                {
                   Id = 1,
                   Name = "Breakfast"
                },
                new RecipeCategory()
                {
                   Id = 2,
                   Name = "Soup"
                },
                new RecipeCategory()
                {
                   Id = 3,
                   Name = "Salad"
                },
                new RecipeCategory()
                {
                   Id = 4,
                   Name = "Main Dish"
                },
                new RecipeCategory()
                {
                   Id = 5,
                   Name = "Appetizer"
                },
                new RecipeCategory()
                {
                   Id = 6,
                   Name = "Dessert"
                },
            };
        }
        internal ICollection<IngredientCategory> SeedIngredientCategories()
        {
            return new HashSet<IngredientCategory>()
            {
                new IngredientCategory()
                {
                    Id = 1,
                    Name = "Eggs, Milk and Diary products"
                },
                new IngredientCategory()
                {
                    Id = 2,
                    Name = "Meat, Ground Meat and Sausage"
                },
                new IngredientCategory()
                {
                    Id = 3,
                    Name = "Beans, Lentils and Legumes"
                },
                new IngredientCategory()
                {
                    Id = 4,
                    Name = "Flour, Bread and Baking Products"
                },
                new IngredientCategory()
                {
                    Id = 5,
                    Name = "Herbs and Spices"
                },
                new IngredientCategory()
                {
                    Id = 6,
                    Name = "Sweeteners"
                },
                new IngredientCategory()
                {
                    Id = 7,
                    Name = "Pasta and Grains"
                },
                new IngredientCategory()
                {
                    Id = 8,
                    Name = "Vegetables"
                },
                new IngredientCategory()
                {
                    Id = 9,
                    Name = "Fruits"
                },
                new IngredientCategory()
                {
                    Id = 10,
                    Name = "Fats and Oils"
                },
                new IngredientCategory()
                {
                    Id = 11,
                    Name = "Fish and Seafood"
                },
                 new IngredientCategory()
                {
                    Id = 12,
                    Name = "Others"
                },
                 new IngredientCategory()
                {
                    Id = 13,
                    Name = "Nuts and seeds"
                },

            };
        }
        internal ICollection<Measure> SeedMeasures()
        {
            return new HashSet<Measure>()
            {
                new Measure()
                {
                   Id = 1,
                   Name = "piece(s)"
                },
                new Measure()
                { 
                   Id = 2,
                   Name = "clove(s)"
                },
                new Measure()
                { 
                   Id = 3,
                   Name = "ml"
                },
                new Measure()
                { 
                   Id = 4,
                   Name = "l"
                },
                new Measure()
                { 
                   Id = 5,
                   Name = "g"
                },
                new Measure()
                {
                   Id = 6,
                   Name = "kg"
                },
                new Measure()
                {
                   Id = 7,
                   Name = "tsp"
                },
                new Measure()
                {
                   Id = 8,
                   Name = "tbsp"
                },
                new Measure()
                {
                   Id = 9,
                   Name = "cup(s)"
                },
                new Measure()
                {
                   Id = 10,
                   Name = "bunch(es)"
                },
                new Measure()
                {
                   Id = 11,
                   Name = "pkg(s)"
                },
                new Measure()
                {
                   Id = 12,
                   Name = "slice(s)"
                },
                new Measure()
                {
                   Id = 13,
                   Name = "pinch(es)"
                },
            };
        }
        internal ICollection<Specification> SeedSpecifications()
        {
            return new HashSet<Specification>()
            {
                new Specification
                {
                    Id = 1,
                    Description = "frozen"
                },
                new Specification
                {
                    Id = 2,
                    Description = "canned"
                },
                new Specification
                {
                    Id = 3,
                    Description = "sliced"
                },
                new Specification
                {
                    Id = 4,
                    Description = "pre-cooked"
                },
                new Specification
                {
                    Id = 5,
                    Description = "grated"
                },
                new Specification
                {
                    Id = 6,
                    Description = "fermented"
                },
                new Specification
                {
                    Id = 7,
                    Description = "blended"
                },
                new Specification
                {
                    Id = 8,
                    Description = "finely-chopped"
                },
                new Specification
                {
                    Id = 9,
                    Description = "fresh"
                },
                new Specification
                {
                    Id = 10,
                    Description = "squeezed"
                }
            };
        }
        internal ICollection<Ingredient> SeedIngredients()
        {
            return new HashSet<Ingredient>()
            {
                new Ingredient()
                {
                    Id = 1,
                    Name = "Beef",
                    IngredientCategoryId = 2
                },
                new Ingredient()
                {
                    Id = 2,
                    Name = "Pork",
                    IngredientCategoryId = 2
                },
                new Ingredient()
                {
                    Id = 3,
                    Name = "Lamb",
                    IngredientCategoryId = 2
                },
                new Ingredient()
                {
                    Id = 4,
                    Name = "Chicken",
                    IngredientCategoryId = 2
                },
                new Ingredient()
                {
                    Id = 5,
                    Name = "Goat",
                    IngredientCategoryId = 2
                },
                new Ingredient()
                {
                    Id = 6,
                    Name = "Turkey",
                    IngredientCategoryId = 2
                },
                new Ingredient()
                {
                    Id = 7,
                    Name = "Rabbit",
                    IngredientCategoryId = 2
                },
                new Ingredient()
                {
                    Id = 8,
                    Name = "Venison",
                    IngredientCategoryId = 2
                },
                new Ingredient()
                {
                    Id = 9,
                    Name = "Duck",
                    IngredientCategoryId = 2
                },
                new Ingredient()
                {
                    Id = 10,
                    Name = "Wild Game Meat",
                    IngredientCategoryId = 2
                },
                new Ingredient()
                {
                    Id = 11,
                    Name = "Ground Meat",
                    IngredientCategoryId = 2
                },
                new Ingredient()
                {
                    Id = 31,
                    Name = "Sausage(s)",
                    IngredientCategoryId = 2
                },
                new Ingredient()
                {
                    Id = 32,
                    Name = "Meatboll(s)",
                    IngredientCategoryId = 2
                },
                new Ingredient()
                {
                    Id = 12,
                    Name = "Egg(s)",
                    IngredientCategoryId = 1
                },
                new Ingredient()
                {
                    Id = 13,
                    Name = "Milk",
                    IngredientCategoryId = 1
                },
                new Ingredient()
                {
                    Id = 14,
                    Name = "Yoghurt",
                    IngredientCategoryId = 1
                },
                new Ingredient()
                {
                    Id = 15,
                    Name = "Cheese",
                    IngredientCategoryId = 1
                },
                new Ingredient()
                {
                    Id = 16,
                    Name = "Yellow cheese",
                    IngredientCategoryId = 1
                },
                new Ingredient()
                {
                    Id = 17,
                    Name = "Cheddar",
                    IngredientCategoryId = 1
                },
                new Ingredient()
                {
                    Id = 18,
                    Name = "Brie",
                    IngredientCategoryId = 1
                },
                new Ingredient()
                {
                    Id = 19,
                    Name = "Feta",
                    IngredientCategoryId = 1
                },
                new Ingredient()
                {
                    Id = 20,
                    Name = "Camembert",
                    IngredientCategoryId = 1
                },
                new Ingredient()
                {
                    Id = 21,
                    Name = "Gouda",
                    IngredientCategoryId = 1
                },
                new Ingredient()
                {
                    Id = 22,
                    Name = "Goat Cheese",
                    IngredientCategoryId = 1
                },
                new Ingredient()
                {
                    Id = 23,
                    Name = "Emmental",
                    IngredientCategoryId = 1
                },
                new Ingredient()
                {
                    Id = 24,
                    Name = "Parmesan",
                    IngredientCategoryId = 1
                },
                new Ingredient()
                {
                    Id = 25,
                    Name = "Ricotta",
                    IngredientCategoryId = 1
                },
                new Ingredient()
                {
                    Id = 26,
                    Name = "Gorgonzola",
                    IngredientCategoryId = 1
                },
                new Ingredient()
                {
                    Id = 27,
                    Name = "Cottage Cheese",
                    IngredientCategoryId = 1
                },
                new Ingredient()
                {
                    Id = 28,
                    Name = "Edam",
                    IngredientCategoryId = 1
                },
                new Ingredient()
                {
                    Id = 29,
                    Name = "Mozzarella",
                    IngredientCategoryId = 1
                },
                new Ingredient()
                {
                    Id = 30,
                    Name = "Mozzarella",
                    IngredientCategoryId = 1
                }, //31 and 32 are for meat
                new Ingredient()
                {
                    Id = 33,
                    Name = "Lentils",
                    IngredientCategoryId = 3
                },
                new Ingredient()
                {
                    Id = 34,
                    Name = "Chickpeas",
                    IngredientCategoryId = 3
                },
                new Ingredient()
                {
                    Id = 35,
                    Name = "Green Peas",
                    IngredientCategoryId = 3
                },
                new Ingredient()
                {
                    Id = 36,
                    Name = "White Beans",
                    IngredientCategoryId = 3
                },
                new Ingredient()
                {
                    Id = 37,
                    Name = "Soybeans",
                    IngredientCategoryId = 3
                },
                new Ingredient()
                {
                    Id = 38,
                    Name = "Mung Beans",
                    IngredientCategoryId = 3
                },
                new Ingredient()
                {
                    Id = 39,
                    Name = "Red Lentils",
                    IngredientCategoryId = 3
                },
                new Ingredient()
                {
                    Id = 40,
                    Name = "Black Beans",
                    IngredientCategoryId = 3
                },
                new Ingredient()
                {
                    Id = 41,
                    Name = "Edamame",
                    IngredientCategoryId = 3
                },
                new Ingredient()
                {
                    Id = 42,
                    Name = "Green Lentils",
                    IngredientCategoryId = 3
                },
                new Ingredient()
                {
                    Id = 43,
                    Name = "White Flour",
                    IngredientCategoryId = 4
                },
                new Ingredient()
                {
                    Id = 44,
                    Name = "Whole Wheat Flour",
                    IngredientCategoryId = 4
                },
                new Ingredient()
                {
                    Id = 45,
                    Name = "Rye Flour",
                    IngredientCategoryId = 4
                },
                new Ingredient()
                {
                    Id = 46,
                    Name = "Spelt Flour",
                    IngredientCategoryId = 4
                },
                new Ingredient()
                {
                    Id = 47,
                    Name = "Almond Flour",
                    IngredientCategoryId = 4
                },
                new Ingredient()
                {
                    Id = 48,
                    Name = "Pine Nuts",
                    IngredientCategoryId = 13
                },
                new Ingredient()
                {
                    Id = 49,
                    Name = "Spelled Flour",
                    IngredientCategoryId = 4
                },
                new Ingredient()
                {
                    Id = 50,
                    Name = "Corn Flour",
                    IngredientCategoryId = 4
                },
                new Ingredient()
                {
                    Id = 51,
                    Name = "Rice Flour",
                    IngredientCategoryId = 4
                },
                new Ingredient()
                {
                    Id = 52,
                    Name = "Salt",
                    IngredientCategoryId = 5
                },
                new Ingredient()
                {
                    Id = 53,
                    Name = "Black Pepper",
                    IngredientCategoryId = 5
                },
                new Ingredient()
                {
                    Id = 54,
                    Name = "Paprika (Red Pepper)", //червен пипер
                    IngredientCategoryId = 5
                },
                new Ingredient()
                {
                    Id = 55,
                    Name = "Turmeric",
                    IngredientCategoryId = 5
                },
                new Ingredient()
                {
                    Id = 56,
                    Name = "Thyme",
                    IngredientCategoryId = 5
                },
                new Ingredient()
                {
                    Id = 57,
                    Name = "Oregano",
                    IngredientCategoryId = 5
                },
                new Ingredient()
                {
                    Id = 58,
                    Name = "Rosemary",
                    IngredientCategoryId = 5
                },
                new Ingredient()
                {
                    Id = 59,
                    Name = "Mint", //джоджен
                    IngredientCategoryId = 5
                },
                new Ingredient()
                {
                    Id = 60,
                    Name = "Cardamon",
                    IngredientCategoryId = 5
                },
                new Ingredient()
                {
                    Id = 61,
                    Name = "Curry Powder",
                    IngredientCategoryId = 5
                },
                new Ingredient()
                {
                    Id = 62,
                    Name = "Chilli powder",
                    IngredientCategoryId = 5
                },
                new Ingredient()
                {
                    Id = 63,
                    Name = "Ginger",
                    IngredientCategoryId = 5
                },
                new Ingredient()
                {
                    Id = 64,
                    Name = "Parsley", //магданоз
                    IngredientCategoryId = 5
                },
                new Ingredient()
                {
                    Id = 65,
                    Name = "Bay leaf", //дафинов лист
                    IngredientCategoryId = 5
                },
                new Ingredient()
                {
                    Id = 66,
                    Name = "Allspice", //бахар
                    IngredientCategoryId = 5
                },
                new Ingredient()
                {
                    Id = 67,
                    Name = "Basil", //босилек
                    IngredientCategoryId = 5
                },
                new Ingredient()
                {
                    Id = 68,
                    Name = "Clove", //карамфил
                    IngredientCategoryId = 5
                },
                new Ingredient()
                {
                    Id = 69,
                    Name = "Cinnamon",
                    IngredientCategoryId = 5
                },
                new Ingredient()
                {
                    Id = 70,
                    Name = "Vanilla",
                    IngredientCategoryId = 5
                },
                new Ingredient()
                {
                    Id = 71,
                    Name = "Celery",
                    IngredientCategoryId = 5
                },
                new Ingredient()
                {
                    Id = 72,
                    Name = "Peppermint",
                    IngredientCategoryId = 5
                },
                new Ingredient()
                {
                    Id = 73,
                    Name = "Marjoram",
                    IngredientCategoryId = 5
                },
                new Ingredient()
                {
                    Id = 74,
                    Name = "Wild Garlic",
                    IngredientCategoryId = 5
                },
                new Ingredient()
                {
                    Id = 75,
                    Name = "Fennel (dill)",
                    IngredientCategoryId = 5
                },
                new Ingredient()
                {
                    Id = 76,
                    Name = "Coriander",
                    IngredientCategoryId = 5
                },
                new Ingredient()
                {
                    Id = 77,
                    Name = "Clary Sage",
                    IngredientCategoryId = 5
                },
                new Ingredient()
                {
                    Id = 78,
                    Name = "Cilantro",
                    IngredientCategoryId = 5
                },
                new Ingredient()
                {
                    Id = 79,
                    Name = "Chives", //див лук
                    IngredientCategoryId = 5
                },
                new Ingredient()
                {
                    Id = 80,
                    Name = "Sugar",
                    IngredientCategoryId = 6
                },
                new Ingredient()
                {
                    Id = 81,
                    Name = "Honey",
                    IngredientCategoryId = 6
                },
                new Ingredient()
                {
                    Id = 82,
                    Name = "Maple Syrup",
                    IngredientCategoryId = 6
                },
                new Ingredient()
                {
                    Id = 83,
                    Name = "Coconut Sugar",
                    IngredientCategoryId = 6
                },
                new Ingredient()
                {
                    Id = 84,
                    Name = "Coconut Sugar",
                    IngredientCategoryId = 6
                },
                new Ingredient()
                {
                    Id = 85,
                    Name = "Powdered Sugar",
                    IngredientCategoryId = 6
                },
                new Ingredient()
                {
                    Id = 86,
                    Name = "Brown Sugar",
                    IngredientCategoryId = 6
                },
                new Ingredient()
                {
                    Id = 87,
                    Name = "Molasses", //меласа
                    IngredientCategoryId = 6
                },
                new Ingredient()
                {
                    Id = 88,
                    Name = "Agave Syrup",
                    IngredientCategoryId = 6
                },
                new Ingredient()
                {
                    Id = 89,
                    Name = "Cane Sugar",
                    IngredientCategoryId = 6
                },
                new Ingredient()
                {
                    Id = 90,
                    Name = "Lasagna",
                    IngredientCategoryId = 7
                },
                new Ingredient()
                {
                    Id = 91,
                    Name = "Spaghetti",
                    IngredientCategoryId = 7
                },
                new Ingredient()
                {
                    Id = 92,
                    Name = "Tagliatelle",
                    IngredientCategoryId = 7
                },
                new Ingredient()
                {
                    Id = 93,
                    Name = "Fusilli",
                    IngredientCategoryId = 7
                },
                new Ingredient()
                {
                    Id = 94,
                    Name = "Gnocchi",
                    IngredientCategoryId = 7
                },
                new Ingredient()
                {
                    Id = 95,
                    Name = "Macaroni",
                    IngredientCategoryId = 7
                },
                new Ingredient()
                {
                    Id = 96,
                    Name = "Penne",
                    IngredientCategoryId = 7
                },
                new Ingredient()
                {
                    Id = 97,
                    Name = "Rigatoni",
                    IngredientCategoryId = 7
                },
                new Ingredient()
                {
                    Id = 98,
                    Name = "Beet(s)",
                    IngredientCategoryId = 8
                },
                new Ingredient()
                {
                    Id = 99,
                    Name = "Broccoli",
                    IngredientCategoryId = 8
                },
                new Ingredient()
                {
                    Id = 100,
                    Name = "Brussels Sprout",
                    IngredientCategoryId = 8
                },
                new Ingredient()
                {
                    Id = 101,
                    Name = "Cabbage",
                    IngredientCategoryId = 8
                },
                new Ingredient()
                {
                    Id = 102,
                    Name = "Celery",
                    IngredientCategoryId = 8
                },
                new Ingredient()
                {
                    Id = 103,
                    Name = "Kale",
                    IngredientCategoryId = 8
                },
                new Ingredient()
                {
                    Id = 104,
                    Name = "Lettuce",
                    IngredientCategoryId = 8
                },
                new Ingredient()
                {
                    Id = 105,
                    Name = "Spinach",
                    IngredientCategoryId = 8
                },
                new Ingredient()
                {
                    Id = 106,
                    Name = "Asparagus",
                    IngredientCategoryId = 8
                },
                new Ingredient()
                {
                    Id = 107,
                    Name = "Cauliflower",
                    IngredientCategoryId = 8
                },
                new Ingredient()
                {
                    Id = 108,
                    Name = "Eggplant",
                    IngredientCategoryId = 8
                },
                new Ingredient()
                {
                    Id = 109,
                    Name = "Mushrooms",
                    IngredientCategoryId = 8
                },
                new Ingredient()
                {
                    Id = 110,
                    Name = "Nettles",
                    IngredientCategoryId = 8
                },
                new Ingredient()
                {
                    Id = 111,
                    Name = "Leek", //праз лук
                    IngredientCategoryId = 8
                },
                new Ingredient()
                {
                    Id = 112,
                    Name = "Garlic",
                    IngredientCategoryId = 8
                },
                new Ingredient()
                {
                    Id = 113,
                    Name = "Onion",
                    IngredientCategoryId = 8
                },
                new Ingredient()
                {
                    Id = 114,
                    Name = "Carrot",
                    IngredientCategoryId = 8
                },
                new Ingredient()
                {
                    Id = 115,
                    Name = "Celeriac", //целина корен
                    IngredientCategoryId = 8
                },
                new Ingredient()
                {
                    Id = 116,
                    Name = "Ginger Root",
                    IngredientCategoryId = 8
                },
                new Ingredient()
                {
                    Id = 117,
                    Name = "Radish(es)",
                    IngredientCategoryId = 8
                },
                new Ingredient()
                {
                    Id = 118,
                    Name = "Potato(s)",
                    IngredientCategoryId = 8
                },
                new Ingredient()
                {
                    Id = 119,
                    Name = "Sweet Potato(s)",
                    IngredientCategoryId = 8
                },
                new Ingredient()
                {
                    Id = 120,
                    Name = "Sweet Corn",
                    IngredientCategoryId = 8
                },
                new Ingredient()
                {
                    Id = 121,
                    Name = "Zucchini",
                    IngredientCategoryId = 8
                },
                new Ingredient()
                {
                    Id = 122,
                    Name = "Tomatoe(s)",
                    IngredientCategoryId = 8
                },
                new Ingredient()
                {
                    Id = 123,
                    Name = "Cherry Tomatoe(s)",
                    IngredientCategoryId = 8
                },
                new Ingredient()
                {
                    Id = 124,
                    Name = "Green onion(s)",
                    IngredientCategoryId = 8
                },
                new Ingredient()
                {
                    Id = 125,
                    Name = "Cucumber(s)",
                    IngredientCategoryId = 8
                },
                new Ingredient()
                {
                    Id = 126,
                    Name = "Baby spinach",
                    IngredientCategoryId = 8
                },
                new Ingredient()
                {
                    Id = 127,
                    Name = "Red Bell Pepper",
                    IngredientCategoryId = 8
                },
                new Ingredient()
                {
                    Id = 128,
                    Name = "Green Bell Pepper",
                    IngredientCategoryId = 8
                },
                new Ingredient()
                {
                    Id = 129,
                    Name = "Red Onion(s)",
                    IngredientCategoryId = 8
                },
                new Ingredient()
                {
                    Id = 130,
                    Name = "Arugula", //Рукола
                    IngredientCategoryId = 8
                },
                new Ingredient()
                {
                    Id = 131,
                    Name = "Parsnip", //Пащърнак
                    IngredientCategoryId = 8
                },
                new Ingredient()
                {
                    Id = 132,
                    Name = "Apple(s)",
                    IngredientCategoryId = 9
                },
                new Ingredient()
                {
                    Id = 133,
                    Name = "Banana(s)",
                    IngredientCategoryId = 9
                },
                new Ingredient()
                {
                    Id = 134,
                    Name = "Avocado",
                    IngredientCategoryId = 9
                },
                new Ingredient()
                {
                    Id = 135,
                    Name = "Strawberries",
                    IngredientCategoryId = 9
                },
                new Ingredient()
                {
                    Id = 136,
                    Name = "Pear(s)",
                    IngredientCategoryId = 9
                },
                new Ingredient()
                {
                    Id = 137,
                    Name = "Cherries",
                    IngredientCategoryId = 9
                },
                new Ingredient()
                {
                    Id = 138,
                    Name = "Pineapple",
                    IngredientCategoryId = 9
                },
                new Ingredient()
                {
                    Id = 139,
                    Name = "Kiwi",
                    IngredientCategoryId = 9
                },
                new Ingredient()
                {
                    Id = 140,
                    Name = "Orange(s)",
                    IngredientCategoryId = 9
                },
                new Ingredient()
                {
                    Id = 141,
                    Name = "Lemon(s)",
                    IngredientCategoryId = 9
                },
                new Ingredient()
                {
                    Id = 142,
                    Name = "Grapes",
                    IngredientCategoryId = 9
                },
                new Ingredient()
                {
                    Id = 143,
                    Name = "Peaches",
                    IngredientCategoryId = 9
                },
                new Ingredient()
                {
                    Id = 144,
                    Name = "Mango",
                    IngredientCategoryId = 9
                },
                new Ingredient()
                {
                    Id = 145,
                    Name = "Raspberries",
                    IngredientCategoryId = 9
                },
                new Ingredient()
                {
                    Id = 146,
                    Name = "Blueberries",
                    IngredientCategoryId = 9
                },
                new Ingredient()
                {
                    Id = 147,
                    Name = "Plum(s)",
                    IngredientCategoryId = 9
                },
                new Ingredient()
                {
                    Id = 148,
                    Name = "Grapefruit",
                    IngredientCategoryId = 9
                },
                new Ingredient()
                {
                    Id = 149,
                    Name = "Lime",
                    IngredientCategoryId = 9
                },
                new Ingredient()
                {
                    Id = 150,
                    Name = "Prune(s)",
                    IngredientCategoryId = 9
                },
                new Ingredient()
                {
                    Id = 151,
                    Name = "Sour Cherries",
                    IngredientCategoryId = 9
                },
                new Ingredient()
                {
                    Id = 152,
                    Name = "Melon",
                    IngredientCategoryId = 9
                },
                new Ingredient()
                {
                    Id = 153,
                    Name = "Watermelon",
                    IngredientCategoryId = 9
                },
                new Ingredient()
                {
                    Id = 154,
                    Name = "Sunflower Oil",
                    IngredientCategoryId = 10
                },
                new Ingredient()
                {
                    Id = 155,
                    Name = "Olive Oil",
                    IngredientCategoryId = 10
                },
                new Ingredient()
                {
                    Id = 156,
                    Name = "Butter",
                    IngredientCategoryId = 10
                },
                new Ingredient()
                {
                    Id = 157,
                    Name = "Cream",
                    IngredientCategoryId = 10
                },
                new Ingredient()
                {
                    Id = 158,
                    Name = "Coconut Oil",
                    IngredientCategoryId = 10
                },
                new Ingredient()
                {
                    Id = 159,
                    Name = "Avocado Oil",
                    IngredientCategoryId = 10
                },
                new Ingredient()
                {
                    Id = 160,
                    Name = "Ghee",
                    IngredientCategoryId = 10
                },
                new Ingredient()
                {
                    Id = 161,
                    Name = "Lard", //свинска мас
                    IngredientCategoryId = 10
                },
                new Ingredient()
                {
                    Id = 162,
                    Name = "Mascarpone",
                    IngredientCategoryId = 10
                },
                new Ingredient()
                {
                    Id = 163,
                    Name = "Sour Cream",
                    IngredientCategoryId = 10
                },
                new Ingredient()
                {
                    Id = 164,
                    Name = "Whipped Cream",
                    IngredientCategoryId = 10
                },
                new Ingredient()
                {
                    Id = 165,
                    Name = "Coconut Cream",
                    IngredientCategoryId = 10
                },
                new Ingredient()
                {
                    Id = 166,
                    Name = "Diary Free Cream",
                    IngredientCategoryId = 10
                },
                new Ingredient()
                {
                    Id = 167,
                    Name = "Sesame Oil",
                    IngredientCategoryId = 10
                },
                // Риба и морски храни
                new Ingredient()
                {
                    Id = 168,
                    Name = "Trout",
                    IngredientCategoryId = 11
                },
                new Ingredient()
                {
                    Id = 169,
                    Name = "Salmon",
                    IngredientCategoryId = 11
                },
                new Ingredient()
                {
                    Id = 170,
                    Name = "Tuna",
                    IngredientCategoryId = 11
                },
                new Ingredient()
                {
                    Id = 171,
                    Name = "Sardines",
                    IngredientCategoryId = 11
                },
                new Ingredient()
                {
                    Id = 172,
                    Name = "Mackerel", //скумрия
                    IngredientCategoryId = 11
                },
                new Ingredient()
                {
                    Id = 173,
                    Name = "Cod", //треска
                    IngredientCategoryId = 11
                },
                new Ingredient()
                {
                    Id = 174,
                    Name = "Mussels", //миди
                    IngredientCategoryId = 11
                },
                new Ingredient()
                {
                    Id = 175,
                    Name = "Skip Jack", //лефер
                    IngredientCategoryId = 11
                },
                new Ingredient()
                {
                    Id = 176,
                    Name = "Shark",
                    IngredientCategoryId = 11
                },
                new Ingredient()
                {
                    Id = 177,
                    Name = "Shark",
                    IngredientCategoryId = 11
                },
                new Ingredient()
                {
                    Id = 178,
                    Name = "Silver Catfish (Pangasius)",
                    IngredientCategoryId = 11
                },
                new Ingredient()
                {
                    Id = 179,
                    Name = "Octopus",
                    IngredientCategoryId = 11
                },
                new Ingredient()
                {
                    Id = 180,
                    Name = "Squids",
                    IngredientCategoryId = 11
                },
                new Ingredient()
                {
                    Id = 181,
                    Name = "Hake fish", //хек
                    IngredientCategoryId = 11
                },
                new Ingredient()
                {
                    Id = 182,
                    Name = "Salmon Trout", //сьомгова пастърва
                    IngredientCategoryId = 11
                },
                new Ingredient()
                {
                    Id = 183,
                    Name = "Sprat", //цаца
                    IngredientCategoryId = 11
                },
                new Ingredient()
                {
                    Id = 184,
                    Name = "Sardines",
                    IngredientCategoryId = 11
                },
                // Others
                new Ingredient()
                {
                    Id = 185,
                    Name = "Lemon Juice",
                    IngredientCategoryId = 12
                },
                new Ingredient()
                {
                    Id = 186,
                    Name = "Tomatoe Paste",
                    IngredientCategoryId = 12
                },
                new Ingredient()
                {
                    Id = 187,
                    Name = "Mustard",
                    IngredientCategoryId = 12
                },
                new Ingredient()
                {
                    Id = 188,
                    Name = "Savory", //Чубрица
                    IngredientCategoryId = 5
                },
                new Ingredient()
                {
                    Id = 189,
                    Name = "Rice",
                    IngredientCategoryId = 7
                },
                new Ingredient()
                {
                    Id = 190,
                    Name = "Brown Rice",
                    IngredientCategoryId = 7
                },
                new Ingredient()
                {
                    Id = 191,
                    Name = "Black Rice",
                    IngredientCategoryId = 7
                },
                new Ingredient()
                {
                    Id = 192,
                    Name = "Barley",
                    IngredientCategoryId = 7
                },
                new Ingredient()
                {
                    Id = 193,
                    Name = "Bulgur",
                    IngredientCategoryId = 7
                },
                new Ingredient()
                {
                    Id = 194,
                    Name = "Quinoa",
                    IngredientCategoryId = 7
                },
                new Ingredient()
                {
                    Id = 195,
                    Name = "Amaranth",
                    IngredientCategoryId = 7
                },
                new Ingredient()
                {
                    Id = 196,
                    Name = "Oats",
                    IngredientCategoryId = 7
                },
                new Ingredient()
                {
                    Id = 197,
                    Name = "Oatmeal",
                    IngredientCategoryId = 7
                },
                new Ingredient()
                {
                    Id = 198,
                    Name = "Chia Seeds",
                    IngredientCategoryId = 13
                },
                new Ingredient()
                {
                    Id = 199,
                    Name = "Wheat",
                    IngredientCategoryId = 7
                },
                new Ingredient()
                {
                    Id = 200,
                    Name = "Corn",
                    IngredientCategoryId = 7
                },
                new Ingredient()
                {
                    Id = 201,
                    Name = "Spelt",
                    IngredientCategoryId = 7
                },
                new Ingredient()
                {
                    Id = 202,
                    Name = "Buckwheat", // Елда
                    IngredientCategoryId = 7
                },
                new Ingredient()
                {
                    Id = 203,
                    Name = "Millet", // Просо
                    IngredientCategoryId = 7
                },
                new Ingredient()
                {
                    Id = 204,
                    Name = "Farro", // зърната от 3 вида пшеница 
                    IngredientCategoryId = 7
                },
                // Nuts here:
                new Ingredient()
                {
                    Id = 205,
                    Name = "Almonds",
                    IngredientCategoryId = 13
                },
                new Ingredient()
                {
                    Id = 206,
                    Name = "Brazil Nuts",
                    IngredientCategoryId = 13
                },
                new Ingredient()
                {
                    Id = 207,
                    Name = "Cashew Nuts",
                    IngredientCategoryId = 13
                },
                new Ingredient()
                {
                    Id = 208,
                    Name = "Hazel Nuts",
                    IngredientCategoryId = 13
                },
                new Ingredient()
                {
                    Id = 209,
                    Name = "Macadamias",
                    IngredientCategoryId = 13
                },
                new Ingredient()
                {
                    Id = 210,
                    Name = "Pecans",
                    IngredientCategoryId = 13
                },
                new Ingredient()
                {
                    Id = 211,
                    Name = "Pistachios",
                    IngredientCategoryId = 13
                },
                new Ingredient()
                {
                    Id = 212,
                    Name = "Walnuts",
                    IngredientCategoryId = 13
                },
                new Ingredient()
                {
                    Id = 213,
                    Name = "Peanuts",
                    IngredientCategoryId = 13
                },
                new Ingredient()
                {
                    Id = 214,
                    Name = "Pumpkin Seeds",
                    IngredientCategoryId = 13
                },
                new Ingredient()
                {
                    Id = 215,
                    Name = "Flax Seeds",
                    IngredientCategoryId = 13
                },
                new Ingredient()
                {
                    Id = 216,
                    Name = "Sesame Seeds",
                    IngredientCategoryId = 13
                },
                new Ingredient()
                {
                    Id = 217,
                    Name = "Poppy Seeds",
                    IngredientCategoryId = 13
                },
                new Ingredient()
                {
                    Id = 218,
                    Name = "Sunflower Seeds",
                    IngredientCategoryId = 13
                },
                new Ingredient()
                {
                    Id = 219,
                    Name = "Psyllium Seeds",
                    IngredientCategoryId = 13
                },
                //new Ingredient()
                //{
                //    Id = 220,
                //    Name = "Rice",
                //    IngredientCategoryId = 7
                //},
                //new Ingredient()
                //{
                //    Id = 221,
                //    Name = "Brown Rice",
                //    IngredientCategoryId = 7
                //},
                new Ingredient()
                {
                    Id = 222,
                    Name = "Vegetable Broth",
                    IngredientCategoryId = 12
                },
                new Ingredient()
                {
                    Id = 223,
                    Name = "Beef Broth",
                    IngredientCategoryId = 12
                },
                new Ingredient()
                {
                    Id = 224,
                    Name = "Fish Broth",
                    IngredientCategoryId = 12
                },
                new Ingredient()
                {
                    Id = 225,
                    Name = "Fish Broth",
                    IngredientCategoryId = 12
                },
                new Ingredient()
                {
                    Id = 226,
                    Name = "Noodles",
                    IngredientCategoryId = 7
                },
                new Ingredient()
                {
                    Id = 227,
                    Name = "Date(s)",
                    IngredientCategoryId = 9
                },
                new Ingredient()
                {
                    Id = 228,
                    Name = "Granola",
                    IngredientCategoryId = 7
                },
                new Ingredient()
                {
                    Id = 229,
                    Name = "Fruits of choice",
                    IngredientCategoryId = 9
                },
                new Ingredient()
                {
                    Id = 230,
                    Name = "Nuts of choice",
                    IngredientCategoryId = 13
                },
                new Ingredient()
                {
                    Id = 231,
                    Name = "Seeds of choice",
                    IngredientCategoryId = 13
                },
                new Ingredient()
                {
                    Id = 232,
                    Name = "Diary-free milk",
                    IngredientCategoryId = 12
                },
                new Ingredient()
                {
                    Id = 233,
                    Name = "Almond milk",
                    IngredientCategoryId = 12
                },
                new Ingredient()
                {
                    Id = 234,
                    Name = "Oat milk",
                    IngredientCategoryId = 12
                },
                new Ingredient()
                {
                    Id = 235,
                    Name = "Soy milk",
                    IngredientCategoryId = 12
                },
                new Ingredient()
                {
                    Id = 236,
                    Name = "Hemp milk",
                    IngredientCategoryId = 12
                },
                new Ingredient()
                {
                    Id = 237,
                    Name = "Cashew milk",
                    IngredientCategoryId = 12
                },
                new Ingredient()
                {
                    Id = 238,
                    Name = "Hazelnut milk",
                    IngredientCategoryId = 12
                },
                new Ingredient()
                {
                    Id = 239,
                    Name = "Tofu",
                    IngredientCategoryId = 12
                },
                new Ingredient()
                {
                    Id = 240,
                    Name = "Bread",
                    IngredientCategoryId = 4
                },
                new Ingredient()
                {
                    Id = 241,
                    Name = "Bread",
                    IngredientCategoryId = 4
                },
                new Ingredient()
                {
                    Id = 242,
                    Name = "Sourdough Bread",
                    IngredientCategoryId = 4
                },
                new Ingredient()
                {
                    Id = 243,
                    Name = "Full Grain Bread",
                    IngredientCategoryId = 4
                },
                new Ingredient()
                {
                    Id = 244,
                    Name = "White Bread",
                    IngredientCategoryId = 4
                },
                new Ingredient()
                {
                    Id = 245,
                    Name = "Rye Bread",
                    IngredientCategoryId = 4
                },
                new Ingredient()
                {
                    Id = 246,
                    Name = "Flatbread",
                    IngredientCategoryId = 4
                },
                new Ingredient()
                {
                    Id = 247,
                    Name = "Brioche",
                    IngredientCategoryId = 4
                },
                new Ingredient()
                {
                    Id = 248,
                    Name = "Baguette",
                    IngredientCategoryId = 4
                },
                new Ingredient()
                {
                    Id = 249,
                    Name = "Ciabatta",
                    IngredientCategoryId = 4
                },
                new Ingredient()
                {
                    Id = 250,
                    Name = "Bread of choice",
                    IngredientCategoryId = 4
                },
                new Ingredient()
                {
                    Id = 251,
                    Name = "Hemp Seeds",
                    IngredientCategoryId = 13
                },
                new Ingredient()
                {
                    Id = 252,
                    Name = "Himalayan Salt",
                    IngredientCategoryId = 5
                },
            };
        }
        internal ICollection<Recipe> SeedRecipes()
        {
            return new HashSet<Recipe>()
            {
                new Recipe()
                {
                    Id = Guid.Parse("11112341-30e4-473f-b93a-d0352b978a84"),
                    OwnerId = AdminUserId,
                    Title = "Moussaka",
                    Description = "Moussaka is beloved Balkan and Middle East dish. Its preparation depends on the region. In Bulgaria Moussaka is based on potatoes and ground meat. The meal is served warm and Bulgarians eat it very often simply because it’s super delicious and easy to cook. ",
                    Instructions = "Start with cooking the onion in a pan with 1/4 oil until golden brown. Then add the ground meat, the pepper, the paprika, and half the salt. Add the tomatoes and fry until they evaporate and the meat gets brown. Then remove the pan from the heat. Mix well with the potatoes and the other 1/2 tablespoon of salt. Add the mixture in a casserole pan with the rest of the oil. Bake in oven for about 40 minutes on 425 F (~220 C). In the meantime mix the yoghurt and the eggs separately and pour on top  of the meal for the last 10  minutes in the oven untill it turns brownish.",
                    Servings = 8,
                    TotalTime = TimeSpan.FromMinutes(120.0),
                    ImageUrl = "https://www.supichka.com/files/images/1242/fit_1400_933.jpg",
                    RecipeCategoryId = 4,
                },
                new Recipe()
                {
                    Id = Guid.Parse("4a37318d-86fc-4411-a686-b01ae7e007c8"),
                    OwnerId = AdminUserId,
                    Title = "Beef Stew",
                    Description = "Savor the essence of a classic beef stew: tender beef, seared to perfection, nestled among hearty potatoes, sweet carrots, and crisp celery in a rich broth. Fragrant herbs and spices dance in each spoonful, invoking warmth and tradition. It's a comforting embrace on chilly nights, a symphony of flavors that transports you to cozy kitchens and cherished gatherings. With its melt-in-your-mouth beef and earthy vegetables, this stew is more than a meal—it's a timeless delight, a celebration of culinary craftsmanship and the simple joys of good food shared with loved ones.",
                    Instructions = "Add the onion, black pepper (beans), parsley, sunflower oil, salt and the beef to a pressure cooker. Fill with clean water to a level of 2 fingers above the products. Cook under pressure for about 40 minutes. Open the pressure cooker and strain the broth from the onion and black pepper beans. Portion the meat and remove the meat zip. Take back to a boil the portioned meat, the bone broth and add the largely cut into pieces carrots, celery root and potatoes. Bring the pressure cooker to a boil again and cook for another 20 minutes.\r\n",
                    Servings = 4,
                    TotalTime = TimeSpan.FromMinutes(90.0),
                    ImageUrl = "https://www.simplyrecipes.com/thmb/W8uC2OmR-C8WvHiURqfomkvnUnw=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/__opt__aboutcom__coeus__resources__content_migration__simply_recipes__uploads__2015__03__irish-beef-stew-vertical-a2-1800-8012236ba7e34c37abc3baedcab4aff7.jpg",
                    RecipeCategoryId = 4
                },
                new Recipe()
                {
                    Id = Guid.Parse("25c6718c-b53b-4092-9454-d6999355f12d"),
                    OwnerId = AdminUserId,
                    Title = "Homemade Chicken Soup",
                    Description = "Classical easy and delicious chicken soup to keep you warm in the cold winter days.",
                    Instructions = "Boil 2l of water. Add the chicken meat and some salt. Boil until ready or at leas for half an hour. Remove the chicken and portion it into small pieces. Take the remaining chicken broth back and again bring to a boil. Cut the vegetables into small pieces. First add the carrots and the onions to the boiling broth. After 5 minutes add the cut into small pieces potatoes. 5 minutes later also add the noodles. Finally add the portioned chicken to the soup. After boiling for another 5 minutes, add some finely cut celery. ",
                    Servings = 6,
                    TotalTime = TimeSpan.FromMinutes(60.0),
                    ImageUrl = "https://i2.wp.com/www.downshiftology.com/wp-content/uploads/2023/10/Chicken-Soup-6.jpg",
                    RecipeCategoryId = 2
                },
                new Recipe()
                {
                    Id = Guid.Parse("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"),
                    OwnerId = AdminUserId,
                    Title = "Stuffed red peppers with ground meat and rice",
                    Description = "This versatile meal is not only simple to make, but feeds families big and small, making it a cheap and easy weeknight dinner legend.",
                    Instructions = "1. Finely-chop the onion and carrots. Add to a pre-heated 3-4 tbsp of sunflower oil. Bake for a few minutes. 2. Add the minced meat while constantly mixing 3. Add the tomatoes and leave for the liquid to evaporate. Finally add the rice and the red pepper. Bake for another minute and remove from the stove 4. Add spices according to your taste - at least salt and black pepper (may add also allspice, cumin, etc.) 5. Stuff the peppers and put them in the oven with a little bit of salty water. Bake for 45mins on 180 degrees.",
                    Servings = 8,
                    TotalTime = TimeSpan.FromMinutes(120.0),
                    ImageUrl = "https://hips.hearstapps.com/hmg-prod/images/stuffed-peppers-lead-649c91e2c4e39.jpg",
                    RecipeCategoryId = 2
                },
                new Recipe()
                {
                    Id = Guid.Parse("115e248e-3165-425d-aec6-5dda97c99be4"),
                    OwnerId = AdminUserId,
                    Title = "Fruity Strawberry Smoothy",
                    Description = "Indulge in a refreshing blend of creamy yogurt, ripe dates, nutrient-rich chia seeds, and succulent strawberries, creating a tantalizing fruity smoothie bursting with flavor and wholesome goodness. Perfect for a quick breakfast boost or a revitalizing snack any time of the day!",
                    Instructions = "Begin by soaking the chia seeds in water for about 10-15 minutes to allow them to gel up and soften.\r\nOnce the chia seeds have absorbed the water, place them along with the yogurt, pitted dates, and fresh strawberries into a blender.\r\nBlend all the ingredients on high speed until smooth and creamy, ensuring there are no chunks remaining.\r\nPour the smoothie into glasses and serve immediately for a delightful and nutritious treat. Enjoy your refreshing fruity smoothie!",
                    Servings = 1,
                    TotalTime = TimeSpan.FromMinutes(10.0),
                    ImageUrl = "https://www.proteincakery.com/wp-content/uploads/2023/11/strawberry-chia-collagen-smoothie.jpg",
                    RecipeCategoryId = 1
                },
                new Recipe()
                {
                    Id = Guid.Parse("cd9be7fb-c016-4246-ac36-411f6c3ece14"),
                    OwnerId = AdminUserId,
                    Title = "Overnight Oats (prepare the night beofre)",
                    Description = "Wake up to a simple breakfast solution with our delightful Overnight Oats. A harmonious blend of hearty oats, nutritious chia seeds, ripe banana, creamy milk (whether dairy or dairy-free), crunchy granola, and an assortment of vibrant fruits, all lovingly combined and left to mingle overnight for a deliciously convenient morning meal. Start your day right with this wholesome and customizable dish that promises to energize and satisfy with every spoonful.",
                    Instructions = "Mix the banana and the milk of your choice in a high-speed blender and blend until smooth. Divide the rest of ingridients (half a cup rolled-oats, 1tbsp chia seeds and 2tsp sunflower seeds) and place in 2 bowls. Mix well and pour half of the blended milk with banana on top of each bowl. Store in a fridge during the night. The morning after top with granolla and fruits of your choice.",
                    Servings = 2,
                    TotalTime = TimeSpan.FromMinutes(10.0),
                    ImageUrl = "https://i0.wp.com/adiligentheart.com/wp-content/uploads/2023/01/image-31.png?w=1000&ssl=1",
                    RecipeCategoryId = 1
                },
                new Recipe()
                {
                    Id = Guid.Parse("16541e8d-716c-45d9-8d6d-e3ae70d46c7b"),
                    OwnerId = AdminUserId,
                    Title = "Avocado Toast",
                    Description = "Elevate your morning routine with this tasty Avocado Toast! Perfect start of the day for those busy mronings..",
                    Instructions = "For a delicious twist, grill your slice of bread to your preference. Then, simply smash the avocado with a fork and spread it generously over the bread. Top it off with sliced cherry tomatoes, sprinkle with Himalayan salt and hemp seeds, and finally squeeze a little bit of lemon juice on top. Now, savor the flavors and enjoy your delightful avocado toast!",
                    Servings = 1,
                    TotalTime = TimeSpan.FromMinutes(10.0),
                    ImageUrl = "https://cookingupmemories.com/wp-content/uploads/2021/01/avocado-toast-with-balsalmic-glaze-breakfast-768x1152.jpg.webp",
                    RecipeCategoryId = 1
                },
            };
        }

        internal ICollection<RecipeIngredient> SeedRecipeIngredients()
        {
            return new HashSet<RecipeIngredient>()
            {
                //First Recipe Ingredients =>
                    new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("11112341-30e4-473f-b93a-d0352b978a84"),
                    IngredientId = 11,
                    Qty = 500,
                    MeasureId = 5
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("11112341-30e4-473f-b93a-d0352b978a84"),
                    IngredientId = 113,
                    Qty = 2,
                    MeasureId = 1
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("11112341-30e4-473f-b93a-d0352b978a84"),
                    IngredientId = 118,
                    Qty = 1,
                    MeasureId = 6
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("11112341-30e4-473f-b93a-d0352b978a84"),
                    IngredientId = 122,
                    Qty = 250,
                    MeasureId = 5
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("11112341-30e4-473f-b93a-d0352b978a84"),
                    IngredientId = 154,
                    Qty = 3,
                    MeasureId = 8
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("11112341-30e4-473f-b93a-d0352b978a84"),
                    IngredientId = 54,
                    Qty = 1,
                    MeasureId = 8
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("11112341-30e4-473f-b93a-d0352b978a84"),
                    IngredientId = 14,
                    Qty = 1,
                    MeasureId = 9
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("11112341-30e4-473f-b93a-d0352b978a84"),
                    IngredientId = 12,
                    Qty = 2,
                    MeasureId = 1
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("11112341-30e4-473f-b93a-d0352b978a84"),
                    IngredientId = 52,
                    Qty = 1,
                    MeasureId = 7
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("11112341-30e4-473f-b93a-d0352b978a84"),
                    IngredientId = 53,
                    Qty = 1,
                    MeasureId = 7
                }, //second recipe =>                        
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("4a37318d-86fc-4411-a686-b01ae7e007c8"),
                    IngredientId = 1,
                    Qty = 400,
                    MeasureId = 5
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("4a37318d-86fc-4411-a686-b01ae7e007c8"),
                    IngredientId = 118,
                    Qty = 6,
                    MeasureId = 1
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("4a37318d-86fc-4411-a686-b01ae7e007c8"),
                    IngredientId = 71,
                    Qty = 1,
                    MeasureId = 1
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("4a37318d-86fc-4411-a686-b01ae7e007c8"),
                    IngredientId = 113,
                    Qty = 1,
                    MeasureId = 1
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("4a37318d-86fc-4411-a686-b01ae7e007c8"),
                    IngredientId = 114,
                    Qty = 3,
                    MeasureId = 1
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("4a37318d-86fc-4411-a686-b01ae7e007c8"),
                    IngredientId = 154,
                    Qty = 3,
                    MeasureId = 8
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("4a37318d-86fc-4411-a686-b01ae7e007c8"),
                    IngredientId = 64,
                    Qty = 1,
                    MeasureId = 10
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("4a37318d-86fc-4411-a686-b01ae7e007c8"),
                    IngredientId = 52,
                    Qty = 1,
                    MeasureId = 7
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("4a37318d-86fc-4411-a686-b01ae7e007c8"),
                    IngredientId = 53,
                    Qty = 10,
                    MeasureId = 1
                }, // Third Recipe =>
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("25c6718c-b53b-4092-9454-d6999355f12d"),
                    IngredientId = 4,
                    Qty = 600,
                    MeasureId = 5
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("25c6718c-b53b-4092-9454-d6999355f12d"),
                    IngredientId = 114,
                    Qty = 3,
                    MeasureId = 1
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("25c6718c-b53b-4092-9454-d6999355f12d"),
                    IngredientId = 113,
                    Qty = 1,
                    MeasureId = 1
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("25c6718c-b53b-4092-9454-d6999355f12d"),
                    IngredientId = 188,
                    Qty = 1,
                    MeasureId = 7
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("25c6718c-b53b-4092-9454-d6999355f12d"),
                    IngredientId = 53,
                    Qty = 1,
                    MeasureId = 7
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("25c6718c-b53b-4092-9454-d6999355f12d"),
                    IngredientId = 57,
                    Qty = 1,
                    MeasureId = 7
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("25c6718c-b53b-4092-9454-d6999355f12d"),
                    IngredientId = 64,
                    Qty = 1,
                    MeasureId = 10
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("25c6718c-b53b-4092-9454-d6999355f12d"),
                    IngredientId = 52,
                    Qty = 1,
                    MeasureId = 7
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("25c6718c-b53b-4092-9454-d6999355f12d"),
                    IngredientId = 56,
                    Qty = 1,
                    MeasureId = 7
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("25c6718c-b53b-4092-9454-d6999355f12d"),
                    IngredientId = 156,
                    Qty = 150,
                    MeasureId = 5
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("25c6718c-b53b-4092-9454-d6999355f12d"),
                    IngredientId = 163,
                    Qty = 250,
                    MeasureId = 5
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("25c6718c-b53b-4092-9454-d6999355f12d"),
                    IngredientId = 154,
                    Qty = 1,
                    MeasureId = 8
                },// Fourth Recipe => 
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"),
                    IngredientId = 2,
                    Qty = 500,
                    MeasureId = 5
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"),
                    IngredientId = 127,
                    Qty = 10,
                    MeasureId = 1
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"),
                    IngredientId = 114,
                    Qty = 1,
                    MeasureId = 1
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"),
                    IngredientId = 122,
                    Qty = 1,
                    MeasureId = 1,
                    SpecificationId = 5
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"),
                    IngredientId = 154,
                    Qty = 4,
                    MeasureId = 8
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"),
                    IngredientId = 189,
                    Qty = 160,
                    MeasureId = 3
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"),
                    IngredientId = 113,
                    Qty = 1,
                    MeasureId = 1
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"),
                    IngredientId = 52,
                    Qty = 1,
                    MeasureId = 7
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"),
                    IngredientId = 53,
                    Qty = 1,
                    MeasureId = 7
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"),
                    IngredientId = 54,
                    Qty = 1,
                    MeasureId = 8
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"),
                    IngredientId = 188,
                    Qty = 1,
                    MeasureId = 7
                }, // Fifth Recipe =>
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("115e248e-3165-425d-aec6-5dda97c99be4"),
                    IngredientId = 14,
                    Qty = 1,
                    MeasureId = 9
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("115e248e-3165-425d-aec6-5dda97c99be4"),
                    IngredientId = 135,
                    Qty = 1,
                    MeasureId = 9
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("115e248e-3165-425d-aec6-5dda97c99be4"),
                    IngredientId = 198,
                    Qty = 0.5m,
                    MeasureId = 9
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("115e248e-3165-425d-aec6-5dda97c99be4"),
                    IngredientId = 116,
                    Qty = 1,
                    MeasureId = 12,
                    SpecificationId = 9
                }, // Sixth Recipe =>
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("cd9be7fb-c016-4246-ac36-411f6c3ece14"),
                    IngredientId = 196,
                    Qty = 0.5m,
                    MeasureId = 9
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("cd9be7fb-c016-4246-ac36-411f6c3ece14"),
                    IngredientId = 198,
                    Qty = 1,
                    MeasureId = 8
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("cd9be7fb-c016-4246-ac36-411f6c3ece14"),
                    IngredientId = 218,
                    Qty = 2,
                    MeasureId = 7
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("cd9be7fb-c016-4246-ac36-411f6c3ece14"),
                    IngredientId = 13,
                    Qty = 1,
                    MeasureId = 9
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("cd9be7fb-c016-4246-ac36-411f6c3ece14"),
                    IngredientId = 228,
                    Qty = 2,
                    MeasureId = 8
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("cd9be7fb-c016-4246-ac36-411f6c3ece14"),
                    IngredientId = 229,
                    Qty = 2,
                    MeasureId = 8,
                    SpecificationId = 8
                }, // Seventh Recipe
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("16541e8d-716c-45d9-8d6d-e3ae70d46c7b"),
                    IngredientId = 134,
                    Qty = 0.5m,
                    MeasureId = 1
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("16541e8d-716c-45d9-8d6d-e3ae70d46c7b"),
                    IngredientId = 243,
                    Qty = 1,
                    MeasureId = 12
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("16541e8d-716c-45d9-8d6d-e3ae70d46c7b"),
                    IngredientId = 251,
                    Qty = 1,
                    MeasureId = 7
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("16541e8d-716c-45d9-8d6d-e3ae70d46c7b"),
                    IngredientId = 252,
                    Qty = 1,
                    MeasureId = 13
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("16541e8d-716c-45d9-8d6d-e3ae70d46c7b"),
                    IngredientId = 141,
                    Qty = 1,
                    MeasureId = 1,
                    SpecificationId = 10
                },
                new RecipeIngredient()
                {
                    RecipeId = Guid.Parse("16541e8d-716c-45d9-8d6d-e3ae70d46c7b"),
                    IngredientId = 123,
                    Qty = 6,
                    MeasureId = 1
                } // END
            };
        }

    }
}
