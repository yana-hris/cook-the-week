namespace CookTheWeek.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models.IgredientEntities;
    public class IngredientEntityConfiguration : IEntityTypeConfiguration<Ingredient>
    {
        public void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            builder
                .HasData(GenerateMainIngredients());
        }

        private ICollection<Ingredient> GenerateMainIngredients()
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
                    Name = "Minced Meat",
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
                    Name = "Yogurt",
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
                    Name = "Tomato(s)",
                    IngredientCategoryId = 8
                },
                new Ingredient()
                {
                    Id = 123,
                    Name = "Cherry Tomato(s)",
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
                    Id = 167,
                    Name = "Diary Free Cream",
                    IngredientCategoryId = 10
                },
                new Ingredient()
                {
                    Id = 168,
                    Name = "Sesame Oil",
                    IngredientCategoryId = 10
                },
                // Риба и морски храни
                new Ingredient()
                {
                    Id = 169,
                    Name = "Trout",
                    IngredientCategoryId = 11
                },
                new Ingredient()
                {
                    Id = 170,
                    Name = "Salmon",
                    IngredientCategoryId = 11
                },
                new Ingredient()
                {
                    Id = 171,
                    Name = "Tuna",
                    IngredientCategoryId = 11
                },
                new Ingredient()
                {
                    Id = 172,
                    Name = "Sardines",
                    IngredientCategoryId = 11
                },                
                new Ingredient()
                {
                    Id = 173,
                    Name = "Mackerel", //скумрия
                    IngredientCategoryId = 11
                },
                new Ingredient()
                {
                    Id = 174,
                    Name = "Cod", //треска
                    IngredientCategoryId = 11
                },
                new Ingredient()
                {
                    Id = 175,
                    Name = "Mussels", //миди
                    IngredientCategoryId = 11
                },
                new Ingredient()
                {
                    Id = 176,
                    Name = "Skip Jack", //лефер
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
                    Name = "Shark",
                    IngredientCategoryId = 11
                },
                new Ingredient()
                {
                    Id = 179,
                    Name = "Silver Catfish (Pangasius)",
                    IngredientCategoryId = 11
                },
                new Ingredient()
                {
                    Id = 180,
                    Name = "Octopus",
                    IngredientCategoryId = 11
                },
                new Ingredient()
                {
                    Id = 181,
                    Name = "Squids",
                    IngredientCategoryId = 11
                },
                new Ingredient()
                {
                    Id = 182,
                    Name = "Hake fish", //хек
                    IngredientCategoryId = 11
                },
                new Ingredient()
                {
                    Id = 183,
                    Name = "Salmon Trout", //сьомгова пастърва
                    IngredientCategoryId = 11
                },
                new Ingredient()
                {
                    Id = 184,
                    Name = "Sprat", //цаца
                    IngredientCategoryId = 11
                },
                new Ingredient()
                {
                    Id = 185,
                    Name = "Sardines",
                    IngredientCategoryId = 11
                },
                // Others
                new Ingredient()
                {
                    Id = 186,
                    Name = "Lemon Juice", 
                    IngredientCategoryId = 12
                },
                new Ingredient()
                {
                    Id = 187,
                    Name = "Tomatoe Paste",
                    IngredientCategoryId = 12
                },
                new Ingredient()
                {
                    Id = 188,
                    Name = "Mustard",
                    IngredientCategoryId = 12
                },
                new Ingredient()
                {
                    Id = 189,
                    Name = "Savory", //Чубрица
                    IngredientCategoryId = 5
                },
                new Ingredient()
                {
                    Id = 221,
                    Name = "Rice",
                    IngredientCategoryId = 7
                },
                new Ingredient()
                {
                    Id = 222,
                    Name = "Brown Rice",
                    IngredientCategoryId = 7
                },
                new Ingredient()
                {
                    Id = 192,
                    Name = "Black Rice",
                    IngredientCategoryId = 7
                },
                new Ingredient()
                {
                    Id = 193,
                    Name = "Barley",
                    IngredientCategoryId = 7
                },
                new Ingredient()
                {
                    Id = 194,
                    Name = "Bulgur",
                    IngredientCategoryId = 7
                },
                new Ingredient()
                {
                    Id = 195,
                    Name = "Quinoa",
                    IngredientCategoryId = 7
                },
                new Ingredient()
                {
                    Id = 196,
                    Name = "Amaranth",
                    IngredientCategoryId = 7
                },
                new Ingredient()
                {
                    Id = 197,
                    Name = "Oats",
                    IngredientCategoryId = 7
                },
                new Ingredient()
                {
                    Id = 198,
                    Name = "Oatmeal",
                    IngredientCategoryId = 7
                },
                new Ingredient()
                {
                    Id = 199,
                    Name = "Chia Seeds",
                    IngredientCategoryId = 13
                },
                new Ingredient()
                {
                    Id = 200,
                    Name = "Wheat",
                    IngredientCategoryId = 7
                },
                new Ingredient()
                {
                    Id = 201,
                    Name = "Corn",
                    IngredientCategoryId = 7
                },
                new Ingredient()
                {
                    Id = 202,
                    Name = "Spelt",
                    IngredientCategoryId = 7
                },
                new Ingredient()
                {
                    Id = 203,
                    Name = "Buckwheat", // Елда
                    IngredientCategoryId = 7
                },
                new Ingredient()
                {
                    Id = 204,
                    Name = "Millet", // Просо
                    IngredientCategoryId = 7
                },
                new Ingredient()
                {
                    Id = 205,
                    Name = "Farro", // зърната от 3 вида пшеница 
                    IngredientCategoryId = 7
                },
                // Nuts here:
                new Ingredient()
                {
                    Id = 206,
                    Name = "Almonds", 
                    IngredientCategoryId = 13
                },
                new Ingredient()
                {
                    Id = 207,
                    Name = "Brazil Nuts",
                    IngredientCategoryId = 13
                },
                new Ingredient()
                {
                    Id = 208,
                    Name = "Cashew Nuts",
                    IngredientCategoryId = 13
                },
                new Ingredient()
                {
                    Id = 209,
                    Name = "Hazel Nuts",
                    IngredientCategoryId = 13
                },
                new Ingredient()
                {
                    Id = 210,
                    Name = "Macadamias",
                    IngredientCategoryId = 13
                },
                new Ingredient()
                {
                    Id = 211,
                    Name = "Pecans",
                    IngredientCategoryId = 13
                },
                new Ingredient()
                {
                    Id = 212,
                    Name = "Pistachios",
                    IngredientCategoryId = 13
                },
                new Ingredient()
                {
                    Id = 213,
                    Name = "Walnuts",
                    IngredientCategoryId = 13
                },
                new Ingredient()
                {
                    Id = 214,
                    Name = "Peanuts",
                    IngredientCategoryId = 13
                },
                new Ingredient()
                {
                    Id = 215,
                    Name = "Pumpkin Seeds",
                    IngredientCategoryId = 13
                },
                new Ingredient()
                {
                    Id = 216,
                    Name = "Flax Seeds",
                    IngredientCategoryId = 13
                },
                new Ingredient()
                {
                    Id = 217,
                    Name = "Sesame Seeds",
                    IngredientCategoryId = 13
                },
                new Ingredient()
                {
                Id = 218,
                Name = "Poppy Seeds",
                IngredientCategoryId = 13
                },
                new Ingredient()
                {
                    Id = 219,
                    Name = "Sunflower Seeds",
                    IngredientCategoryId = 13
                },
                new Ingredient()
                {
                    Id = 220,
                    Name = "Psyllium Seeds",
                    IngredientCategoryId = 13
                },
            };
        }
    }
}
