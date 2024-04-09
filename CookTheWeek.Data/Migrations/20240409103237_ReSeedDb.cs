using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CookTheWeek.Data.Migrations
{
    /// <inheritdoc />
    public partial class ReSeedDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 220);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 221);

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 11, new Guid("377a3703-e719-42db-b013-19e5bd23892a") });

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 54, new Guid("377a3703-e719-42db-b013-19e5bd23892a") });

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 113, new Guid("377a3703-e719-42db-b013-19e5bd23892a") });

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 118, new Guid("377a3703-e719-42db-b013-19e5bd23892a") });

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 122, new Guid("377a3703-e719-42db-b013-19e5bd23892a") });

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 154, new Guid("377a3703-e719-42db-b013-19e5bd23892a") });

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("377a3703-e719-42db-b013-19e5bd23892a"));

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("72ed6dd1-7c97-4af7-ab79-fc72e4a53b16"), 0, "3079126a-d0d5-4de0-abfe-f1ba917ea845", "admin@gmail.com", true, false, null, "ADMIN@GMAIL.COM", "ADMINUSER", "AQAAAAIAAYagAAAAEOpskLdBqYRyQGZRywtEmGs/AzlvYf9B6s5QCxvfYjshj/+IPr4gszo39eOlHkQPQA==", null, false, "f74b262a-1137-4d67-a92c-dca40cf12591", false, "adminUser" },
                    { new Guid("e8ec0c24-2dd1-4a7a-aefc-b54bc9a8e403"), 0, "7e673843-3fde-456f-8aa7-23c9196049f4", "appUser@yahoo.com", true, false, null, "APPUSER@YAHOO.COM", "APPUSER", "AQAAAAIAAYagAAAAEJoXIkIWdfEbtNwVCKmW0/ryGpGyAYm/FUXlG0TM9o25jAbLn9BkVrX16r15vUXJ/Q==", null, false, "eb724f78-b433-4c1b-a451-e2550744b50c", false, "appUser" }
                });

            migrationBuilder.UpdateData(
                table: "IngredientCategories",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Flour, Bread and Baking Products");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 11,
                column: "Name",
                value: "Ground meat");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 14,
                column: "Name",
                value: "Yoghurt");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 122,
                column: "Name",
                value: "Tomatoe(s)");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 123,
                column: "Name",
                value: "Cherry tomatoe(s)");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 167,
                column: "Name",
                value: "Sesame oil");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 168,
                columns: new[] { "IngredientCategoryId", "Name" },
                values: new object[] { 11, "Trout" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 169,
                column: "Name",
                value: "Salmon");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 170,
                column: "Name",
                value: "Tuna");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 171,
                column: "Name",
                value: "Sardines");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 172,
                column: "Name",
                value: "Mackerel");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 173,
                column: "Name",
                value: "Cod");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 174,
                column: "Name",
                value: "Mussels");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 175,
                column: "Name",
                value: "Skip jack");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 176,
                column: "Name",
                value: "Shark");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 178,
                column: "Name",
                value: "Silver catfish(");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 179,
                column: "Name",
                value: "Octopus");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 180,
                column: "Name",
                value: "Squids");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 181,
                column: "Name",
                value: "Hake fish");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 182,
                column: "Name",
                value: "Salmon trout");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 183,
                column: "Name",
                value: "Sprat");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 184,
                column: "Name",
                value: "Sardines");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 185,
                columns: new[] { "IngredientCategoryId", "Name" },
                values: new object[] { 12, "Lemon juice" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 186,
                column: "Name",
                value: "Tomatoe paste");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 187,
                column: "Name",
                value: "Mustard");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 188,
                columns: new[] { "IngredientCategoryId", "Name" },
                values: new object[] { 5, "Savory" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 189,
                columns: new[] { "IngredientCategoryId", "Name" },
                values: new object[] { 7, "Rice" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 192,
                column: "Name",
                value: "Barley");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 193,
                column: "Name",
                value: "Bulgur");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 194,
                column: "Name",
                value: "Quinoa");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 195,
                column: "Name",
                value: "Amaranth");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 196,
                column: "Name",
                value: "Oats");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 197,
                column: "Name",
                value: "Oatmeal");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 198,
                columns: new[] { "IngredientCategoryId", "Name" },
                values: new object[] { 13, "Chia seeds" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 199,
                columns: new[] { "IngredientCategoryId", "Name" },
                values: new object[] { 7, "Wheat" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 200,
                column: "Name",
                value: "Corn");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 201,
                column: "Name",
                value: "Spelt");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 202,
                column: "Name",
                value: "Buckwheat");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 203,
                column: "Name",
                value: "Millet");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 204,
                column: "Name",
                value: "Farro");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 205,
                columns: new[] { "IngredientCategoryId", "Name" },
                values: new object[] { 13, "Almonds" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 206,
                column: "Name",
                value: "Brazil nuts");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 207,
                column: "Name",
                value: "Cashew nuts");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 208,
                column: "Name",
                value: "Hazel nuts");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 209,
                column: "Name",
                value: "Macadamias");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 210,
                column: "Name",
                value: "Pecans");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 211,
                column: "Name",
                value: "Pistachios");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 212,
                column: "Name",
                value: "Walnuts");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 213,
                column: "Name",
                value: "Peanuts");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 214,
                column: "Name",
                value: "Pumpkin seeds");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 215,
                column: "Name",
                value: "Flax seeds");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 216,
                column: "Name",
                value: "Sesame seeds");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 217,
                column: "Name",
                value: "Poppy seeds");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 218,
                column: "Name",
                value: "Sunflower seeds");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 219,
                column: "Name",
                value: "Psyllium seeds");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 222,
                columns: new[] { "IngredientCategoryId", "Name" },
                values: new object[] { 12, "Vegetable broth" });

            migrationBuilder.InsertData(
                table: "Ingredients",
                columns: new[] { "Id", "IngredientCategoryId", "Name" },
                values: new object[,]
                {
                    { 166, 10, "Diary free cream" },
                    { 190, 7, "Brown rice" },
                    { 191, 7, "Black rice" },
                    { 223, 12, "Beef broth" },
                    { 224, 12, "Fish broth" },
                    { 225, 12, "Fish broth" },
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
                    { 242, 4, "Sourdough bread" },
                    { 243, 4, "Full grain bread" },
                    { 244, 4, "White bread" },
                    { 245, 4, "Rye bread" },
                    { 246, 4, "Flatbread" },
                    { 247, 4, "Brioche" },
                    { 248, 4, "Baguette" },
                    { 249, 4, "Ciabatta" },
                    { 250, 4, "Bread of choice" },
                    { 251, 13, "Hemp seeds" },
                    { 252, 5, "Himalayan salt" }
                });

            migrationBuilder.InsertData(
                table: "Measures",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 12, "slice(s)" },
                    { 13, "pinch(es)" }
                });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "Id", "Description", "ImageUrl", "Instructions", "OwnerId", "RecipeCategoryId", "Servings", "Title", "TotalTime" },
                values: new object[,]
                {
                    { new Guid("11112341-30e4-473f-b93a-d0352b978a84"), "Moussaka is beloved Balkan and Middle East dish. Its preparation depends on the region. In Bulgaria Moussaka is based on potatoes and ground meat. The meal is served warm and Bulgarians eat it very often simply because it’s super delicious and easy to cook. ", "https://www.supichka.com/files/images/1242/fit_1400_933.jpg", "Start with cooking the onion in a pan with 1/4 oil until golden brown. Then add the ground meat, the pepper, the paprika, and half the salt. Add the tomatoes and fry until they evaporate and the meat gets brown. Then remove the pan from the heat. Mix well with the potatoes and the other 1/2 tablespoon of salt. Add the mixture in a casserole pan with the rest of the oil. Bake in oven for about 40 minutes on 425 F (~220 C). In the meantime mix the yoghurt and the eggs separately and pour on top  of the meal for the last 10  minutes in the oven untill it turns brownish.", "72ed6dd1-7c97-4af7-ab79-fc72e4a53b16", 4, 8, "Moussaka", new TimeSpan(0, 2, 0, 0, 0) },
                    { new Guid("115e248e-3165-425d-aec6-5dda97c99be4"), "Indulge in a refreshing blend of creamy yogurt, ripe dates, nutrient-rich chia seeds, and succulent strawberries, creating a tantalizing fruity smoothie bursting with flavor and wholesome goodness. Perfect for a quick breakfast boost or a revitalizing snack any time of the day!", "https://www.proteincakery.com/wp-content/uploads/2023/11/strawberry-chia-collagen-smoothie.jpg", "Begin by soaking the chia seeds in water for about 10-15 minutes to allow them to gel up and soften.\r\nOnce the chia seeds have absorbed the water, place them along with the yogurt, pitted dates, and fresh strawberries into a blender.\r\nBlend all the ingredients on high speed until smooth and creamy, ensuring there are no chunks remaining.\r\nPour the smoothie into glasses and serve immediately for a delightful and nutritious treat. Enjoy your refreshing fruity smoothie!", "72ed6dd1-7c97-4af7-ab79-fc72e4a53b16", 1, 1, "Fruity strawberry smoothy", new TimeSpan(0, 0, 10, 0, 0) },
                    { new Guid("16541e8d-716c-45d9-8d6d-e3ae70d46c7b"), "Elevate your morning routine with this tasty Avocado Toast! Perfect start of the day for those busy mronings..", "https://cookingupmemories.com/wp-content/uploads/2021/01/avocado-toast-with-balsalmic-glaze-breakfast-768x1152.jpg.webp", "For a delicious twist, grill your slice of bread to your preference. Then, simply smash the avocado with a fork and spread it generously over the bread. Top it off with sliced cherry tomatoes, sprinkle with Himalayan salt and hemp seeds, and finally squeeze a little bit of lemon juice on top. Now, savor the flavors and enjoy your delightful avocado toast!", "72ed6dd1-7c97-4af7-ab79-fc72e4a53b16", 1, 1, "Avocado toast", new TimeSpan(0, 0, 10, 0, 0) },
                    { new Guid("25c6718c-b53b-4092-9454-d6999355f12d"), "Classical easy and delicious chicken soup to keep you warm in the cold winter days.", "https://i2.wp.com/www.downshiftology.com/wp-content/uploads/2023/10/Chicken-Soup-6.jpg", "Boil 2l of water. Add the chicken meat and some salt. Boil until ready or at leas for half an hour. Remove the chicken and portion it into small pieces. Take the remaining chicken broth back and again bring to a boil. Cut the vegetables into small pieces. First add the carrots and the onions to the boiling broth. After 5 minutes add the cut into small pieces potatoes. 5 minutes later also add the noodles. Finally add the portioned chicken to the soup. After boiling for another 5 minutes, add some finely cut celery. ", "72ed6dd1-7c97-4af7-ab79-fc72e4a53b16", 2, 6, "Homemade chicken soup", new TimeSpan(0, 1, 0, 0, 0) },
                    { new Guid("4a37318d-86fc-4411-a686-b01ae7e007c8"), "Savor the essence of a classic beef stew: tender beef, seared to perfection, nestled among hearty potatoes, sweet carrots, and crisp celery in a rich broth. Fragrant herbs and spices dance in each spoonful, invoking warmth and tradition. It's a comforting embrace on chilly nights, a symphony of flavors that transports you to cozy kitchens and cherished gatherings. With its melt-in-your-mouth beef and earthy vegetables, this stew is more than a meal—it's a timeless delight, a celebration of culinary craftsmanship and the simple joys of good food shared with loved ones.", "https://www.simplyrecipes.com/thmb/W8uC2OmR-C8WvHiURqfomkvnUnw=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/__opt__aboutcom__coeus__resources__content_migration__simply_recipes__uploads__2015__03__irish-beef-stew-vertical-a2-1800-8012236ba7e34c37abc3baedcab4aff7.jpg", "Add the onion, black pepper (beans), parsley, sunflower oil, salt and the beef to a pressure cooker. Fill with clean water to a level of 2 fingers above the products. Cook under pressure for about 40 minutes. Open the pressure cooker and strain the broth from the onion and black pepper beans. Portion the meat and remove the meat zip. Take back to a boil the portioned meat, the bone broth and add the largely cut into pieces carrots, celery root and potatoes. Bring the pressure cooker to a boil again and cook for another 20 minutes.\r\n", "72ed6dd1-7c97-4af7-ab79-fc72e4a53b16", 4, 4, "Beef stew", new TimeSpan(0, 1, 30, 0, 0) },
                    { new Guid("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"), "This versatile meal is not only simple to make, but feeds families big and small, making it a cheap and easy weeknight dinner legend.", "https://hips.hearstapps.com/hmg-prod/images/stuffed-peppers-lead-649c91e2c4e39.jpg", "1. Finely-chop the onion and carrots. Add to a pre-heated 3-4 tbsp of sunflower oil. Bake for a few minutes. 2. Add the minced meat while constantly mixing 3. Add the tomatoes and leave for the liquid to evaporate. Finally add the rice and the red pepper. Bake for another minute and remove from the stove 4. Add spices according to your taste - at least salt and black pepper (may add also allspice, cumin, etc.) 5. Stuff the peppers and put them in the oven with a little bit of salty water. Bake for 45mins on 180 degrees.", "72ed6dd1-7c97-4af7-ab79-fc72e4a53b16", 2, 8, "Stuffed red peppers with ground meat and rice", new TimeSpan(0, 2, 0, 0, 0) },
                    { new Guid("cd9be7fb-c016-4246-ac36-411f6c3ece14"), "Wake up to a simple breakfast solution with our delightful Overnight Oats. A harmonious blend of hearty oats, nutritious chia seeds, ripe banana, creamy milk (whether dairy or dairy-free), crunchy granola, and an assortment of vibrant fruits, all lovingly combined and left to mingle overnight for a deliciously convenient morning meal. Start your day right with this wholesome and customizable dish that promises to energize and satisfy with every spoonful.", "https://i0.wp.com/adiligentheart.com/wp-content/uploads/2023/01/image-31.png?w=1000&ssl=1", "Mix the banana and the milk of your choice in a high-speed blender and blend until smooth. Divide the rest of ingridients (half a cup rolled-oats, 1tbsp chia seeds and 2tsp sunflower seeds) and place in 2 bowls. Mix well and pour half of the blended milk with banana on top of each bowl. Store in a fridge during the night. The morning after top with granolla and fruits of your choice.", "72ed6dd1-7c97-4af7-ab79-fc72e4a53b16", 1, 2, "Overnight oats(the night beofre)", new TimeSpan(0, 0, 10, 0, 0) }
                });

            migrationBuilder.InsertData(
                table: "Specifications",
                columns: new[] { "Id", "Description" },
                values: new object[] { 10, "squeezed" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("72ed6dd1-7c97-4af7-ab79-fc72e4a53b16"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e8ec0c24-2dd1-4a7a-aefc-b54bc9a8e403"));

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 166);

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
                keyValue: 228);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 229);

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
                keyValue: 243);

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
                keyValue: 251);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 252);

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
                keyValue: 10);

            migrationBuilder.UpdateData(
                table: "IngredientCategories",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Flour and Baking Products");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 11,
                column: "Name",
                value: "Minced meat");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 14,
                column: "Name",
                value: "Yogurt");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 122,
                column: "Name",
                value: "Tomato(s)");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 123,
                column: "Name",
                value: "Cherry tomato(s)");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 167,
                column: "Name",
                value: "Diary free cream");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 168,
                columns: new[] { "IngredientCategoryId", "Name" },
                values: new object[] { 10, "Sesame oil" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 169,
                column: "Name",
                value: "Trout");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 170,
                column: "Name",
                value: "Salmon");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 171,
                column: "Name",
                value: "Tuna");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 172,
                column: "Name",
                value: "Sardines");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 173,
                column: "Name",
                value: "Mackerel");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 174,
                column: "Name",
                value: "Cod");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 175,
                column: "Name",
                value: "Mussels");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 176,
                column: "Name",
                value: "Skip jack");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 178,
                column: "Name",
                value: "Shark");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 179,
                column: "Name",
                value: "Silver catfish(");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 180,
                column: "Name",
                value: "Octopus");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 181,
                column: "Name",
                value: "Squids");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 182,
                column: "Name",
                value: "Hake fish");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 183,
                column: "Name",
                value: "Salmon trout");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 184,
                column: "Name",
                value: "Sprat");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 185,
                columns: new[] { "IngredientCategoryId", "Name" },
                values: new object[] { 11, "Sardines" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 186,
                column: "Name",
                value: "Lemon juice");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 187,
                column: "Name",
                value: "Tomatoe paste");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 188,
                columns: new[] { "IngredientCategoryId", "Name" },
                values: new object[] { 12, "Mustard" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 189,
                columns: new[] { "IngredientCategoryId", "Name" },
                values: new object[] { 5, "Savory" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 192,
                column: "Name",
                value: "Black rice");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 193,
                column: "Name",
                value: "Barley");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 194,
                column: "Name",
                value: "Bulgur");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 195,
                column: "Name",
                value: "Quinoa");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 196,
                column: "Name",
                value: "Amaranth");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 197,
                column: "Name",
                value: "Oats");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 198,
                columns: new[] { "IngredientCategoryId", "Name" },
                values: new object[] { 7, "Oatmeal" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 199,
                columns: new[] { "IngredientCategoryId", "Name" },
                values: new object[] { 13, "Chia seeds" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 200,
                column: "Name",
                value: "Wheat");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 201,
                column: "Name",
                value: "Corn");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 202,
                column: "Name",
                value: "Spelt");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 203,
                column: "Name",
                value: "Buckwheat");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 204,
                column: "Name",
                value: "Millet");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 205,
                columns: new[] { "IngredientCategoryId", "Name" },
                values: new object[] { 7, "Farro" });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 206,
                column: "Name",
                value: "Almonds");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 207,
                column: "Name",
                value: "Brazil nuts");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 208,
                column: "Name",
                value: "Cashew nuts");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 209,
                column: "Name",
                value: "Hazel nuts");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 210,
                column: "Name",
                value: "Macadamias");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 211,
                column: "Name",
                value: "Pecans");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 212,
                column: "Name",
                value: "Pistachios");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 213,
                column: "Name",
                value: "Walnuts");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 214,
                column: "Name",
                value: "Peanuts");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 215,
                column: "Name",
                value: "Pumpkin seeds");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 216,
                column: "Name",
                value: "Flax seeds");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 217,
                column: "Name",
                value: "Sesame seeds");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 218,
                column: "Name",
                value: "Poppy seeds");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 219,
                column: "Name",
                value: "Sunflower seeds");

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 222,
                columns: new[] { "IngredientCategoryId", "Name" },
                values: new object[] { 7, "Brown rice" });

            migrationBuilder.InsertData(
                table: "Ingredients",
                columns: new[] { "Id", "IngredientCategoryId", "Name" },
                values: new object[,]
                {
                    { 220, 13, "Psyllium seeds" },
                    { 221, 7, "Rice" }
                });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "Id", "Description", "ImageUrl", "Instructions", "OwnerId", "RecipeCategoryId", "Servings", "Title", "TotalTime" },
                values: new object[] { new Guid("377a3703-e719-42db-b013-19e5bd23892a"), "Мусака! Царицата на българската кухня! Едно от най-обичаните и най-често приготвяните ястия. Мусаката е абсолютният любимец както на всеки българин, така и на чужденците. Ако се притеснявате какво да сготвите на гостите от чужбина, можете да заложите на мусаката. Чуждестранните гости едва ли ще харесат таратора и едва ли ще съберат сили да пробват печените чушки с доматен сос, но мусаката задължително ще спечели доверието им, ще си оближат пръстите и ще искат порция допълнително. Рецептите за мусака нямат много вариации, вариантите са основно в приготвянето на заливката. Предложената тук рецепта за мусака е класическа, лесна и подходяща и за по-неопитните готвачи.", "https://www.supichka.com/files/images/1242/fit_1400_933.jpg", "Загрявате мазнината в дълбок тиган или тенджера. В загрятата мазнина задушете измитият, изчистен и нарязан на дребно лук, докато омекне. Добавяте нарязаните на дребно половината домати и каймата, черният пипер и чубрицата, както и една чаена лъжица сол. Може да ползвате кайма, каквато имате под ръка, но най-вкусната мусаката става с кайма смес – 50% свинско и 50 % телешко месо.\r\nСлед като водата от доматите изври добавяте червения пипер. Измивате, почиствате, обелвате и нарязвате на дребни кубчета картофите и ги прибавяте към месото. Намазнявате тавичка на дъното на която поставяте другата половина от доматите и върху тях изсипвате сместа за мусака. Добавяте една непълна чаена чаша гореща вода и поставяте в предварително загрята на 200 градуса фурна. Печете до докато водата изври, а картофите станат златисти.\r\nИдва моментът за приготвяне на заливката за мусаката. Тя става лесно и много бързо. Разбърквате киселото мляко и яйцата, след което при постоянно бъркане добавяте и брашното, до получаване на гладка смес. Накрая добавяте и настъргания на дребно кашкавал. Заливате мусаката и печете докато порозовее.", "92E86642-3F6F-4949-BACB-DE0EF010E36D", 4, 8, "Мусака", new TimeSpan(0, 2, 0, 0, 0) });

            migrationBuilder.InsertData(
                table: "RecipesIngredients",
                columns: new[] { "IngredientId", "RecipeId", "MeasureId", "Qty", "SpecificationId" },
                values: new object[,]
                {
                    { 11, new Guid("377a3703-e719-42db-b013-19e5bd23892a"), 5, 500m, null },
                    { 54, new Guid("377a3703-e719-42db-b013-19e5bd23892a"), 8, 1m, null },
                    { 113, new Guid("377a3703-e719-42db-b013-19e5bd23892a"), 1, 2m, null },
                    { 118, new Guid("377a3703-e719-42db-b013-19e5bd23892a"), 6, 1m, null },
                    { 122, new Guid("377a3703-e719-42db-b013-19e5bd23892a"), 5, 250m, null },
                    { 154, new Guid("377a3703-e719-42db-b013-19e5bd23892a"), 8, 3m, null }
                });
        }
    }
}
