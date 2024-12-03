namespace CookTheWeek.Data.SeedData
{
    using System.Globalization;

    using Microsoft.AspNetCore.Identity;

    using CookTheWeek.Data.Models;

    using static Common.GeneralApplicationConstants;

    internal static class SeedData
    {
        internal static ICollection<ApplicationUser> SeedUsers()
        {
            ICollection<ApplicationUser> users = new HashSet<ApplicationUser>();

            PasswordHasher<ApplicationUser> hasher = new();

            ApplicationUser AppUser = new()
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

            ApplicationUser AdminUser = new()
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

            ApplicationUser DeletedUser = new()
            {
                Id = Guid.Parse(DeletedUserId),
                UserName = DeletedUserUsername,
                NormalizedUserName = DeletedUserUsername.ToUpper(),
                Email = DeletedUserEmail,
                NormalizedEmail = DeletedUserEmail.ToUpper(),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };

            DeletedUser.PasswordHash = hasher.HashPassword(DeletedUser, DeletedUserPassword);

            users.Add(AdminUser);
            users.Add(AppUser);
            users.Add(DeletedUser);

            return users;
        }
        internal static ICollection<RecipeCategory> SeedRecipeCategories()
        {
            return new HashSet<RecipeCategory>()
            {
                new()
                {
                   Id = 1,
                   Name = "Breakfast"
                },
                new()
                {
                   Id = 2,
                   Name = "Soup"
                },
                new()
                {
                   Id = 3,
                   Name = "Salad"
                },
                new()
                {
                   Id = 4,
                   Name = "Main Dish"
                },
                new()
                {
                   Id = 5,
                   Name = "Appetizer"
                },
                new()
                {
                   Id = 6,
                   Name = "Dessert"
                },
            };
        }
        internal static ICollection<IngredientCategory> SeedIngredientCategories()
        {
            return new HashSet<IngredientCategory>()
            {
                new()
                {
                    Id = 1,
                    Name = "Eggs, Milk and Diary products"
                },
                new()
                {
                    Id = 2,
                    Name = "Meat, Ground Meat and Sausage"
                },
                new()
                {
                    Id = 3,
                    Name = "Beans, Lentils and Legumes"
                },
                new()
                {
                    Id = 4,
                    Name = "Flour, Bread and Baking Products"
                },
                new()
                {
                    Id = 5,
                    Name = "Herbs and Spices"
                },
                new()
                {
                    Id = 6,
                    Name = "Sweeteners"
                },
                new()
                {
                    Id = 7,
                    Name = "Pasta and Grains"
                },
                new()
                {
                    Id = 8,
                    Name = "Vegetables"
                },
                new()
                {
                    Id = 9,
                    Name = "Fruits"
                },
                new()
                {
                    Id = 10,
                    Name = "Fats and Oils, Sauces and Broths"
                },
                new()
                {
                    Id = 11,
                    Name = "Fish and Seafood"
                },
                 new()
                {
                    Id = 12,
                    Name = "Others"
                },
                 new()
                {
                    Id = 13,
                    Name = "Nuts and seeds"
                },

            };
        }
        internal static ICollection<Measure> SeedMeasures()
        {
            return new HashSet<Measure>()
            {
                new()
                {
                   Id = 1,
                   Name = "pc"
                },
                new()
                { 
                   Id = 2,
                   Name = "clove"
                },
                new()
                { 
                   Id = 3,
                   Name = "ml"
                },
                new()
                { 
                   Id = 4,
                   Name = "l"
                },
                new()
                { 
                   Id = 5,
                   Name = "g"
                },
                new()
                {
                   Id = 6,
                   Name = "kg"
                },
                new()
                {
                   Id = 7,
                   Name = "tsp"
                },
                new()
                {
                   Id = 8,
                   Name = "tbsp"
                },
                new()
                {
                   Id = 9,
                   Name = "cup"
                },
                new()
                {
                   Id = 10,
                   Name = "bunch"
                },
                new()
                {
                   Id = 11,
                   Name = "pkg"
                },
                new()
                {
                   Id = 12,
                   Name = "slice"
                },
                new()
                {
                   Id = 13,
                   Name = "pinch"
                },
            };
        }
        internal static ICollection<Specification> SeedSpecifications()
        {
            return new HashSet<Specification>()
            {
                new() {
                    Id = 1,
                    Description = "frozen"
                },
                new() {
                    Id = 2,
                    Description = "canned"
                },
                new() {
                    Id = 3,
                    Description = "sliced"
                },
                new() {
                    Id = 4,
                    Description = "pre-cooked"
                },
                new() {
                    Id = 5,
                    Description = "grated"
                },
                new() {
                    Id = 6,
                    Description = "fermented"
                },
                new() {
                    Id = 7,
                    Description = "blended"
                },
                new() {
                    Id = 8,
                    Description = "finely-chopped"
                },
                new() {
                    Id = 9,
                    Description = "fresh"
                },
                new() {
                    Id = 10,
                    Description = "squeezed"
                },
                new() {
                    Id = 11,
                    Description = "dried"
                }
            };
        }
        internal static ICollection<Ingredient> SeedIngredients()
        {
            return new HashSet<Ingredient>()
            {
                new()
                {
                    Id = 1,
                    Name = "Beef",
                    CategoryId = 2
                },
                new()
                {
                    Id = 2,
                    Name = "Pork",
                    CategoryId = 2
                },
                new()
                {
                    Id = 3,
                    Name = "Lamb",
                    CategoryId = 2
                },
                new()
                {
                    Id = 4,
                    Name = "Chicken",
                    CategoryId = 2
                },
                new()
                {
                    Id = 5,
                    Name = "Goat",
                    CategoryId = 2
                },
                new()
                {
                    Id = 6,
                    Name = "Turkey",
                    CategoryId = 2
                },
                new()
                {
                    Id = 7,
                    Name = "Rabbit",
                    CategoryId = 2
                },
                new()
                {
                    Id = 8,
                    Name = "Venison",
                    CategoryId = 2
                },
                new()
                {
                    Id = 9,
                    Name = "Duck",
                    CategoryId = 2
                },
                new()
                {
                    Id = 10,
                    Name = "Wild Game Meat",
                    CategoryId = 2
                },
                new()
                {
                    Id = 11,
                    Name = "Ground Meat",
                    CategoryId = 2
                },
                new()
                {
                    Id = 31,
                    Name = "Sausage(s)",
                    CategoryId = 2
                },
                new()
                {
                    Id = 32,
                    Name = "Meatboll(s)",
                    CategoryId = 2
                },
                new()
                {
                    Id = 12,
                    Name = "Egg(s)",
                    CategoryId = 1
                },
                new()
                {
                    Id = 13,
                    Name = "Milk",
                    CategoryId = 1
                },
                new()
                {
                    Id = 14,
                    Name = "Yoghurt",
                    CategoryId = 1
                },
                new()
                {
                    Id = 15,
                    Name = "Cheese",
                    CategoryId = 1
                },
                new()
                {
                    Id = 16,
                    Name = "Yellow cheese",
                    CategoryId = 1
                },
                new()
                {
                    Id = 17,
                    Name = "Cheddar",
                    CategoryId = 1
                },
                new()
                {
                    Id = 18,
                    Name = "Brie",
                    CategoryId = 1
                },
                new()
                {
                    Id = 19,
                    Name = "Feta",
                    CategoryId = 1
                },
                new()
                {
                    Id = 20,
                    Name = "Camembert",
                    CategoryId = 1
                },
                new()
                {
                    Id = 21,
                    Name = "Gouda",
                    CategoryId = 1
                },
                new()
                {
                    Id = 22,
                    Name = "Goat Cheese",
                    CategoryId = 1
                },
                new()
                {
                    Id = 23,
                    Name = "Emmental",
                    CategoryId = 1
                },
                new()
                {
                    Id = 24,
                    Name = "Parmesan",
                    CategoryId = 1
                },
                new()
                {
                    Id = 25,
                    Name = "Ricotta",
                    CategoryId = 1
                },
                new()
                {
                    Id = 26,
                    Name = "Gorgonzola",
                    CategoryId = 1
                },
                new()
                {
                    Id = 27,
                    Name = "Cottage Cheese",
                    CategoryId = 1
                },
                new()
                {
                    Id = 28,
                    Name = "Edam",
                    CategoryId = 1
                },
                new()
                {
                    Id = 29,
                    Name = "Mozzarella",
                    CategoryId = 1
                },
                new()
                {
                    Id = 30,
                    Name = "Pizza Dough",
                    CategoryId = 4
                }, //31 and 32 are for meat
                new()
                {
                    Id = 33,
                    Name = "Lentils",
                    CategoryId = 3
                },
                new()
                {
                    Id = 34,
                    Name = "Chickpeas",
                    CategoryId = 3
                },
                new()
                {
                    Id = 35,
                    Name = "Green Peas",
                    CategoryId = 3
                },
                new()
                {
                    Id = 36,
                    Name = "White Beans",
                    CategoryId = 3
                },
                new()
                {
                    Id = 37,
                    Name = "Soybeans",
                    CategoryId = 3
                },
                new()
                {
                    Id = 38,
                    Name = "Mung Beans",
                    CategoryId = 3
                },
                new()
                {
                    Id = 39,
                    Name = "Red Lentils",
                    CategoryId = 3
                },
                new()
                {
                    Id = 40,
                    Name = "Black Beans",
                    CategoryId = 3
                },
                new()
                {
                    Id = 41,
                    Name = "Edamame",
                    CategoryId = 3
                },
                new()
                {
                    Id = 42,
                    Name = "Green Lentils",
                    CategoryId = 3
                },
                new()
                {
                    Id = 43,
                    Name = "White Flour",
                    CategoryId = 4
                },
                new()
                {
                    Id = 44,
                    Name = "Whole Wheat Flour",
                    CategoryId = 4
                },
                new()
                {
                    Id = 45,
                    Name = "Rye Flour",
                    CategoryId = 4
                },
                new()
                {
                    Id = 46,
                    Name = "Spelt Flour",
                    CategoryId = 4
                },
                new()
                {
                    Id = 47,
                    Name = "Almond Flour",
                    CategoryId = 4
                },
                new()
                {
                    Id = 48,
                    Name = "Pine Nuts",
                    CategoryId = 13
                },
                new ()
                {
                    Id = 49,
                    Name = "Spelled Flour",
                    CategoryId = 4
                },
                new ()
                {
                    Id = 50,
                    Name = "Corn Flour",
                    CategoryId = 4
                },
                new ()
                {
                    Id = 51,
                    Name = "Rice Flour",
                    CategoryId = 4
                },
                new ()
                {
                    Id = 52,
                    Name = "Salt",
                    CategoryId = 5
                },
                new ()
                {
                    Id = 53,
                    Name = "Black Pepper",
                    CategoryId = 5
                },
                new ()
                {
                    Id = 54,
                    Name = "Paprika",
                    CategoryId = 5
                },
                new ()
                {
                    Id = 55,
                    Name = "Turmeric",
                    CategoryId = 5
                },
                new ()
                {
                    Id = 56,
                    Name = "Thyme",
                    CategoryId = 5
                },
                new ()
                {
                    Id = 57,
                    Name = "Oregano",
                    CategoryId = 5
                },
                new ()
                {
                    Id = 58,
                    Name = "Rosemary",
                    CategoryId = 5
                },
                new ()
                {
                    Id = 59,
                    Name = "Mint", 
                    CategoryId = 5
                },
                new ()
                {
                    Id = 60,
                    Name = "Cardamon",
                    CategoryId = 5
                },
                new ()
                {
                    Id = 61,
                    Name = "Curry Powder",
                    CategoryId = 5
                },
                new ()
                {
                    Id = 62,
                    Name = "Chilli powder",
                    CategoryId = 5
                },
                new ()
                {
                    Id = 63,
                    Name = "Ginger",
                    CategoryId = 5
                },
                new ()
                {
                    Id = 64,
                    Name = "Parsley", 
                    CategoryId = 5
                },
                new ()
                {
                    Id = 65,
                    Name = "Bay leaf",
                    CategoryId = 5
                },
                new ()
                {
                    Id = 66,
                    Name = "Allspice", 
                    CategoryId = 5
                },
                new ()
                {
                    Id = 67,
                    Name = "Basil", 
                    CategoryId = 5
                },
                new ()
                {
                    Id = 68,
                    Name = "Clove", 
                    CategoryId = 5
                },
                new ()
                {
                    Id = 69,
                    Name = "Cinnamon",
                    CategoryId = 5
                },
                new ()
                {
                    Id = 70,
                    Name = "Vanilla",
                    CategoryId = 5
                },
                new ()
                {
                    Id = 71,
                    Name = "Celery",
                    CategoryId = 5
                },
                new ()
                {
                    Id = 72,
                    Name = "Peppermint",
                    CategoryId = 5
                },
                new ()
                {
                    Id = 73,
                    Name = "Marjoram",
                    CategoryId = 5
                },
                new ()
                {
                    Id = 74,
                    Name = "Wild Garlic",
                    CategoryId = 5
                },
                new ()
                {
                    Id = 75,
                    Name = "Fennel (dill)",
                    CategoryId = 5
                },
                new ()
                {
                    Id = 76,
                    Name = "Coriander",
                    CategoryId = 5
                },
                new ()
                {
                    Id = 77,
                    Name = "Clary Sage",
                    CategoryId = 5
                },
                new ()
                {
                    Id = 78,
                    Name = "Cilantro",
                    CategoryId = 5
                },
                new ()
                {
                    Id = 79,
                    Name = "Chives", //див лук
                    CategoryId = 5
                },
                new ()
                {
                    Id = 80,
                    Name = "Sugar",
                    CategoryId = 6
                },
                new ()
                {
                    Id = 81,
                    Name = "Honey",
                    CategoryId = 6
                },
                new ()
                {
                    Id = 82,
                    Name = "Maple Syrup",
                    CategoryId = 6
                },
                new ()
                {
                    Id = 83,
                    Name = "Pumpkin",
                    CategoryId = 8
                },
                new ()
                {
                    Id = 84,
                    Name = "Coconut Sugar",
                    CategoryId = 6
                },
                new ()
                {
                    Id = 85,
                    Name = "Powdered Sugar",
                    CategoryId = 6
                },
                new ()
                {
                    Id = 86,
                    Name = "Brown Sugar",
                    CategoryId = 6
                },
                new ()
                {
                    Id = 87,
                    Name = "Molasses", //меласа
                    CategoryId = 6
                },
                new ()
                {
                    Id = 88,
                    Name = "Agave Syrup",
                    CategoryId = 6
                },
                new ()
                {
                    Id = 89,
                    Name = "Cane Sugar",
                    CategoryId = 6
                },
                new ()
                {
                    Id = 90,
                    Name = "Lasagna",
                    CategoryId = 7
                },
                new ()
                {
                    Id = 91,
                    Name = "Spaghetti",
                    CategoryId = 7
                },
                new ()
                {
                    Id = 92,
                    Name = "Tagliatelle",
                    CategoryId = 7
                },
                new ()
                {
                    Id = 93,
                    Name = "Fusilli",
                    CategoryId = 7
                },
                new ()
                {
                    Id = 94,
                    Name = "Gnocchi",
                    CategoryId = 7
                },
                new ()
                {
                    Id = 95,
                    Name = "Macaroni",
                    CategoryId = 7
                },
                new ()
                {
                    Id = 96,
                    Name = "Penne",
                    CategoryId = 7
                },
                new ()
                {
                    Id = 97,
                    Name = "Rigatoni",
                    CategoryId = 7
                },
                new ()
                {
                    Id = 98,
                    Name = "Beet(s)",
                    CategoryId = 8
                },
                new ()
                {
                    Id = 99,
                    Name = "Broccoli",
                    CategoryId = 8
                },
                new ()
                {
                    Id = 100,
                    Name = "Brussels Sprout",
                    CategoryId = 8
                },
                new ()
                {
                    Id = 101,
                    Name = "Cabbage",
                    CategoryId = 8
                },
                new()
                {
                    Id = 102,
                    Name = "Celery",
                    CategoryId = 8
                },
                new()
                {
                    Id = 103,
                    Name = "Kale",
                    CategoryId = 8
                },
                new()
                {
                    Id = 104,
                    Name = "Lettuce",
                    CategoryId = 8
                },
                new()
                {
                    Id = 105,
                    Name = "Spinach",
                    CategoryId = 8
                },
                new()
                {
                    Id = 106,
                    Name = "Asparagus",
                    CategoryId = 8
                },
                new()
                {
                    Id = 107,
                    Name = "Cauliflower",
                    CategoryId = 8
                },
                new()
                {
                    Id = 108,
                    Name = "Eggplant",
                    CategoryId = 8
                },
                new()
                {
                    Id = 109,
                    Name = "Mushrooms",
                    CategoryId = 8
                },
                new()
                {
                    Id = 110,
                    Name = "Nettles",
                    CategoryId = 8
                },
                new()
                {
                    Id = 111,
                    Name = "Leek", //праз лук
                    CategoryId = 8
                },
                new()
                {
                    Id = 112,
                    Name = "Garlic",
                    CategoryId = 8
                },
                new()
                {
                    Id = 113,
                    Name = "Onion",
                    CategoryId = 8
                },
                new()
                {
                    Id = 114,
                    Name = "Carrot",
                    CategoryId = 8
                },
                new()
                {
                    Id = 115,
                    Name = "Celeriac", //целина корен
                    CategoryId = 8
                },
                new()
                {
                    Id = 116,
                    Name = "Ginger Root",
                    CategoryId = 8
                },
                new()
                {
                    Id = 117,
                    Name = "Radish(es)",
                    CategoryId = 8
                },
                new()
                {
                    Id = 118,
                    Name = "Potato(s)",
                    CategoryId = 8
                },
                new()
                {
                    Id = 119,
                    Name = "Sweet Potato(s)",
                    CategoryId = 8
                },
                new()
                {
                    Id = 120,
                    Name = "Sweet Corn",
                    CategoryId = 8
                },
                new()
                {
                    Id = 121,
                    Name = "Zucchini",
                    CategoryId = 8
                },
                new()
                {
                    Id = 122,
                    Name = "Tomatoe(s)",
                    CategoryId = 8
                },
                new()
                {
                    Id = 123,
                    Name = "Cherry Tomatoe(s)",
                    CategoryId = 8
                },
                new()
                {
                    Id = 124,
                    Name = "Green onion(s)",
                    CategoryId = 8
                },
                new()
                {
                    Id = 125,
                    Name = "Cucumber(s)",
                    CategoryId = 8
                },
                new()
                {
                    Id = 126,
                    Name = "Baby spinach",
                    CategoryId = 8
                },
                new()
                {
                    Id = 127,
                    Name = "Red Bell Pepper",
                    CategoryId = 8
                },
                new()
                {
                    Id = 128,
                    Name = "Green Bell Pepper",
                    CategoryId = 8
                },
                new()
                {
                    Id = 129,
                    Name = "Red Onion(s)",
                    CategoryId = 8
                },
                new()
                {
                    Id = 130,
                    Name = "Arugula", //Рукола
                    CategoryId = 8
                },
                new()
                {
                    Id = 131,
                    Name = "Parsnip", //Пащърнак
                    CategoryId = 8
                },
                new()
                {
                    Id = 132,
                    Name = "Apple(s)",
                    CategoryId = 9
                },
                new()
                {
                    Id = 133,
                    Name = "Banana(s)",
                    CategoryId = 9
                },
                new()
                {
                    Id = 134,
                    Name = "Avocado",
                    CategoryId = 9
                },
                new()
                {
                    Id = 135,
                    Name = "Strawberries",
                    CategoryId = 9
                },
                new()
                {
                    Id = 136,
                    Name = "Pear(s)",
                    CategoryId = 9
                },
                new()
                {
                    Id = 137,
                    Name = "Cherries",
                    CategoryId = 9
                },
                new()
                {
                    Id = 138,
                    Name = "Pineapple",
                    CategoryId = 9
                },
                new()
                {
                    Id = 139,
                    Name = "Kiwi",
                    CategoryId = 9
                },
                new()
                {
                    Id = 140,
                    Name = "Orange(s)",
                    CategoryId = 9
                },
                new()
                {
                    Id = 141,
                    Name = "Lemon(s)",
                    CategoryId = 9
                },
                new()
                {
                    Id = 142,
                    Name = "Grapes",
                    CategoryId = 9
                },
                new()
                {
                    Id = 143,
                    Name = "Peaches",
                    CategoryId = 9
                },
                new()
                {
                    Id = 144,
                    Name = "Mango",
                    CategoryId = 9
                },
                new()
                {
                    Id = 145,
                    Name = "Raspberries",
                    CategoryId = 9
                },
                new()
                {
                    Id = 146,
                    Name = "Blueberries",
                    CategoryId = 9
                },
                new()
                {
                    Id = 147,
                    Name = "Plum(s)",
                    CategoryId = 9
                },
                new()
                {
                    Id = 148,
                    Name = "Grapefruit",
                    CategoryId = 9
                },
                new()
                {
                    Id = 149,
                    Name = "Lime",
                    CategoryId = 9
                },
                new()
                {
                    Id = 150,
                    Name = "Prune(s)",
                    CategoryId = 9
                },
                new()
                {
                    Id = 151,
                    Name = "Sour Cherries",
                    CategoryId = 9
                },
                new()
                {
                    Id = 152,
                    Name = "Melon",
                    CategoryId = 9
                },
                new()
                {
                    Id = 153,
                    Name = "Watermelon",
                    CategoryId = 9
                },
                new()
                {
                    Id = 154,
                    Name = "Sunflower Oil",
                    CategoryId = 10
                },
                new()
                {
                    Id = 155,
                    Name = "Olive Oil",
                    CategoryId = 10
                },
                new()
                {
                    Id = 156,
                    Name = "Butter",
                    CategoryId = 10
                },
                new()
                {
                    Id = 157,
                    Name = "Cream",
                    CategoryId = 10
                },
                new()
                {
                    Id = 158,
                    Name = "Coconut Oil",
                    CategoryId = 10
                },
                new()
                {
                    Id = 159,
                    Name = "Avocado Oil",
                    CategoryId = 10
                },
                new()
                {
                    Id = 160,
                    Name = "Ghee",
                    CategoryId = 10
                },
                new()
                {
                    Id = 161,
                    Name = "Lard", //свинска мас
                    CategoryId = 10
                },
                new()
                {
                    Id = 162,
                    Name = "Mascarpone",
                    CategoryId = 10
                },
                new()
                {
                    Id = 163,
                    Name = "Sour Cream",
                    CategoryId = 10
                },
                new()
                {
                    Id = 164,
                    Name = "Whipped Cream",
                    CategoryId = 10
                },
                new()
                {
                    Id = 165,
                    Name = "Coconut Cream",
                    CategoryId = 10
                },
                new()
                {
                    Id = 166,
                    Name = "Diary Free Cream",
                    CategoryId = 10
                },
                new()
                {
                    Id = 167,
                    Name = "Sesame Oil",
                    CategoryId = 10
                },
                // Риба и морски храни
                new()
                {
                    Id = 168,
                    Name = "Trout",
                    CategoryId = 11
                },
                new()
                {
                    Id = 169,
                    Name = "Salmon",
                    CategoryId = 11
                },
                new()
                {
                    Id = 170,
                    Name = "Tuna",
                    CategoryId = 11
                },
                new()
                {
                    Id = 171,
                    Name = "Sardines",
                    CategoryId = 11
                },
                new()
                {
                    Id = 172,
                    Name = "Mackerel", //скумрия
                    CategoryId = 11
                },
                new()
                {
                    Id = 173,
                    Name = "Cod", //треска
                    CategoryId = 11
                },
                new()
                {
                    Id = 174,
                    Name = "Mussels", //миди
                    CategoryId = 11
                },
                new()
                {
                    Id = 175,
                    Name = "Skip Jack", //лефер
                    CategoryId = 11
                },
                new()
                {
                    Id = 176,
                    Name = "Shark",
                    CategoryId = 11
                },
                new()
                {
                    Id = 177,
                    Name = "Shark",
                    CategoryId = 11
                },
                new()
                {
                    Id = 178,
                    Name = "Silver Catfish (Pangasius)",
                    CategoryId = 11
                },
                new()
                {
                    Id = 179,
                    Name = "Octopus",
                    CategoryId = 11
                },
                new()
                {
                    Id = 180,
                    Name = "Squids",
                    CategoryId = 11
                },
                new()
                {
                    Id = 181,
                    Name = "Hake fish", //хек
                    CategoryId = 11
                },
                new()
                {
                    Id = 182,
                    Name = "Salmon Trout", //сьомгова пастърва
                    CategoryId = 11
                },
                new()
                {
                    Id = 183,
                    Name = "Sprat", //цаца
                    CategoryId = 11
                },
                new()
                {
                    Id = 184,
                    Name = "Sardines",
                    CategoryId = 11
                },
                // Others
                new()
                {
                    Id = 185,
                    Name = "Lemon Juice",
                    CategoryId = 12
                },
                new()
                {
                    Id = 186,
                    Name = "Tomatoe Paste",
                    CategoryId = 12
                },
                new()
                {
                    Id = 187,
                    Name = "Mustard",
                    CategoryId = 12
                },
                new()
                {
                    Id = 188,
                    Name = "Savory", //Чубрица
                    CategoryId = 5
                },
                new()
                {
                    Id = 189,
                    Name = "Rice",
                    CategoryId = 7
                },
                new()
                {
                    Id = 190,
                    Name = "Brown Rice",
                    CategoryId = 7
                },
                new()
                {
                    Id = 191,
                    Name = "Black Rice",
                    CategoryId = 7
                },
                new()
                {
                    Id = 192,
                    Name = "Barley",
                    CategoryId = 7
                },
                new()
                {
                    Id = 193,
                    Name = "Bulgur",
                    CategoryId = 7
                },
                new()
                {
                    Id = 194,
                    Name = "Quinoa",
                    CategoryId = 7
                },
                new()
                {
                    Id = 195,
                    Name = "Amaranth",
                    CategoryId = 7
                },
                new()
                {
                    Id = 196,
                    Name = "Oats",
                    CategoryId = 7
                },
                new()
                {
                    Id = 197,
                    Name = "Oatmeal",
                    CategoryId = 7
                },
                new()
                {
                    Id = 198,
                    Name = "Chia Seeds",
                    CategoryId = 13
                },
                new()
                {
                    Id = 199,
                    Name = "Wheat",
                    CategoryId = 7
                },
                new()
                {
                    Id = 200,
                    Name = "Corn",
                    CategoryId = 7
                },
                new()
                {
                    Id = 201,
                    Name = "Spelt",
                    CategoryId = 7
                },
                new()
                {
                    Id = 202,
                    Name = "Buckwheat", // Елда
                    CategoryId = 7
                },
                new()
                {
                    Id = 203,
                    Name = "Millet", // Просо
                    CategoryId = 7
                },
                new()
                {
                    Id = 204,
                    Name = "Farro", // зърната от 3 вида пшеница 
                    CategoryId = 7
                },
                // Nuts here:
                new()
                {
                    Id = 205,
                    Name = "Almonds",
                    CategoryId = 13
                },
                new()
                {
                    Id = 206,
                    Name = "Brazil Nuts",
                    CategoryId = 13
                },
                new()
                {
                    Id = 207,
                    Name = "Cashew Nuts",
                    CategoryId = 13
                },
                new()
                {
                    Id = 208,
                    Name = "Hazel Nuts",
                    CategoryId = 13
                },
                new()
                {
                    Id = 209,
                    Name = "Macadamias",
                    CategoryId = 13
                },
                new()
                {
                    Id = 210,
                    Name = "Pecans",
                    CategoryId = 13
                },
                new()
                {
                    Id = 211,
                    Name = "Pistachios",
                    CategoryId = 13
                },
                new()
                {
                    Id = 212,
                    Name = "Walnuts",
                    CategoryId = 13
                },
                new()
                {
                    Id = 213,
                    Name = "Peanuts",
                    CategoryId = 13
                },
                new()
                {
                    Id = 214,
                    Name = "Pumpkin Seeds",
                    CategoryId = 13
                },
                new()
                {
                    Id = 215,
                    Name = "Flax Seeds",
                    CategoryId = 13
                },
                new()
                {
                    Id = 216,
                    Name = "Sesame Seeds",
                    CategoryId = 13
                },
                new()
                {
                    Id = 217,
                    Name = "Poppy Seeds",
                    CategoryId = 13
                },
                new()
                {
                    Id = 218,
                    Name = "Sunflower Seeds",
                    CategoryId = 13
                },
                new()
                {
                    Id = 219,
                    Name = "Psyllium Seeds",
                    CategoryId = 13
                },
                new Ingredient()
                {
                    Id = 220,
                    Name = "Water",
                    CategoryId = 10
                },
                new Ingredient()
                {
                    Id = 221,
                    Name = "Warm Water",
                    CategoryId = 10
                },
                new()
                {
                    Id = 222,
                    Name = "Vegetable Broth",
                    CategoryId = 10
                },
                new()
                {
                    Id = 223,
                    Name = "Beef Broth",
                    CategoryId = 10
                },
                new()
                {
                    Id = 224,
                    Name = "Fish Broth",
                    CategoryId = 10
                },
                new()
                {
                    Id = 225,
                    Name = "Fish Broth",
                    CategoryId = 10
                },
                new()
                {
                    Id = 226,
                    Name = "Noodles",
                    CategoryId = 7
                },
                new()
                {
                    Id = 227,
                    Name = "Date(s)",
                    CategoryId = 9
                },
                new()
                {
                    Id = 228,
                    Name = "Granola",
                    CategoryId = 7
                },
                new()
                {
                    Id = 229,
                    Name = "Fruits of choice",
                    CategoryId = 9
                },
                new()
                {
                    Id = 230,
                    Name = "Nuts of choice",
                    CategoryId = 13
                },
                new()
                {
                    Id = 231,
                    Name = "Seeds of choice",
                    CategoryId = 13
                },
                new()
                {
                    Id = 232,
                    Name = "Diary-free milk",
                    CategoryId = 12
                },
                new()
                {
                    Id = 233,
                    Name = "Almond milk",
                    CategoryId = 12
                },
                new()
                {
                    Id = 234,
                    Name = "Oat milk",
                    CategoryId = 12
                },
                new()
                {
                    Id = 235,
                    Name = "Soy milk",
                    CategoryId = 12
                },
                new()
                {
                    Id = 236,
                    Name = "Hemp milk",
                    CategoryId = 12
                },
                new()
                {
                    Id = 237,
                    Name = "Cashew milk",
                    CategoryId = 12
                },
                new()
                {
                    Id = 238,
                    Name = "Hazelnut milk",
                    CategoryId = 12
                },
                new()
                {
                    Id = 239,
                    Name = "Tofu",
                    CategoryId = 12
                },
                new()
                {
                    Id = 240,
                    Name = "Bread",
                    CategoryId = 4
                },
                new()
                {
                    Id = 241,
                    Name = "Bread",
                    CategoryId = 4
                },
                new()
                {
                    Id = 242,
                    Name = "Sourdough Bread",
                    CategoryId = 4
                },
                new()
                {
                    Id = 243,
                    Name = "Full Grain Bread",
                    CategoryId = 4
                },
                new()
                {
                    Id = 244,
                    Name = "White Bread",
                    CategoryId = 4
                },
                new()
                {
                    Id = 245,
                    Name = "Rye Bread",
                    CategoryId = 4
                },
                new()
                {
                    Id = 246,
                    Name = "Flatbread",
                    CategoryId = 4
                },
                new()
                {
                    Id = 247,
                    Name = "Brioche",
                    CategoryId = 4
                },
                new()
                {
                    Id = 248,
                    Name = "Baguette",
                    CategoryId = 4
                },
                new()
                {
                    Id = 249,
                    Name = "Ciabatta",
                    CategoryId = 4
                },
                new()
                {
                    Id = 250,
                    Name = "Bread of choice",
                    CategoryId = 4
                },
                new()
                {
                    Id = 251,
                    Name = "Hemp Seeds",
                    CategoryId = 13
                },
                new()
                {
                    Id = 252,
                    Name = "Himalayan Salt",
                    CategoryId = 5
                },
                new()
                {
                    Id = 253,
                    Name = "Pumpkin",
                    CategoryId = 8
                },
                new()
                {
                    Id = 254,
                    Name = "Instant Yeast",
                    CategoryId = 4
                },
                new()
                {
                    Id = 255,
                    Name = "Orange Peels",
                    CategoryId = 5
                },
                new()
                {
                    Id = 256,
                    Name = "Rolled Oats",
                    CategoryId = 7
                },
                new()
                {
                    Id = 257,
                    Name = "Baking Powder",
                    CategoryId = 4
                },
                new()
                {
                    Id = 258,
                    Name = "Nutmeg (ground)",
                    CategoryId = 5
                },
                new()
                {
                    Id = 259,
                    Name = "Baking Soda",
                    CategoryId = 4
                },
                new()
                {
                    Id = 260,
                    Name = "Raisin(s)",
                    CategoryId = 9
                },
                new()
                {
                    Id = 261,
                    Name = "Dried Fruits",
                    CategoryId = 6
                },
            };
        }
        internal static ICollection<Recipe> SeedRecipes()
        {
            return new HashSet<Recipe>()
            {
                new()
                {
                    Id = DeletedRecipeId,
                    OwnerId = Guid.Parse(DeletedUserId),
                    Title = "Deleted Recipe",
                    Description = "This recipe has been deleted.",
                    Servings = 0,
                    TotalTime = TimeSpan.Zero,  
                    ImageUrl = "",
                    CategoryId = 1,
                    IsSiteRecipe = false,
                    IsDeleted = true

                },
                new()
                {
                    Id = Guid.Parse("11112341-30e4-473f-b93a-d0352b978a84"),
                    OwnerId = Guid.Parse(AdminUserId),
                    Title = "Moussaka",
                    Description = "Moussaka is beloved Balkan and Middle East dish. Its preparation depends on the region. In Bulgaria Moussaka is based on potatoes and ground meat. The meal is served warm and Bulgarians eat it very often simply because it’s super delicious and easy to cook. ",
                    
                    Servings = 8,
                    TotalTime = TimeSpan.FromMinutes(120.0),
                    ImageUrl = "https://www.supichka.com/files/images/1242/fit_1400_933.jpg",
                    CategoryId = 4,
                    IsSiteRecipe = true,
                },
                new()
                {
                    Id = Guid.Parse("4a37318d-86fc-4411-a686-b01ae7e007c8"),
                    OwnerId = Guid.Parse(AdminUserId),
                    Title = "Beef Stew",
                    Description = "Savor the essence of a classic beef stew: tender beef, seared to perfection, nestled among hearty potatoes, sweet carrots, and crisp celery in a rich broth. Fragrant herbs and spices dance in each spoonful, invoking warmth and tradition. It's a comforting embrace on chilly nights, a symphony of flavors that transports you to cozy kitchens and cherished gatherings. With its melt-in-your-mouth beef and earthy vegetables, this stew is more than a meal—it's a timeless delight, a celebration of culinary craftsmanship and the simple joys of good food shared with loved ones.",
                    
                    Servings = 4,
                    TotalTime = TimeSpan.FromMinutes(90.0),
                    ImageUrl = "https://www.simplyrecipes.com/thmb/W8uC2OmR-C8WvHiURqfomkvnUnw=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/__opt__aboutcom__coeus__resources__content_migration__simply_recipes__uploads__2015__03__irish-beef-stew-vertical-a2-1800-8012236ba7e34c37abc3baedcab4aff7.jpg",
                    CategoryId = 4,
                    IsSiteRecipe = true,
                },
                new()
                {
                    Id = Guid.Parse("25c6718c-b53b-4092-9454-d6999355f12d"),
                    OwnerId = Guid.Parse(AdminUserId),
                    Title = "Homemade Chicken Soup",
                    Description = "Classical easy and delicious chicken soup to keep you warm in the cold winter days.",
                   
                    Servings = 6,
                    TotalTime = TimeSpan.FromMinutes(60.0),
                    ImageUrl = "https://i2.wp.com/www.downshiftology.com/wp-content/uploads/2023/10/Chicken-Soup-6.jpg",
                    CategoryId = 2,
                    IsSiteRecipe = true,
                },
                new()
                {
                    Id = Guid.Parse("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"),
                    OwnerId = Guid.Parse(AdminUserId),
                    Title = "Stuffed red peppers with ground meat and rice",
                    Description = "This versatile meal is not only simple to make, but feeds families big and small, making it a cheap and easy weeknight dinner legend.",
                   
                    Servings = 8,
                    TotalTime = TimeSpan.FromMinutes(120.0),
                    ImageUrl = "https://hips.hearstapps.com/hmg-prod/images/stuffed-peppers-lead-649c91e2c4e39.jpg",
                    CategoryId = 4,
                    IsSiteRecipe = true,
                },
                new()
                {
                    Id = Guid.Parse("115e248e-3165-425d-aec6-5dda97c99be4"),
                    OwnerId = Guid.Parse(AdminUserId),
                    Title = "Fruity Strawberry Smoothy",
                    Description = "Indulge in a refreshing blend of creamy yogurt, ripe dates, nutrient-rich chia seeds, and succulent strawberries, creating a tantalizing fruity smoothie bursting with flavor and wholesome goodness. Perfect for a quick breakfast boost or a revitalizing snack any time of the day!",
                   
                    Servings = 1,
                    TotalTime = TimeSpan.FromMinutes(10.0),
                    ImageUrl = "https://www.proteincakery.com/wp-content/uploads/2023/11/strawberry-chia-collagen-smoothie.jpg",
                    CategoryId = 1,
                    IsSiteRecipe = true,
                },
                new()
                {
                    Id = Guid.Parse("cd9be7fb-c016-4246-ac36-411f6c3ece14"),
                    OwnerId = Guid.Parse(AdminUserId),
                    Title = "Overnight Oats (prepare the night before)",
                    Description = "Wake up to a simple breakfast solution with our delightful Overnight Oats. A harmonious blend of hearty oats, nutritious chia seeds, ripe banana, creamy milk (whether dairy or dairy-free), crunchy granola, and an assortment of vibrant fruits, all lovingly combined and left to mingle overnight for a deliciously convenient morning meal. Start your day right with this wholesome and customizable dish that promises to energize and satisfy with every spoonful.",
                  
                    Servings = 2,
                    TotalTime = TimeSpan.FromMinutes(10.0),
                    ImageUrl = "https://i0.wp.com/adiligentheart.com/wp-content/uploads/2023/01/image-31.png?w=1000&ssl=1",
                    CategoryId = 1,
                    IsSiteRecipe = true,
                },
                new()
                {
                    Id = Guid.Parse("16541e8d-716c-45d9-8d6d-e3ae70d46c7b"),
                    OwnerId = Guid.Parse(AdminUserId),
                    Title = "Avocado Toast",
                    Description = "Elevate your morning routine with this tasty Avocado Toast! Perfect start of the day for those busy mronings..",
                   
                    Servings = 1,
                    TotalTime = TimeSpan.FromMinutes(10.0),
                    ImageUrl = "https://cookingupmemories.com/wp-content/uploads/2021/01/avocado-toast-with-balsalmic-glaze-breakfast-768x1152.jpg.webp",
                    CategoryId = 1,
                    IsSiteRecipe = true,
                },
                new()
                {
                    Id = Guid.Parse("27664DF3-CB8D-4FF6-A2CF-DA0745A17531"),
                    OwnerId = Guid.Parse(AppUserId),
                    Title = "Beans stew",
                    Description = "Savor the rich aroma and comforting flavors of our bean stew, a delightful blend of tender beans, savory spices, and hearty vegetables. With each spoonful, experience a symphony of taste and texture that warms the soul and satisfies the palate. Perfect for any occasion, our bean stew is a nourishing and delicious treat to be enjoyed alone or shared with loved ones.",
                    
                    Servings = 8,
                    TotalTime = TimeSpan.FromMinutes(120.0),
                    ImageUrl = "https://images.pexels.com/photos/8479384/pexels-photo-8479384.jpeg",
                    CategoryId = 4,
                    IsSiteRecipe = false,
                },
                new()
                {
                    Id = Guid.Parse("294C6ABE-0072-427E-A1E8-355BA414FA5B"),
                    OwnerId = Guid.Parse(AppUserId),
                    Title = "Thai Pumpkin Cream Soup",
                    Description = "Thai pumpkin soup is a creamy and flavorful dish that combines the sweetness of pumpkin with the rich and aromatic flavors of Thai spices such as ginger and coconut milk. This soup offers a perfect balance of creamy texture and vibrant, exotic taste, making it a comforting and satisfying meal, especially during cooler seasons. Enjoyed as a starter or a main course, it's a delightful fusion of Thai cuisine and comforting soup tradition.",
                   
                    Servings = 4,
                    TotalTime = TimeSpan.FromMinutes(40.0),
                    ImageUrl = "https://dishingouthealth.com/wp-content/uploads/2020/09/ThaiPumpkinSoup_Styled2.jpg",
                    CategoryId = 2,
                    IsSiteRecipe = false,
                },
            };
        }
        internal static ICollection<Step> SeedSteps()
        {
            return new HashSet<Step>()
            {
                new Step()
                {
                    Id = 1,
                    RecipeId = Guid.Parse("11112341-30e4-473f-b93a-d0352b978a84"),
                    Description = "Start with cooking the onion in a pan with 1/4 oil until golden brown. Then add the ground meat, the pepper, the paprika, and half the salt. ",
                },
                new Step()
                {
                    Id = 2,
                    RecipeId = Guid.Parse("11112341-30e4-473f-b93a-d0352b978a84"),
                    Description = "Add the tomatoes and fry until they evaporate and the meat gets brown. Then remove the pan from the heat. ",
                },
                new Step()
                {
                    Id = 3,
                    RecipeId = Guid.Parse("11112341-30e4-473f-b93a-d0352b978a84"),
                    Description = "Mix well with the potatoes and the other 1/2 tablespoon of salt. Add the mixture in a casserole pan with the rest of the oil. Bake in oven for about 40 minutes on 425 F (~220 C). ",
                },
                new Step()
                {
                    Id = 4,
                    RecipeId = Guid.Parse("11112341-30e4-473f-b93a-d0352b978a84"),
                    Description = "In the meantime mix the yoghurt and the eggs separately and pour on top  of the meal for the last 10  minutes in the oven untill it turns brownish.",
                },
                new Step()
                {
                    Id = 5,
                    RecipeId = Guid.Parse("4a37318d-86fc-4411-a686-b01ae7e007c8"),
                    Description = "Add the onion, black pepper (beans), parsley, sunflower oil, salt and the beef to a pressure cooker.",
                },
                new Step()
                {
                    Id = 6,
                    Description = "Fill with clean water to a level of 2 fingers above the products. Cook under pressure for about 40 minutes.",
                    RecipeId = Guid.Parse("4a37318d-86fc-4411-a686-b01ae7e007c8"),
                },
                new Step()
                {
                    Id = 7,
                    RecipeId = Guid.Parse("4a37318d-86fc-4411-a686-b01ae7e007c8"),
                    Description = "Open the pressure cooker and strain the broth from the onion and black pepper beans. Portion the meat and remove the meat zip.",
                },
                new Step()
                {
                    Id = 8,
                    RecipeId = Guid.Parse("4a37318d-86fc-4411-a686-b01ae7e007c8"),
                    Description = " Take back to a boil the portioned meat, the bone broth and add the largely cut into pieces carrots, celery root and potatoes. ",
                },
                new Step()
                {
                    Id = 9,
                    RecipeId = Guid.Parse("4a37318d-86fc-4411-a686-b01ae7e007c8"),
                    Description = "Bring the pressure cooker to a boil again and cook for another 20 minutes.",
                },
                new Step()
                {
                    Id = 10,
                    Description = "Boil 2l of water. Add the chicken meat and some salt. Boil until ready or at leas for half an hour.",
                    RecipeId = Guid.Parse("25c6718c-b53b-4092-9454-d6999355f12d"),
                },
                new Step()
                {
                    Id = 11,
                    RecipeId = Guid.Parse("25c6718c-b53b-4092-9454-d6999355f12d"),
                    Description = "Remove the chicken and portion it into small pieces.",
                },
                new Step()
                {
                    Id = 12,
                    RecipeId = Guid.Parse("25c6718c-b53b-4092-9454-d6999355f12d"),
                    Description = "Take the remaining chicken broth back and again bring to a boil. ",
                },
                new Step()
                {
                    Id = 13,
                    RecipeId = Guid.Parse("25c6718c-b53b-4092-9454-d6999355f12d"),
                    Description = "Cut the vegetables into small pieces. First add the carrots and the onions to the boiling broth. ",
                },
                new Step()
                {
                    Id = 14,
                    RecipeId = Guid.Parse("25c6718c-b53b-4092-9454-d6999355f12d"),
                    Description = "After 5 minutes add the cut into small pieces potatoes. 5 minutes later also add the noodles. Finally add the portioned chicken to the soup. ",
                },
                new Step()
                {
                    Id = 15,
                    RecipeId = Guid.Parse("25c6718c-b53b-4092-9454-d6999355f12d"),
                    Description = "After boiling for another 5 minutes, add some finely cut celery.",
                },
                new Step()
                {
                    Id = 16,
                    RecipeId = Guid.Parse("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"),
                    Description = "Finely-chop the onion and carrots. Add to a pre-heated 3-4 tbsp of sunflower oil. Bake for a few minutes. ",
                },
                new Step()
                {
                    Id = 17,
                    RecipeId = Guid.Parse("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"),
                    Description = "Add the minced meat while constantly mixing",
                },
                new Step()
                {
                    Id = 18,
                    RecipeId = Guid.Parse("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"),
                    Description = "Add the tomatoes and leave for the liquid to evaporate. Finally add the rice and the red pepper. Bake for another minute and remove from the stove ",
                },
                new Step()
                {
                    Id = 19,
                    Description = "Add spices according to your taste - at least salt and black pepper (may add also allspice, cumin, etc.) ",
                    RecipeId = Guid.Parse("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"),
                },
                new Step()
                {
                    Id = 20,
                    RecipeId = Guid.Parse("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"),
                    Description = "Stuff the peppers and put them in the oven with a little bit of salty water. Bake for 45mins on 180 degrees.",
                },
                new Step()
                {
                    Id = 21,
                    RecipeId = Guid.Parse("cd9be7fb-c016-4246-ac36-411f6c3ece14"),
                    Description = "Mix the banana and the milk of your choice in a high-speed blender and blend until smooth.",
                },
                new Step()
                {
                    Id = 22,
                    RecipeId = Guid.Parse("cd9be7fb-c016-4246-ac36-411f6c3ece14"),
                    Description = "Divide the rest of ingridients (half a cup rolled-oats, 1tbsp chia seeds and 2tsp sunflower seeds) and place in 2 bowls. ",
                },
                new Step()
                {
                    Id = 23,
                    RecipeId = Guid.Parse("cd9be7fb-c016-4246-ac36-411f6c3ece14"),
                    Description = "Mix well and pour half of the blended milk with banana on top of each bowl. ",
                },
                new Step()
                {
                    Id = 24,
                    RecipeId = Guid.Parse("cd9be7fb-c016-4246-ac36-411f6c3ece14"),
                    Description = "Store in a fridge during the night. ",
                },
                new Step()
                {
                    Id = 25,
                    RecipeId = Guid.Parse("cd9be7fb-c016-4246-ac36-411f6c3ece14"),
                    Description = "The morning after top with granolla and fruits of your choice.",
                },
                new Step()
                {
                    Id = 26,
                    RecipeId = Guid.Parse("16541e8d-716c-45d9-8d6d-e3ae70d46c7b"),
                    Description = "For a delicious twist, grill your slice of bread to your preference. ",
                },
                new Step()
                {
                    Id = 27,
                    RecipeId = Guid.Parse("16541e8d-716c-45d9-8d6d-e3ae70d46c7b"),
                    Description = "Then, simply smash the avocado with a fork and spread it generously over the bread. ",
                },
                new Step()
                {
                    Id = 28,
                    RecipeId = Guid.Parse("16541e8d-716c-45d9-8d6d-e3ae70d46c7b"),
                    Description = "Top it off with sliced cherry tomatoes, sprinkle with Himalayan salt and hemp seeds, and finally squeeze a little bit of lemon juice on top. ",
                },
                new Step()
                {
                    Id = 29,
                    RecipeId = Guid.Parse("16541e8d-716c-45d9-8d6d-e3ae70d46c7b"),
                    Description = "Now, savor the flavors and enjoy your delightful avocado toast!",
                },
                new Step()
                {
                    Id = 30,
                    RecipeId = Guid.Parse("27664DF3-CB8D-4FF6-A2CF-DA0745A17531"),
                    Description = "Prepare the Beans: Soak the beans overnight or for at least 6-8 hours.",
                },
                new Step()
                {
                    Id = 31,
                    RecipeId = Guid.Parse("27664DF3-CB8D-4FF6-A2CF-DA0745A17531"),
                    Description = "Rinse them thoroughly and place them in a pressure cooker without covering. As soon as the beans begin to foam, rinse them with cold water in the sink, then add fresh water to the pot and bring it to a boil.",
                },
                new Step()
                {
                    Id = 32,
                    RecipeId = Guid.Parse("27664DF3-CB8D-4FF6-A2CF-DA0745A17531"),
                    Description = "Prepare the Vegetables: While the beans are cooking, chop the onions, carrots, and peppers into appropriate pieces. ",
                },
                new Step()
                {
                    Id = 33,
                    RecipeId = Guid.Parse("27664DF3-CB8D-4FF6-A2CF-DA0745A17531"),
                    Description = "Place them in the pot with 2-3 tablespoons of sunflower oil. Add paprika and other desired spices, except for salt, at this stage. Do not add salt until later.",
                },
                new Step()
                {
                    Id = 34,
                    RecipeId = Guid.Parse("27664DF3-CB8D-4FF6-A2CF-DA0745A17531"),
                    Description = "Pressure Cook: Close the pressure cooker and cook everything for about 40 minutes. Once done, remove from heat.",
                },
                new Step()
                {
                    Id = 35,
                    RecipeId = Guid.Parse("27664DF3-CB8D-4FF6-A2CF-DA0745A17531"),
                    Description = "Check the Beans: When it's safe to open the pressure cooker, check if the beans are fully cooked. If they are, add the grated or blended tomatoes and salt. Boil for an additional 10 minutes, then reduce heat to low and simmer until ready. ",
                },
                new Step()
                {
                    Id = 36,
                    RecipeId = Guid.Parse("27664DF3-CB8D-4FF6-A2CF-DA0745A17531"),
                    Description = "Final Cooking: For enhanced flavor, allow the stew to sit with the lid on for at least a few hours. ",
                },
                new Step()
                {
                    Id = 37,
                    RecipeId = Guid.Parse("27664DF3-CB8D-4FF6-A2CF-DA0745A17531"),
                    Description = "Serve: When ready to serve, sprinkle finely chopped fresh parsley on top for a burst of freshness. Enjoy your delicious bean stew!",
                },
                new Step()
                {
                    Id = 38,
                    RecipeId = Guid.Parse("294C6ABE-0072-427E-A1E8-355BA414FA5B"),
                    Description = "To cook Thai pumpkin soup, start by sautéing aromatics like onions, (garlic, optional), ginger, and (lemongrass, optional) in a pot until fragrant.",
                },
                new Step()
                {
                    Id = 39,
                    RecipeId = Guid.Parse("294C6ABE-0072-427E-A1E8-355BA414FA5B"),
                    Description = "Add diced pumpkin (or canned pumpkin puree), coconut milk, vegetable broth, and Thai curry paste. Simmer until the pumpkin is tender. ",
                },
                new Step()
                {
                    Id = 40,
                    RecipeId = Guid.Parse("294C6ABE-0072-427E-A1E8-355BA414FA5B"),
                    Description = "Then, blend the soup until smooth using an immersion blender or countertop blender.",
                },
                new Step()
                {
                    Id = 41,
                    RecipeId = Guid.Parse("294C6ABE-0072-427E-A1E8-355BA414FA5B"),
                    Description = "Adjust seasoning with salt, pepper, and lime juice to taste. ",
                },
                new Step()
                {
                    Id = 42,
                    RecipeId = Guid.Parse("294C6ABE-0072-427E-A1E8-355BA414FA5B"),
                    Description = "Serve hot, garnished with fresh cilantro, a swirl of coconut milk, and a sprinkle of chili flakes for extra heat, if desired.",
                },
                new Step()
                {
                    Id = 43,
                    RecipeId = Guid.Parse("115e248e-3165-425d-aec6-5dda97c99be4"),
                    Description = "Begin by soaking the chia seeds in water for about 10-15 minutes to allow them to gel up and soften."
                },
                new Step()
                {
                    Id = 44,
                    RecipeId = Guid.Parse("115e248e-3165-425d-aec6-5dda97c99be4"),
                    Description = "Once the chia seeds have absorbed the water, place them along with the yogurt, pitted dates, and fresh strawberries into a blender."
                },
                new Step()
                {
                    Id = 45,
                    RecipeId = Guid.Parse("115e248e-3165-425d-aec6-5dda97c99be4"),
                    Description = "Blend all the ingredients on high speed until smooth and creamy, ensuring there are no chunks remaining."
                },
                new Step()
                {
                    Id = 46,
                    RecipeId = Guid.Parse("115e248e-3165-425d-aec6-5dda97c99be4"),
                    Description = "our the smoothie into glasses and serve immediately for a delightful and nutritious treat. "
                },
                new Step()
                {
                    Id = 47,
                    RecipeId = Guid.Parse("115e248e-3165-425d-aec6-5dda97c99be4"),
                    Description = "Enjoy your refreshing fruity smoothie!"
                },
            };
        }
        internal static ICollection<RecipeIngredient> SeedRecipeIngredients()
        {
            return new HashSet<RecipeIngredient>()
            {
                // Recipe By Admin (No. 1)
                new()
                {
                    Id = 1,
                    RecipeId = Guid.Parse("11112341-30e4-473f-b93a-d0352b978a84"),
                    IngredientId = 11,
                    Qty = 500,
                    MeasureId = 5,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 2,
                    RecipeId = Guid.Parse("11112341-30e4-473f-b93a-d0352b978a84"),
                    IngredientId = 113,
                    Qty = 2,
                    MeasureId = 1,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 3,
                    RecipeId = Guid.Parse("11112341-30e4-473f-b93a-d0352b978a84"),
                    IngredientId = 118,
                    Qty = 1,
                    MeasureId = 6,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 4,
                    RecipeId = Guid.Parse("11112341-30e4-473f-b93a-d0352b978a84"),
                    IngredientId = 122,
                    Qty = 250,
                    MeasureId = 5,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 5,
                    RecipeId = Guid.Parse("11112341-30e4-473f-b93a-d0352b978a84"),
                    IngredientId = 154,
                    Qty = 3,
                    MeasureId = 8,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 6,
                    RecipeId = Guid.Parse("11112341-30e4-473f-b93a-d0352b978a84"),
                    IngredientId = 54,
                    Qty = 1,
                    MeasureId = 8,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 7,
                    RecipeId = Guid.Parse("11112341-30e4-473f-b93a-d0352b978a84"),
                    IngredientId = 14,
                    Qty = 1,
                    MeasureId = 9,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 8,
                    RecipeId = Guid.Parse("11112341-30e4-473f-b93a-d0352b978a84"),
                    IngredientId = 12,
                    Qty = 2,
                    MeasureId = 1,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 9,
                    RecipeId = Guid.Parse("11112341-30e4-473f-b93a-d0352b978a84"),
                    IngredientId = 52,
                    Qty = 1,
                    MeasureId = 7,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 10,
                    RecipeId = Guid.Parse("11112341-30e4-473f-b93a-d0352b978a84"),
                    IngredientId = 53,
                    Qty = 1,
                    MeasureId = 7,
                    SpecificationId = null,
                }, // Recipe By Admin (No. 2)                    
                new()
                {
                    Id = 11,
                    RecipeId = Guid.Parse("4a37318d-86fc-4411-a686-b01ae7e007c8"),
                    IngredientId = 1,
                    Qty = 400,
                    MeasureId = 5,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 12,
                    RecipeId = Guid.Parse("4a37318d-86fc-4411-a686-b01ae7e007c8"),
                    IngredientId = 118,
                    Qty = 6,
                    MeasureId = 1,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 13,
                    RecipeId = Guid.Parse("4a37318d-86fc-4411-a686-b01ae7e007c8"),
                    IngredientId = 71,
                    Qty = 1,
                    MeasureId = 1,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 14,
                    RecipeId = Guid.Parse("4a37318d-86fc-4411-a686-b01ae7e007c8"),
                    IngredientId = 113,
                    Qty = 1,
                    MeasureId = 1,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 15,
                    RecipeId = Guid.Parse("4a37318d-86fc-4411-a686-b01ae7e007c8"),
                    IngredientId = 114,
                    Qty = 3,
                    MeasureId = 1,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 16,
                    RecipeId = Guid.Parse("4a37318d-86fc-4411-a686-b01ae7e007c8"),
                    IngredientId = 154,
                    Qty = 3,
                    MeasureId = 8,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 17,
                    RecipeId = Guid.Parse("4a37318d-86fc-4411-a686-b01ae7e007c8"),
                    IngredientId = 64,
                    Qty = 1,
                    MeasureId = 10,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 18,
                    RecipeId = Guid.Parse("4a37318d-86fc-4411-a686-b01ae7e007c8"),
                    IngredientId = 52,
                    Qty = 1,
                    MeasureId = 7,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 19,
                    RecipeId = Guid.Parse("4a37318d-86fc-4411-a686-b01ae7e007c8"),
                    IngredientId = 53,
                    Qty = 10,
                    MeasureId = 1,
                    SpecificationId = null,
                }, // Recipe By Admin (No. 3)
                new()
                {
                    Id = 20,
                    RecipeId = Guid.Parse("25c6718c-b53b-4092-9454-d6999355f12d"),
                    IngredientId = 4,
                    Qty = 600,
                    MeasureId = 5,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 21,
                    RecipeId = Guid.Parse("25c6718c-b53b-4092-9454-d6999355f12d"),
                    IngredientId = 114,
                    Qty = 3,
                    MeasureId = 1,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 22,
                    RecipeId = Guid.Parse("25c6718c-b53b-4092-9454-d6999355f12d"),
                    IngredientId = 113,
                    Qty = 1,
                    MeasureId = 1,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 23,
                    RecipeId = Guid.Parse("25c6718c-b53b-4092-9454-d6999355f12d"),
                    IngredientId = 188,
                    Qty = 1,
                    MeasureId = 7,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 24,
                    RecipeId = Guid.Parse("25c6718c-b53b-4092-9454-d6999355f12d"),
                    IngredientId = 53,
                    Qty = 1,
                    MeasureId = 7,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 25,
                    RecipeId = Guid.Parse("25c6718c-b53b-4092-9454-d6999355f12d"),
                    IngredientId = 57,
                    Qty = 1,
                    MeasureId = 7,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 26,
                    RecipeId = Guid.Parse("25c6718c-b53b-4092-9454-d6999355f12d"),
                    IngredientId = 64,
                    Qty = 1,
                    MeasureId = 10,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 27,
                    RecipeId = Guid.Parse("25c6718c-b53b-4092-9454-d6999355f12d"),
                    IngredientId = 52,
                    Qty = 1,
                    MeasureId = 7,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 28,
                    RecipeId = Guid.Parse("25c6718c-b53b-4092-9454-d6999355f12d"),
                    IngredientId = 56,
                    Qty = 1,
                    MeasureId = 7,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 29,
                    RecipeId = Guid.Parse("25c6718c-b53b-4092-9454-d6999355f12d"),
                    IngredientId = 156,
                    Qty = 150,
                    MeasureId = 5,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 30,
                    RecipeId = Guid.Parse("25c6718c-b53b-4092-9454-d6999355f12d"),
                    IngredientId = 163,
                    Qty = 250,
                    MeasureId = 5,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 31,
                    RecipeId = Guid.Parse("25c6718c-b53b-4092-9454-d6999355f12d"),
                    IngredientId = 154,
                    Qty = 1,
                    MeasureId = 8,
                    SpecificationId = null,
                },// Recipe By Admin (No. 4)
                new()
                {
                    Id = 32,
                    RecipeId = Guid.Parse("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"),
                    IngredientId = 2,
                    Qty = 500,
                    MeasureId = 5,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 33,
                    RecipeId = Guid.Parse("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"),
                    IngredientId = 127,
                    Qty = 10,
                    MeasureId = 1,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 34,
                    RecipeId = Guid.Parse("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"),
                    IngredientId = 114,
                    Qty = 1,
                    MeasureId = 1,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 35,
                    RecipeId = Guid.Parse("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"),
                    IngredientId = 122,
                    Qty = 1,
                    MeasureId = 1,
                    SpecificationId = 5,
                },
                new()
                {
                    Id = 36,
                    RecipeId = Guid.Parse("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"),
                    IngredientId = 154,
                    Qty = 4,
                    MeasureId = 8,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 37,
                    RecipeId = Guid.Parse("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"),
                    IngredientId = 189,
                    Qty = 160,
                    MeasureId = 3,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 38,
                    RecipeId = Guid.Parse("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"),
                    IngredientId = 113,
                    Qty = 1,
                    MeasureId = 1,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 39,
                    RecipeId = Guid.Parse("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"),
                    IngredientId = 52,
                    Qty = 1,
                    MeasureId = 7,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 40,
                    RecipeId = Guid.Parse("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"),
                    IngredientId = 53,
                    Qty = 1,
                    MeasureId = 7,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 41,
                    RecipeId = Guid.Parse("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"),
                    IngredientId = 54,
                    Qty = 1,
                    MeasureId = 8,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 42,
                    RecipeId = Guid.Parse("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"),
                    IngredientId = 188,
                    Qty = 1,
                    MeasureId = 7,
                    SpecificationId = null,
                }, // Recipe By Admin (No. 5)
                new()
                {
                    Id = 43,
                    RecipeId = Guid.Parse("115e248e-3165-425d-aec6-5dda97c99be4"),
                    IngredientId = 14,
                    Qty = 1,
                    MeasureId = 9,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 44,
                    RecipeId = Guid.Parse("115e248e-3165-425d-aec6-5dda97c99be4"),
                    IngredientId = 135,
                    Qty = 1,
                    MeasureId = 9,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 45,
                    RecipeId = Guid.Parse("115e248e-3165-425d-aec6-5dda97c99be4"),
                    IngredientId = 198,
                    Qty = 0.5m,
                    MeasureId = 9,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 46,
                    RecipeId = Guid.Parse("115e248e-3165-425d-aec6-5dda97c99be4"),
                    IngredientId = 116,
                    Qty = 1,
                    MeasureId = 12,
                    SpecificationId = 9
                }, // Recipe By Admin (No. 6)
                new()
                {
                    Id = 47,
                    RecipeId = Guid.Parse("cd9be7fb-c016-4246-ac36-411f6c3ece14"),
                    IngredientId = 196,
                    Qty = 0.5m,
                    MeasureId = 9,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 48,
                    RecipeId = Guid.Parse("cd9be7fb-c016-4246-ac36-411f6c3ece14"),
                    IngredientId = 198,
                    Qty = 1,
                    MeasureId = 8,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 49,
                    RecipeId = Guid.Parse("cd9be7fb-c016-4246-ac36-411f6c3ece14"),
                    IngredientId = 218,
                    Qty = 2,
                    MeasureId = 7,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 50,
                    RecipeId = Guid.Parse("cd9be7fb-c016-4246-ac36-411f6c3ece14"),
                    IngredientId = 13,
                    Qty = 1,
                    MeasureId = 9,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 51,
                    RecipeId = Guid.Parse("cd9be7fb-c016-4246-ac36-411f6c3ece14"),
                    IngredientId = 228,
                    Qty = 2,
                    MeasureId = 8,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 52,
                    RecipeId = Guid.Parse("cd9be7fb-c016-4246-ac36-411f6c3ece14"),
                    IngredientId = 229,
                    Qty = 2,
                    MeasureId = 8,
                    SpecificationId = 8
                }, // Recipe By Admin (No 7)
                new()
                {
                    Id = 53,
                    RecipeId = Guid.Parse("16541e8d-716c-45d9-8d6d-e3ae70d46c7b"),
                    IngredientId = 134,
                    Qty = 0.5m,
                    MeasureId = 1,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 54,
                    RecipeId = Guid.Parse("16541e8d-716c-45d9-8d6d-e3ae70d46c7b"),
                    IngredientId = 243,
                    Qty = 1,
                    MeasureId = 12,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 55,
                    RecipeId = Guid.Parse("16541e8d-716c-45d9-8d6d-e3ae70d46c7b"),
                    IngredientId = 251,
                    Qty = 1,
                    MeasureId = 7,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 56,
                    RecipeId = Guid.Parse("16541e8d-716c-45d9-8d6d-e3ae70d46c7b"),
                    IngredientId = 252,
                    Qty = 1,
                    MeasureId = 13,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 57,
                    RecipeId = Guid.Parse("16541e8d-716c-45d9-8d6d-e3ae70d46c7b"),
                    IngredientId = 141,
                    Qty = 1,
                    MeasureId = 1,
                    SpecificationId = 10
                },
                new()
                {
                    Id = 58,
                    RecipeId = Guid.Parse("16541e8d-716c-45d9-8d6d-e3ae70d46c7b"),
                    IngredientId = 123,
                    Qty = 6,
                    MeasureId = 1,
                    SpecificationId = null,
                }, // Recipe by AppUser (No 8)
                new()
                {
                    Id = 59,
                    RecipeId = Guid.Parse("27664DF3-CB8D-4FF6-A2CF-DA0745A17531"),
                    IngredientId = 36,
                    Qty = 500.00m,
                    MeasureId = 5,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 60,
                    RecipeId = Guid.Parse("27664DF3-CB8D-4FF6-A2CF-DA0745A17531"),
                    IngredientId = 52,
                    Qty = 2,
                    MeasureId = 7,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 61,
                    RecipeId = Guid.Parse("27664DF3-CB8D-4FF6-A2CF-DA0745A17531"),
                    IngredientId = 54,
                    Qty = 1,
                    MeasureId = 8,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 62,
                    RecipeId = Guid.Parse("27664DF3-CB8D-4FF6-A2CF-DA0745A17531"),
                    IngredientId = 59,
                    Qty = 1,
                    MeasureId = 7,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 63,
                    RecipeId = Guid.Parse("27664DF3-CB8D-4FF6-A2CF-DA0745A17531"),
                    IngredientId = 64,
                    Qty = 1,
                    MeasureId = 10,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 64,
                    RecipeId = Guid.Parse("27664DF3-CB8D-4FF6-A2CF-DA0745A17531"),
                    IngredientId = 67,
                    Qty = 1,
                    MeasureId = 7,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 65,
                    RecipeId = Guid.Parse("27664DF3-CB8D-4FF6-A2CF-DA0745A17531"),
                    IngredientId = 112,
                    Qty = 1,
                    MeasureId = 2,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 66,
                    RecipeId = Guid.Parse("27664DF3-CB8D-4FF6-A2CF-DA0745A17531"),
                    IngredientId = 113,
                    Qty = 2,
                    MeasureId = 1,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 67,
                    RecipeId = Guid.Parse("27664DF3-CB8D-4FF6-A2CF-DA0745A17531"),
                    IngredientId = 114,
                    Qty = 2,
                    MeasureId = 1,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 68,
                    RecipeId = Guid.Parse("27664DF3-CB8D-4FF6-A2CF-DA0745A17531"),
                    IngredientId = 122,
                    Qty = 2,
                    MeasureId = 1,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 69,
                    RecipeId = Guid.Parse("27664DF3-CB8D-4FF6-A2CF-DA0745A17531"),
                    IngredientId = 127,
                    Qty = 2,
                    MeasureId = 1,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 70,
                    RecipeId = Guid.Parse("27664DF3-CB8D-4FF6-A2CF-DA0745A17531"),
                    IngredientId = 188,
                    Qty = 1,
                    MeasureId = 7,
                    SpecificationId = null,
                },// Recipe by AppUser (No 8)
                new()
                {
                    Id = 71,
                    RecipeId = Guid.Parse("294C6ABE-0072-427E-A1E8-355BA414FA5B"),
                    IngredientId = 116,
                    Qty = 2,
                    MeasureId = 7,
                    SpecificationId = 5
                },
                new()
                {
                    Id = 72,
                    RecipeId = Guid.Parse("294C6ABE-0072-427E-A1E8-355BA414FA5B"),
                    IngredientId = 155,
                    Qty = 1,
                    MeasureId = 8,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 73,
                    RecipeId = Guid.Parse("294C6ABE-0072-427E-A1E8-355BA414FA5B"),
                    IngredientId = 165,
                    Qty = 400.0m,
                    MeasureId = 3,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 74,
                    RecipeId = Guid.Parse("294C6ABE-0072-427E-A1E8-355BA414FA5B"),
                    IngredientId = 222,
                    Qty = 2,
                    MeasureId = 9,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 75,
                    RecipeId = Guid.Parse("294C6ABE-0072-427E-A1E8-355BA414FA5B"),
                    IngredientId = 83,
                    Qty = 1,
                    MeasureId = 6,
                    SpecificationId = null,
                },
                new()
                {
                    Id = 76,
                    RecipeId = Guid.Parse("294C6ABE-0072-427E-A1E8-355BA414FA5B"),
                    IngredientId = 253,
                    Qty = 1,
                    MeasureId = 7,
                    SpecificationId = null,
                },
            };
        }

        internal static ICollection<FavouriteRecipe> SeedRecipeLikes()
        {
            return new HashSet<FavouriteRecipe>()
            {
                new()
                {
                    UserId = Guid.Parse(AppUserId),
                    RecipeId = Guid.Parse("11112341-30e4-473f-b93a-d0352b978a84"),
                },
                new()
                {
                    UserId = Guid.Parse(AppUserId),
                    RecipeId = Guid.Parse("16541e8d-716c-45d9-8d6d-e3ae70d46c7b"),
                },

                new()
                {
                    UserId = Guid.Parse(AppUserId),
                    RecipeId = Guid.Parse("115e248e-3165-425d-aec6-5dda97c99be4"),
                },

            };
        }
                 
        internal static ICollection<MealPlan> SeedMealPlans()
        {
            return new HashSet<MealPlan>()
            {
                new()
                {
                    Id = Guid.Parse("80b65919-165a-4f21-b1bf-42ae7e724351"),
                    Name = "Super cool week, full of tasty bites",
                    OwnerId = Guid.Parse(AppUserId),
                    StartDate = DateTime.ParseExact("10-10-2024",  MealDateFormat, CultureInfo.InvariantCulture),
                    IsFinished = false
                },
                new()
                {
                    Id = Guid.Parse("d74c7ca3-9a16-480c-9127-622919a93c72"),
                    Name = "My first Meal Plan Ever",
                    OwnerId = Guid.Parse(AppUserId),
                    StartDate = DateTime.ParseExact("01-01-2024",  MealDateFormat, CultureInfo.InvariantCulture),
                    IsFinished = true
                },
            };
        }
                  
        internal static ICollection<Meal> SeedMeals()
        {
            return new HashSet<Meal>()
            {
                new()
                {
                    Id = 1,
                    RecipeId = Guid.Parse("11112341-30e4-473f-b93a-d0352b978a84"),
                    ServingSize = 10,
                    CookDate = DateTime.ParseExact("01-01-2024",  MealDateFormat, CultureInfo.InvariantCulture),
                    IsCooked = true,
                    MealPlanId = Guid.Parse("d74c7ca3-9a16-480c-9127-622919a93c72"),
                },
                new()
                {
                    Id = 2,
                    RecipeId = Guid.Parse("25c6718c-b53b-4092-9454-d6999355f12d"),
                    ServingSize = 4,
                    CookDate = DateTime.ParseExact("02-01-2024", MealDateFormat, CultureInfo.InvariantCulture),
                    IsCooked = true,
                    MealPlanId = Guid.Parse("d74c7ca3-9a16-480c-9127-622919a93c72"),
                },
                new()
                {
                    Id = 3,
                    RecipeId = Guid.Parse("27664DF3-CB8D-4FF6-A2CF-DA0745A17531"),
                    ServingSize = 6,
                    CookDate = DateTime.ParseExact("06-01-2024", MealDateFormat, CultureInfo.InvariantCulture),
                    IsCooked = true,
                    MealPlanId = Guid.Parse("d74c7ca3-9a16-480c-9127-622919a93c72"),
                },
                new() // NOT FINISHED
                {
                    Id = 4,
                    RecipeId = Guid.Parse("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"),
                    ServingSize = 4,
                    CookDate = DateTime.ParseExact("10-10-2024",  MealDateFormat, CultureInfo.InvariantCulture),
                    IsCooked = true,
                    MealPlanId = Guid.Parse("80b65919-165a-4f21-b1bf-42ae7e724351"),
                },
                new() // NOT FINISHED
                {
                    Id = 5,
                    RecipeId = Guid.Parse("25c6718c-b53b-4092-9454-d6999355f12d"),
                    ServingSize = 10,
                    CookDate = DateTime.ParseExact("11-10-2024",  MealDateFormat, CultureInfo.InvariantCulture),
                    IsCooked = false,
                    MealPlanId = Guid.Parse("80b65919-165a-4f21-b1bf-42ae7e724351"),
                },
                new() // NOT FINISHED
                {
                    Id = 6,
                    RecipeId = Guid.Parse("cd9be7fb-c016-4246-ac36-411f6c3ece14"),
                    ServingSize = 2,
                    CookDate = DateTime.ParseExact("12-10-2024",  MealDateFormat, CultureInfo.InvariantCulture),
                    IsCooked = false,
                    MealPlanId = Guid.Parse("80b65919-165a-4f21-b1bf-42ae7e724351"),
                },
            };
        }
                 
        internal static ICollection<Tag> SeedTags()
        {
            return new HashSet<Tag>
            {
                // Based on dietary restrictions
                new Tag { Name = "Vegan" },
                new Tag { Name = "Vegetarian" },
                new Tag { Name = "Pescatarian" },
                new Tag { Name = "Gluten-Free" },
                new Tag { Name = "Dairy-Free" },
                new Tag { Name = "Nut-Free" },

                // Based on Special Occasion
                new Tag { Name = "Christmas" },
                new Tag { Name = "Easter" },

                // Based on Time 
                new Tag { Name = "Quick" },
                new Tag { Name = "15-Minute Meals" },
                new Tag { Name = "Slow-Cooked" },
                new Tag { Name = "No-Cook" },

                // Based on audience
                new Tag {Name = "Kid-Friendly"},
                new Tag {Name = "Budget-Friendly"},

                // Based on Season
                new Tag {Name = "Spring"},
                new Tag {Name = "Summer"},
                new Tag {Name = "Autumn (Fall)"},
                new Tag {Name = "Winter"},

                // Based on Unique Characteristics
                new Tag {Name = "Classic"},
                new Tag {Name = "Healthy"},
                new Tag {Name = "Light"},
            };
        }
                
        internal static ICollection<RecipeTag> SeedRecipeTags()
        {
            return new HashSet<RecipeTag>
            {
                new RecipeTag 
                { 
                    RecipeId = Guid.Parse("D6E392D1-4682-425B-A7F9-2EC333AE7CAD"), 
                    TagId = 9
                },
                new RecipeTag
                {
                    RecipeId = Guid.Parse("D6E392D1-4682-425B-A7F9-2EC333AE7CAD"),
                    TagId = 13
                },
                new RecipeTag
                {
                    RecipeId = Guid.Parse("D6E392D1-4682-425B-A7F9-2EC333AE7CAD"),
                    TagId = 20
                },
            };
        }
    }
}
