using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CookTheWeek.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("72ed6dd1-7c97-4af7-ab79-fc72e4a53b16"), 0, "dfa50e88-6543-4e93-8d7b-6014f597fcb0", "admin@gmail.com", true, false, null, "ADMIN@GMAIL.COM", "ADMINUSER", "AQAAAAIAAYagAAAAEBXQLC4/VSLNQxzuD9+DeJ7IFwy3QxUVgrszHXHnenNlSjjm06wNeIEie8vO//6fYg==", null, false, "2d198897-923c-470c-8b6a-1e53187b21f9", false, "adminUser" },
                    { new Guid("e8ec0c24-2dd1-4a7a-aefc-b54bc9a8e403"), 0, "ea289326-45ce-407f-87f3-f3a727ee8480", "appUser@yahoo.com", true, false, null, "APPUSER@YAHOO.COM", "APPUSER", "AQAAAAIAAYagAAAAECcABIkWNHM/Szz/Paz4bZnVbh712ynSaXcAyOzoWkSfbf0CSiGuBa6AdnaTOOg5NA==", null, false, "903d29fd-385b-417b-b6d5-6ba0915e73a1", false, "appUser" }
                });

            migrationBuilder.InsertData(
                table: "IngredientCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Eggs, Milk and Diary products" },
                    { 2, "Meat, Ground Meat and Sausage" },
                    { 3, "Beans, Lentils and Legumes" },
                    { 4, "Flour, Bread and Baking Products" },
                    { 5, "Herbs and Spices" },
                    { 6, "Sweeteners" },
                    { 7, "Pasta and Grains" },
                    { 8, "Vegetables" },
                    { 9, "Fruits" },
                    { 10, "Fats and Oils, Sauces and Broths" },
                    { 11, "Fish and Seafood" },
                    { 12, "Others" },
                    { 13, "Nuts and seeds" }
                });

            migrationBuilder.InsertData(
                table: "Measures",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "pc" },
                    { 2, "clove" },
                    { 3, "ml" },
                    { 4, "l" },
                    { 5, "g" },
                    { 6, "kg" },
                    { 7, "tsp" },
                    { 8, "tbsp" },
                    { 9, "cup" },
                    { 10, "bunch" },
                    { 11, "pkg" },
                    { 12, "slice" },
                    { 13, "pinch" }
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
                    { 9, "fresh" },
                    { 10, "squeezed" },
                    { 11, "dried" }
                });

            migrationBuilder.InsertData(
                table: "Ingredients",
                columns: new[] { "Id", "CategoryId", "Name" },
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
                    { 11, 2, "Ground Meat" },
                    { 12, 1, "Egg(s)" },
                    { 13, 1, "Milk" },
                    { 14, 1, "Yoghurt" },
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
                    { 30, 4, "Pizza Dough" },
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
                    { 48, 13, "Pine Nuts" },
                    { 49, 4, "Spelled Flour" },
                    { 50, 4, "Corn Flour" },
                    { 51, 4, "Rice Flour" },
                    { 52, 5, "Salt" },
                    { 53, 5, "Black Pepper" },
                    { 54, 5, "Paprika" },
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
                    { 83, 8, "Pumpkin" },
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
                    { 122, 8, "Tomatoe(s)" },
                    { 123, 8, "Cherry Tomatoe(s)" },
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
                    { 166, 10, "Diary Free Cream" },
                    { 167, 10, "Sesame Oil" },
                    { 168, 11, "Trout" },
                    { 169, 11, "Salmon" },
                    { 170, 11, "Tuna" },
                    { 171, 11, "Sardines" },
                    { 172, 11, "Mackerel" },
                    { 173, 11, "Cod" },
                    { 174, 11, "Mussels" },
                    { 175, 11, "Skip Jack" },
                    { 176, 11, "Shark" },
                    { 177, 11, "Shark" },
                    { 178, 11, "Silver Catfish (Pangasius)" },
                    { 179, 11, "Octopus" },
                    { 180, 11, "Squids" },
                    { 181, 11, "Hake fish" },
                    { 182, 11, "Salmon Trout" },
                    { 183, 11, "Sprat" },
                    { 184, 11, "Sardines" },
                    { 185, 12, "Lemon Juice" },
                    { 186, 12, "Tomatoe Paste" },
                    { 187, 12, "Mustard" },
                    { 188, 5, "Savory" },
                    { 189, 7, "Rice" },
                    { 190, 7, "Brown Rice" },
                    { 191, 7, "Black Rice" },
                    { 192, 7, "Barley" },
                    { 193, 7, "Bulgur" },
                    { 194, 7, "Quinoa" },
                    { 195, 7, "Amaranth" },
                    { 196, 7, "Oats" },
                    { 197, 7, "Oatmeal" },
                    { 198, 13, "Chia Seeds" },
                    { 199, 7, "Wheat" },
                    { 200, 7, "Corn" },
                    { 201, 7, "Spelt" },
                    { 202, 7, "Buckwheat" },
                    { 203, 7, "Millet" },
                    { 204, 7, "Farro" },
                    { 205, 13, "Almonds" },
                    { 206, 13, "Brazil Nuts" },
                    { 207, 13, "Cashew Nuts" },
                    { 208, 13, "Hazel Nuts" },
                    { 209, 13, "Macadamias" },
                    { 210, 13, "Pecans" },
                    { 211, 13, "Pistachios" },
                    { 212, 13, "Walnuts" },
                    { 213, 13, "Peanuts" },
                    { 214, 13, "Pumpkin Seeds" },
                    { 215, 13, "Flax Seeds" },
                    { 216, 13, "Sesame Seeds" },
                    { 217, 13, "Poppy Seeds" },
                    { 218, 13, "Sunflower Seeds" },
                    { 219, 13, "Psyllium Seeds" },
                    { 220, 10, "Water" },
                    { 221, 10, "Warm Water" },
                    { 222, 10, "Vegetable Broth" },
                    { 223, 10, "Beef Broth" },
                    { 224, 10, "Fish Broth" },
                    { 225, 10, "Fish Broth" },
                    { 226, 7, "Noodles" },
                    { 227, 9, "Date(s)" },
                    { 228, 7, "Granola" },
                    { 229, 9, "Fruits of choice" },
                    { 230, 13, "Nuts of choice" },
                    { 231, 13, "Seeds of choice" },
                    { 232, 12, "Diary-free milk" },
                    { 233, 12, "Almond milk" },
                    { 234, 12, "Oat milk" },
                    { 235, 12, "Soy milk" },
                    { 236, 12, "Hemp milk" },
                    { 237, 12, "Cashew milk" },
                    { 238, 12, "Hazelnut milk" },
                    { 239, 12, "Tofu" },
                    { 240, 4, "Bread" },
                    { 241, 4, "Bread" },
                    { 242, 4, "Sourdough Bread" },
                    { 243, 4, "Full Grain Bread" },
                    { 244, 4, "White Bread" },
                    { 245, 4, "Rye Bread" },
                    { 246, 4, "Flatbread" },
                    { 247, 4, "Brioche" },
                    { 248, 4, "Baguette" },
                    { 249, 4, "Ciabatta" },
                    { 250, 4, "Bread of choice" },
                    { 251, 13, "Hemp Seeds" },
                    { 252, 5, "Himalayan Salt" },
                    { 253, 8, "Pumpkin" },
                    { 254, 4, "Instant Yeast" },
                    { 255, 5, "Orange Peels" },
                    { 256, 7, "Rolled Oats" },
                    { 257, 4, "Baking Powder" },
                    { 258, 5, "Nutmeg (ground)" },
                    { 259, 4, "Baking Soda" },
                    { 260, 9, "Raisin(s)" },
                    { 261, 6, "Dried Fruits" }
                });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "Id", "CategoryId", "Description", "ImageUrl", "IsSiteRecipe", "OwnerId", "Servings", "Title", "TotalTime" },
                values: new object[,]
                {
                    { new Guid("11112341-30e4-473f-b93a-d0352b978a84"), 4, "Moussaka is beloved Balkan and Middle East dish. Its preparation depends on the region. In Bulgaria Moussaka is based on potatoes and ground meat. The meal is served warm and Bulgarians eat it very often simply because it’s super delicious and easy to cook. ", "https://www.supichka.com/files/images/1242/fit_1400_933.jpg", true, new Guid("72ed6dd1-7c97-4af7-ab79-fc72e4a53b16"), 8, "Moussaka", new TimeSpan(0, 2, 0, 0, 0) },
                    { new Guid("115e248e-3165-425d-aec6-5dda97c99be4"), 1, "Indulge in a refreshing blend of creamy yogurt, ripe dates, nutrient-rich chia seeds, and succulent strawberries, creating a tantalizing fruity smoothie bursting with flavor and wholesome goodness. Perfect for a quick breakfast boost or a revitalizing snack any time of the day!", "https://www.proteincakery.com/wp-content/uploads/2023/11/strawberry-chia-collagen-smoothie.jpg", true, new Guid("72ed6dd1-7c97-4af7-ab79-fc72e4a53b16"), 1, "Fruity Strawberry Smoothy", new TimeSpan(0, 0, 10, 0, 0) },
                    { new Guid("16541e8d-716c-45d9-8d6d-e3ae70d46c7b"), 1, "Elevate your morning routine with this tasty Avocado Toast! Perfect start of the day for those busy mronings..", "https://cookingupmemories.com/wp-content/uploads/2021/01/avocado-toast-with-balsalmic-glaze-breakfast-768x1152.jpg.webp", true, new Guid("72ed6dd1-7c97-4af7-ab79-fc72e4a53b16"), 1, "Avocado Toast", new TimeSpan(0, 0, 10, 0, 0) },
                    { new Guid("25c6718c-b53b-4092-9454-d6999355f12d"), 2, "Classical easy and delicious chicken soup to keep you warm in the cold winter days.", "https://i2.wp.com/www.downshiftology.com/wp-content/uploads/2023/10/Chicken-Soup-6.jpg", true, new Guid("72ed6dd1-7c97-4af7-ab79-fc72e4a53b16"), 6, "Homemade Chicken Soup", new TimeSpan(0, 1, 0, 0, 0) },
                    { new Guid("27664df3-cb8d-4ff6-a2cf-da0745a17531"), 4, "Savor the rich aroma and comforting flavors of our bean stew, a delightful blend of tender beans, savory spices, and hearty vegetables. With each spoonful, experience a symphony of taste and texture that warms the soul and satisfies the palate. Perfect for any occasion, our bean stew is a nourishing and delicious treat to be enjoyed alone or shared with loved ones.", "https://images.pexels.com/photos/8479384/pexels-photo-8479384.jpeg", true, new Guid("e8ec0c24-2dd1-4a7a-aefc-b54bc9a8e403"), 8, "Beans stew", new TimeSpan(0, 2, 0, 0, 0) },
                    { new Guid("294c6abe-0072-427e-a1e8-355ba414fa5b"), 2, "Thai pumpkin soup is a creamy and flavorful dish that combines the sweetness of pumpkin with the rich and aromatic flavors of Thai spices such as ginger and coconut milk. This soup offers a perfect balance of creamy texture and vibrant, exotic taste, making it a comforting and satisfying meal, especially during cooler seasons. Enjoyed as a starter or a main course, it's a delightful fusion of Thai cuisine and comforting soup tradition.", "https://dishingouthealth.com/wp-content/uploads/2020/09/ThaiPumpkinSoup_Styled2.jpg", true, new Guid("e8ec0c24-2dd1-4a7a-aefc-b54bc9a8e403"), 4, "Thai Pumpkin Cream Soup", new TimeSpan(0, 0, 40, 0, 0) },
                    { new Guid("4a37318d-86fc-4411-a686-b01ae7e007c8"), 4, "Savor the essence of a classic beef stew: tender beef, seared to perfection, nestled among hearty potatoes, sweet carrots, and crisp celery in a rich broth. Fragrant herbs and spices dance in each spoonful, invoking warmth and tradition. It's a comforting embrace on chilly nights, a symphony of flavors that transports you to cozy kitchens and cherished gatherings. With its melt-in-your-mouth beef and earthy vegetables, this stew is more than a meal—it's a timeless delight, a celebration of culinary craftsmanship and the simple joys of good food shared with loved ones.", "https://www.simplyrecipes.com/thmb/W8uC2OmR-C8WvHiURqfomkvnUnw=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/__opt__aboutcom__coeus__resources__content_migration__simply_recipes__uploads__2015__03__irish-beef-stew-vertical-a2-1800-8012236ba7e34c37abc3baedcab4aff7.jpg", true, new Guid("72ed6dd1-7c97-4af7-ab79-fc72e4a53b16"), 4, "Beef Stew", new TimeSpan(0, 1, 30, 0, 0) },
                    { new Guid("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"), 4, "This versatile meal is not only simple to make, but feeds families big and small, making it a cheap and easy weeknight dinner legend.", "https://hips.hearstapps.com/hmg-prod/images/stuffed-peppers-lead-649c91e2c4e39.jpg", true, new Guid("72ed6dd1-7c97-4af7-ab79-fc72e4a53b16"), 8, "Stuffed red peppers with ground meat and rice", new TimeSpan(0, 2, 0, 0, 0) },
                    { new Guid("cd9be7fb-c016-4246-ac36-411f6c3ece14"), 1, "Wake up to a simple breakfast solution with our delightful Overnight Oats. A harmonious blend of hearty oats, nutritious chia seeds, ripe banana, creamy milk (whether dairy or dairy-free), crunchy granola, and an assortment of vibrant fruits, all lovingly combined and left to mingle overnight for a deliciously convenient morning meal. Start your day right with this wholesome and customizable dish that promises to energize and satisfy with every spoonful.", "https://i0.wp.com/adiligentheart.com/wp-content/uploads/2023/01/image-31.png?w=1000&ssl=1", true, new Guid("72ed6dd1-7c97-4af7-ab79-fc72e4a53b16"), 2, "Overnight Oats (prepare the night beofre)", new TimeSpan(0, 0, 10, 0, 0) }
                });

            migrationBuilder.InsertData(
                table: "RecipesIngredients",
                columns: new[] { "Id", "IngredientId", "MeasureId", "Qty", "RecipeId", "SpecificationId" },
                values: new object[,]
                {
                    { 1, 11, 5, 500m, new Guid("11112341-30e4-473f-b93a-d0352b978a84"), null },
                    { 2, 113, 1, 2m, new Guid("11112341-30e4-473f-b93a-d0352b978a84"), null },
                    { 3, 118, 6, 1m, new Guid("11112341-30e4-473f-b93a-d0352b978a84"), null },
                    { 4, 122, 5, 250m, new Guid("11112341-30e4-473f-b93a-d0352b978a84"), null },
                    { 5, 154, 8, 3m, new Guid("11112341-30e4-473f-b93a-d0352b978a84"), null },
                    { 6, 54, 8, 1m, new Guid("11112341-30e4-473f-b93a-d0352b978a84"), null },
                    { 7, 14, 9, 1m, new Guid("11112341-30e4-473f-b93a-d0352b978a84"), null },
                    { 8, 12, 1, 2m, new Guid("11112341-30e4-473f-b93a-d0352b978a84"), null },
                    { 9, 52, 7, 1m, new Guid("11112341-30e4-473f-b93a-d0352b978a84"), null },
                    { 10, 53, 7, 1m, new Guid("11112341-30e4-473f-b93a-d0352b978a84"), null },
                    { 11, 1, 5, 400m, new Guid("4a37318d-86fc-4411-a686-b01ae7e007c8"), null },
                    { 12, 118, 1, 6m, new Guid("4a37318d-86fc-4411-a686-b01ae7e007c8"), null },
                    { 13, 71, 1, 1m, new Guid("4a37318d-86fc-4411-a686-b01ae7e007c8"), null },
                    { 14, 113, 1, 1m, new Guid("4a37318d-86fc-4411-a686-b01ae7e007c8"), null },
                    { 15, 114, 1, 3m, new Guid("4a37318d-86fc-4411-a686-b01ae7e007c8"), null },
                    { 16, 154, 8, 3m, new Guid("4a37318d-86fc-4411-a686-b01ae7e007c8"), null },
                    { 17, 64, 10, 1m, new Guid("4a37318d-86fc-4411-a686-b01ae7e007c8"), null },
                    { 18, 52, 7, 1m, new Guid("4a37318d-86fc-4411-a686-b01ae7e007c8"), null },
                    { 19, 53, 1, 10m, new Guid("4a37318d-86fc-4411-a686-b01ae7e007c8"), null },
                    { 20, 4, 5, 600m, new Guid("25c6718c-b53b-4092-9454-d6999355f12d"), null },
                    { 21, 114, 1, 3m, new Guid("25c6718c-b53b-4092-9454-d6999355f12d"), null },
                    { 22, 113, 1, 1m, new Guid("25c6718c-b53b-4092-9454-d6999355f12d"), null },
                    { 23, 188, 7, 1m, new Guid("25c6718c-b53b-4092-9454-d6999355f12d"), null },
                    { 24, 53, 7, 1m, new Guid("25c6718c-b53b-4092-9454-d6999355f12d"), null },
                    { 25, 57, 7, 1m, new Guid("25c6718c-b53b-4092-9454-d6999355f12d"), null },
                    { 26, 64, 10, 1m, new Guid("25c6718c-b53b-4092-9454-d6999355f12d"), null },
                    { 27, 52, 7, 1m, new Guid("25c6718c-b53b-4092-9454-d6999355f12d"), null },
                    { 28, 56, 7, 1m, new Guid("25c6718c-b53b-4092-9454-d6999355f12d"), null },
                    { 29, 156, 5, 150m, new Guid("25c6718c-b53b-4092-9454-d6999355f12d"), null },
                    { 30, 163, 5, 250m, new Guid("25c6718c-b53b-4092-9454-d6999355f12d"), null },
                    { 31, 154, 8, 1m, new Guid("25c6718c-b53b-4092-9454-d6999355f12d"), null },
                    { 32, 2, 5, 500m, new Guid("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"), null },
                    { 33, 127, 1, 10m, new Guid("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"), null },
                    { 34, 114, 1, 1m, new Guid("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"), null },
                    { 35, 122, 1, 1m, new Guid("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"), 5 },
                    { 36, 154, 8, 4m, new Guid("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"), null },
                    { 37, 189, 3, 160m, new Guid("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"), null },
                    { 38, 113, 1, 1m, new Guid("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"), null },
                    { 39, 52, 7, 1m, new Guid("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"), null },
                    { 40, 53, 7, 1m, new Guid("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"), null },
                    { 41, 54, 8, 1m, new Guid("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"), null },
                    { 42, 188, 7, 1m, new Guid("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"), null },
                    { 43, 14, 9, 1m, new Guid("115e248e-3165-425d-aec6-5dda97c99be4"), null },
                    { 44, 135, 9, 1m, new Guid("115e248e-3165-425d-aec6-5dda97c99be4"), null },
                    { 45, 198, 9, 0.5m, new Guid("115e248e-3165-425d-aec6-5dda97c99be4"), null },
                    { 46, 116, 12, 1m, new Guid("115e248e-3165-425d-aec6-5dda97c99be4"), 9 },
                    { 47, 196, 9, 0.5m, new Guid("cd9be7fb-c016-4246-ac36-411f6c3ece14"), null },
                    { 48, 198, 8, 1m, new Guid("cd9be7fb-c016-4246-ac36-411f6c3ece14"), null },
                    { 49, 218, 7, 2m, new Guid("cd9be7fb-c016-4246-ac36-411f6c3ece14"), null },
                    { 50, 13, 9, 1m, new Guid("cd9be7fb-c016-4246-ac36-411f6c3ece14"), null },
                    { 51, 228, 8, 2m, new Guid("cd9be7fb-c016-4246-ac36-411f6c3ece14"), null },
                    { 52, 229, 8, 2m, new Guid("cd9be7fb-c016-4246-ac36-411f6c3ece14"), 8 },
                    { 53, 134, 1, 0.5m, new Guid("16541e8d-716c-45d9-8d6d-e3ae70d46c7b"), null },
                    { 54, 243, 12, 1m, new Guid("16541e8d-716c-45d9-8d6d-e3ae70d46c7b"), null },
                    { 55, 251, 7, 1m, new Guid("16541e8d-716c-45d9-8d6d-e3ae70d46c7b"), null },
                    { 56, 252, 13, 1m, new Guid("16541e8d-716c-45d9-8d6d-e3ae70d46c7b"), null },
                    { 57, 141, 1, 1m, new Guid("16541e8d-716c-45d9-8d6d-e3ae70d46c7b"), 10 },
                    { 58, 123, 1, 6m, new Guid("16541e8d-716c-45d9-8d6d-e3ae70d46c7b"), null },
                    { 59, 36, 5, 500.00m, new Guid("27664df3-cb8d-4ff6-a2cf-da0745a17531"), null },
                    { 60, 52, 7, 2m, new Guid("27664df3-cb8d-4ff6-a2cf-da0745a17531"), null },
                    { 61, 54, 8, 1m, new Guid("27664df3-cb8d-4ff6-a2cf-da0745a17531"), null },
                    { 62, 59, 7, 1m, new Guid("27664df3-cb8d-4ff6-a2cf-da0745a17531"), null },
                    { 63, 64, 10, 1m, new Guid("27664df3-cb8d-4ff6-a2cf-da0745a17531"), null },
                    { 64, 67, 7, 1m, new Guid("27664df3-cb8d-4ff6-a2cf-da0745a17531"), null },
                    { 65, 112, 2, 1m, new Guid("27664df3-cb8d-4ff6-a2cf-da0745a17531"), null },
                    { 66, 113, 1, 2m, new Guid("27664df3-cb8d-4ff6-a2cf-da0745a17531"), null },
                    { 67, 114, 1, 2m, new Guid("27664df3-cb8d-4ff6-a2cf-da0745a17531"), null },
                    { 68, 122, 1, 2m, new Guid("27664df3-cb8d-4ff6-a2cf-da0745a17531"), null },
                    { 69, 127, 1, 2m, new Guid("27664df3-cb8d-4ff6-a2cf-da0745a17531"), null },
                    { 70, 188, 7, 1m, new Guid("27664df3-cb8d-4ff6-a2cf-da0745a17531"), null },
                    { 71, 116, 7, 2m, new Guid("294c6abe-0072-427e-a1e8-355ba414fa5b"), 5 },
                    { 72, 155, 8, 1m, new Guid("294c6abe-0072-427e-a1e8-355ba414fa5b"), null },
                    { 73, 165, 3, 400.0m, new Guid("294c6abe-0072-427e-a1e8-355ba414fa5b"), null },
                    { 74, 222, 9, 2m, new Guid("294c6abe-0072-427e-a1e8-355ba414fa5b"), null },
                    { 75, 83, 6, 1m, new Guid("294c6abe-0072-427e-a1e8-355ba414fa5b"), null },
                    { 76, 253, 7, 1m, new Guid("294c6abe-0072-427e-a1e8-355ba414fa5b"), null }
                });

            migrationBuilder.InsertData(
                table: "Steps",
                columns: new[] { "Id", "Description", "RecipeId" },
                values: new object[,]
                {
                    { 1, "Start with cooking the onion in a pan with 1/4 oil until golden brown. Then add the ground meat, the pepper, the paprika, and half the salt. ", new Guid("11112341-30e4-473f-b93a-d0352b978a84") },
                    { 2, "Add the tomatoes and fry until they evaporate and the meat gets brown. Then remove the pan from the heat. ", new Guid("11112341-30e4-473f-b93a-d0352b978a84") },
                    { 3, "Mix well with the potatoes and the other 1/2 tablespoon of salt. Add the mixture in a casserole pan with the rest of the oil. Bake in oven for about 40 minutes on 425 F (~220 C). ", new Guid("11112341-30e4-473f-b93a-d0352b978a84") },
                    { 4, "In the meantime mix the yoghurt and the eggs separately and pour on top  of the meal for the last 10  minutes in the oven untill it turns brownish.", new Guid("11112341-30e4-473f-b93a-d0352b978a84") },
                    { 5, "Add the onion, black pepper (beans), parsley, sunflower oil, salt and the beef to a pressure cooker.", new Guid("4a37318d-86fc-4411-a686-b01ae7e007c8") },
                    { 6, "Fill with clean water to a level of 2 fingers above the products. Cook under pressure for about 40 minutes.", new Guid("4a37318d-86fc-4411-a686-b01ae7e007c8") },
                    { 7, "Open the pressure cooker and strain the broth from the onion and black pepper beans. Portion the meat and remove the meat zip.", new Guid("4a37318d-86fc-4411-a686-b01ae7e007c8") },
                    { 8, " Take back to a boil the portioned meat, the bone broth and add the largely cut into pieces carrots, celery root and potatoes. ", new Guid("4a37318d-86fc-4411-a686-b01ae7e007c8") },
                    { 9, "Bring the pressure cooker to a boil again and cook for another 20 minutes.", new Guid("4a37318d-86fc-4411-a686-b01ae7e007c8") },
                    { 10, "Boil 2l of water. Add the chicken meat and some salt. Boil until ready or at leas for half an hour.", new Guid("25c6718c-b53b-4092-9454-d6999355f12d") },
                    { 11, "Remove the chicken and portion it into small pieces.", new Guid("25c6718c-b53b-4092-9454-d6999355f12d") },
                    { 12, "Take the remaining chicken broth back and again bring to a boil. ", new Guid("25c6718c-b53b-4092-9454-d6999355f12d") },
                    { 13, "Cut the vegetables into small pieces. First add the carrots and the onions to the boiling broth. ", new Guid("25c6718c-b53b-4092-9454-d6999355f12d") },
                    { 14, "After 5 minutes add the cut into small pieces potatoes. 5 minutes later also add the noodles. Finally add the portioned chicken to the soup. ", new Guid("25c6718c-b53b-4092-9454-d6999355f12d") },
                    { 15, "After boiling for another 5 minutes, add some finely cut celery.", new Guid("25c6718c-b53b-4092-9454-d6999355f12d") },
                    { 16, "1. Finely-chop the onion and carrots. Add to a pre-heated 3-4 tbsp of sunflower oil. Bake for a few minutes. ", new Guid("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb") },
                    { 17, "2. Add the minced meat while constantly mixing", new Guid("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb") },
                    { 18, "3. Add the tomatoes and leave for the liquid to evaporate. Finally add the rice and the red pepper. Bake for another minute and remove from the stove ", new Guid("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb") },
                    { 19, "4. Add spices according to your taste - at least salt and black pepper (may add also allspice, cumin, etc.) ", new Guid("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb") },
                    { 20, "5. Stuff the peppers and put them in the oven with a little bit of salty water. Bake for 45mins on 180 degrees.", new Guid("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb") },
                    { 21, "Mix the banana and the milk of your choice in a high-speed blender and blend until smooth.", new Guid("cd9be7fb-c016-4246-ac36-411f6c3ece14") },
                    { 22, "Divide the rest of ingridients (half a cup rolled-oats, 1tbsp chia seeds and 2tsp sunflower seeds) and place in 2 bowls. ", new Guid("cd9be7fb-c016-4246-ac36-411f6c3ece14") },
                    { 23, "Mix well and pour half of the blended milk with banana on top of each bowl. ", new Guid("cd9be7fb-c016-4246-ac36-411f6c3ece14") },
                    { 24, "Store in a fridge during the night. ", new Guid("cd9be7fb-c016-4246-ac36-411f6c3ece14") },
                    { 25, "The morning after top with granolla and fruits of your choice.", new Guid("cd9be7fb-c016-4246-ac36-411f6c3ece14") },
                    { 26, "For a delicious twist, grill your slice of bread to your preference. ", new Guid("16541e8d-716c-45d9-8d6d-e3ae70d46c7b") },
                    { 27, "Then, simply smash the avocado with a fork and spread it generously over the bread. ", new Guid("16541e8d-716c-45d9-8d6d-e3ae70d46c7b") },
                    { 28, "Top it off with sliced cherry tomatoes, sprinkle with Himalayan salt and hemp seeds, and finally squeeze a little bit of lemon juice on top. ", new Guid("16541e8d-716c-45d9-8d6d-e3ae70d46c7b") },
                    { 29, "Now, savor the flavors and enjoy your delightful avocado toast!", new Guid("16541e8d-716c-45d9-8d6d-e3ae70d46c7b") },
                    { 30, "Prepare the Beans: Soak the beans overnight or for at least 6-8 hours.", new Guid("27664df3-cb8d-4ff6-a2cf-da0745a17531") },
                    { 31, "Rinse them thoroughly and place them in a pressure cooker without covering. As soon as the beans begin to foam, rinse them with cold water in the sink, then add fresh water to the pot and bring it to a boil.", new Guid("27664df3-cb8d-4ff6-a2cf-da0745a17531") },
                    { 32, "Prepare the Vegetables: While the beans are cooking, chop the onions, carrots, and peppers into appropriate pieces. ", new Guid("27664df3-cb8d-4ff6-a2cf-da0745a17531") },
                    { 33, "Place them in the pot with 2-3 tablespoons of sunflower oil. Add paprika and other desired spices, except for salt, at this stage. Do not add salt until later.", new Guid("27664df3-cb8d-4ff6-a2cf-da0745a17531") },
                    { 34, "Pressure Cook: Close the pressure cooker and cook everything for about 40 minutes. Once done, remove from heat.", new Guid("27664df3-cb8d-4ff6-a2cf-da0745a17531") },
                    { 35, "Check the Beans: When it's safe to open the pressure cooker, check if the beans are fully cooked. If they are, add the grated or blended tomatoes and salt. Boil for an additional 10 minutes, then reduce heat to low and simmer until ready. ", new Guid("27664df3-cb8d-4ff6-a2cf-da0745a17531") },
                    { 36, "Final Cooking: For enhanced flavor, allow the stew to sit with the lid on for at least a few hours. ", new Guid("27664df3-cb8d-4ff6-a2cf-da0745a17531") },
                    { 37, "Serve: When ready to serve, sprinkle finely chopped fresh parsley on top for a burst of freshness. Enjoy your delicious bean stew!", new Guid("27664df3-cb8d-4ff6-a2cf-da0745a17531") },
                    { 38, "To cook Thai pumpkin soup, start by sautéing aromatics like onions, (garlic, optional), ginger, and (lemongrass, optional) in a pot until fragrant.", new Guid("294c6abe-0072-427e-a1e8-355ba414fa5b") },
                    { 39, "Add diced pumpkin (or canned pumpkin puree), coconut milk, vegetable broth, and Thai curry paste. Simmer until the pumpkin is tender. ", new Guid("294c6abe-0072-427e-a1e8-355ba414fa5b") },
                    { 40, "Then, blend the soup until smooth using an immersion blender or countertop blender.", new Guid("294c6abe-0072-427e-a1e8-355ba414fa5b") },
                    { 41, "Adjust seasoning with salt, pepper, and lime juice to taste. ", new Guid("294c6abe-0072-427e-a1e8-355ba414fa5b") },
                    { 42, "Serve hot, garnished with fresh cilantro, a swirl of coconut milk, and a sprinkle of chili flakes for extra heat, if desired.", new Guid("294c6abe-0072-427e-a1e8-355ba414fa5b") },
                    { 43, "Begin by soaking the chia seeds in water for about 10-15 minutes to allow them to gel up and soften.", new Guid("115e248e-3165-425d-aec6-5dda97c99be4") },
                    { 44, "Once the chia seeds have absorbed the water, place them along with the yogurt, pitted dates, and fresh strawberries into a blender.", new Guid("115e248e-3165-425d-aec6-5dda97c99be4") },
                    { 45, "Blend all the ingredients on high speed until smooth and creamy, ensuring there are no chunks remaining.", new Guid("115e248e-3165-425d-aec6-5dda97c99be4") },
                    { 46, "our the smoothie into glasses and serve immediately for a delightful and nutritious treat. ", new Guid("115e248e-3165-425d-aec6-5dda97c99be4") },
                    { 47, "Enjoy your refreshing fruity smoothie!", new Guid("115e248e-3165-425d-aec6-5dda97c99be4") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 119);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 121);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 124);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 125);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 126);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 128);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 129);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 130);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 131);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 132);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 133);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 136);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 137);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 138);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 139);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 140);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 142);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 143);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 144);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 145);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 146);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 147);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 148);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 149);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 150);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 151);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 152);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 153);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 157);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 158);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 159);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 160);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 161);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 162);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 164);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 166);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 167);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 168);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 169);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 170);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 171);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 172);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 173);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 174);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 175);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 176);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 177);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 178);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 179);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 180);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 181);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 182);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 183);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 184);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 185);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 186);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 187);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 190);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 191);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 192);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 193);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 194);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 195);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 197);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 199);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 200);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 201);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 202);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 203);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 204);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 205);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 206);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 207);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 208);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 209);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 210);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 211);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 212);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 213);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 214);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 215);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 216);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 217);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 219);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 220);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 221);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 223);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 224);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 225);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 226);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 227);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 230);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 231);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 232);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 233);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 234);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 235);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 236);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 237);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 238);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 239);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 240);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 241);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 242);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 244);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 245);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 246);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 247);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 248);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 249);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 250);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 254);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 255);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 256);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 257);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 258);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 259);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 260);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 261);

            migrationBuilder.DeleteData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "RecipeCategories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "RecipeCategories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "RecipeCategories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "Specifications",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Specifications",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Specifications",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Specifications",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Specifications",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Specifications",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Specifications",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Steps",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "IngredientCategories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "IngredientCategories",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "IngredientCategories",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 118);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 123);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 127);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 134);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 135);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 141);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 154);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 155);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 156);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 163);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 165);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 188);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 189);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 196);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 198);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 218);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 222);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 228);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 229);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 243);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 251);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 252);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 253);

            migrationBuilder.DeleteData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("11112341-30e4-473f-b93a-d0352b978a84"));

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("115e248e-3165-425d-aec6-5dda97c99be4"));

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("16541e8d-716c-45d9-8d6d-e3ae70d46c7b"));

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("25c6718c-b53b-4092-9454-d6999355f12d"));

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("27664df3-cb8d-4ff6-a2cf-da0745a17531"));

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("294c6abe-0072-427e-a1e8-355ba414fa5b"));

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("4a37318d-86fc-4411-a686-b01ae7e007c8"));

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"));

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("cd9be7fb-c016-4246-ac36-411f6c3ece14"));

            migrationBuilder.DeleteData(
                table: "Specifications",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Specifications",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Specifications",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Specifications",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("72ed6dd1-7c97-4af7-ab79-fc72e4a53b16"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e8ec0c24-2dd1-4a7a-aefc-b54bc9a8e403"));

            migrationBuilder.DeleteData(
                table: "IngredientCategories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "IngredientCategories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "IngredientCategories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "IngredientCategories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "IngredientCategories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "IngredientCategories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "IngredientCategories",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "IngredientCategories",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "IngredientCategories",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "IngredientCategories",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "RecipeCategories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RecipeCategories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RecipeCategories",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
