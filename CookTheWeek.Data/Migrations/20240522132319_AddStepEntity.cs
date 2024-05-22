using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CookTheWeek.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStepEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Instructions",
                table: "Recipes");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Recipes",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Soft Delete the Recipe",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Soft Delete for a Recipe");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Recipes",
                type: "nvarchar(1500)",
                maxLength: 1500,
                nullable: true,
                comment: "Recipe Description",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true,
                oldComment: "Recipe Description");

            migrationBuilder.AddColumn<bool>(
                name: "IsFinished",
                table: "MealPlans",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Meal Plan Completion Identifier");

            migrationBuilder.CreateTable(
                name: "Steps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Step Key Identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Recipe Key Identifier"),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false, comment: "Cooking Step Instructions")
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

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("72ed6dd1-7c97-4af7-ab79-fc72e4a53b16"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "58114e28-6ac6-47b7-9a7c-d20a6608b246", "AQAAAAIAAYagAAAAED/m6BheGfVujxJjJKVpISJEWTUSmhvmwM9RY40L/lcrbaY7/lBVnRegjJXCx6c3NQ==", "ad8530f0-eee2-4856-8b43-0bd7c40f57e8" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e8ec0c24-2dd1-4a7a-aefc-b54bc9a8e403"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b0b44f29-7e59-4804-b1cf-78218a1ac913", "AQAAAAIAAYagAAAAEJInoC+PIZSURu564kAAPRABXsehpwWSo2z1pN0sS6d1LuLivR+oDC9OrJOU9v776g==", "131fc6fe-d436-4791-8d2f-e6228c5d315d" });

            migrationBuilder.InsertData(
                table: "Ingredients",
                columns: new[] { "Id", "CategoryId", "Name" },
                values: new object[,]
                {
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
                    { 47, 4, "Almond Flour" }
                });

            migrationBuilder.UpdateData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "pc");

            migrationBuilder.UpdateData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "clove");

            migrationBuilder.UpdateData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 7,
                column: "Name",
                value: "tsp");

            migrationBuilder.UpdateData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 8,
                column: "Name",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 9,
                column: "Name",
                value: "cup");

            migrationBuilder.UpdateData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 10,
                column: "Name",
                value: "bunch");

            migrationBuilder.UpdateData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 11,
                column: "Name",
                value: "pkg");

            migrationBuilder.UpdateData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 12,
                column: "Name",
                value: "slice");

            migrationBuilder.UpdateData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 13,
                column: "Name",
                value: "pinch");

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

            migrationBuilder.CreateIndex(
                name: "IX_Steps_RecipeId",
                table: "Steps",
                column: "RecipeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Steps");

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
                keyValue: 36);

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

            migrationBuilder.DropColumn(
                name: "IsFinished",
                table: "MealPlans");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Recipes",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Soft Delete for a Recipe",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Soft Delete the Recipe");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Recipes",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                comment: "Recipe Description",
                oldClrType: typeof(string),
                oldType: "nvarchar(1500)",
                oldMaxLength: 1500,
                oldNullable: true,
                oldComment: "Recipe Description");

            migrationBuilder.AddColumn<string>(
                name: "Instructions",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "Recipe Instructions");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("72ed6dd1-7c97-4af7-ab79-fc72e4a53b16"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5d95e38d-f8ae-421e-95ef-3dc20a821d17", "AQAAAAIAAYagAAAAEGoPNUL34gVW6jeoUqKS+H9EHJeYBruombJx6x7j7+We/k182ENqQII5ps+KKxSAig==", "c3bb3f5e-6f8f-4c52-a0de-7a48063850f9" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e8ec0c24-2dd1-4a7a-aefc-b54bc9a8e403"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1fc50751-c91f-498d-bdc9-f9ced66bbdc9", "AQAAAAIAAYagAAAAECzaaFIGB1apOrHblQxsrdTpnjw8XDFLvY2Ki5qu7ixQvHk3Mv41C9Bsrd++2rSTgg==", "b6dccf8e-1ae1-4823-81f4-42377f73d67c" });

            migrationBuilder.UpdateData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "pc/s");

            migrationBuilder.UpdateData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "clove/s");

            migrationBuilder.UpdateData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 7,
                column: "Name",
                value: "tsp/s");

            migrationBuilder.UpdateData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 8,
                column: "Name",
                value: "tbsp/s");

            migrationBuilder.UpdateData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 9,
                column: "Name",
                value: "cup/s");

            migrationBuilder.UpdateData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 10,
                column: "Name",
                value: "bunch/es");

            migrationBuilder.UpdateData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 11,
                column: "Name",
                value: "pkg/s");

            migrationBuilder.UpdateData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 12,
                column: "Name",
                value: "slice/s");

            migrationBuilder.UpdateData(
                table: "Measures",
                keyColumn: "Id",
                keyValue: 13,
                column: "Name",
                value: "pinch/es");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("11112341-30e4-473f-b93a-d0352b978a84"),
                column: "Instructions",
                value: "Start with cooking the onion in a pan with 1/4 oil until golden brown. Then add the ground meat, the pepper, the paprika, and half the salt. Add the tomatoes and fry until they evaporate and the meat gets brown. Then remove the pan from the heat. Mix well with the potatoes and the other 1/2 tablespoon of salt. Add the mixture in a casserole pan with the rest of the oil. Bake in oven for about 40 minutes on 425 F (~220 C). In the meantime mix the yoghurt and the eggs separately and pour on top  of the meal for the last 10  minutes in the oven untill it turns brownish.");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("115e248e-3165-425d-aec6-5dda97c99be4"),
                column: "Instructions",
                value: "Begin by soaking the chia seeds in water for about 10-15 minutes to allow them to gel up and soften.\r\nOnce the chia seeds have absorbed the water, place them along with the yogurt, pitted dates, and fresh strawberries into a blender.\r\nBlend all the ingredients on high speed until smooth and creamy, ensuring there are no chunks remaining.\r\nPour the smoothie into glasses and serve immediately for a delightful and nutritious treat. Enjoy your refreshing fruity smoothie!");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("16541e8d-716c-45d9-8d6d-e3ae70d46c7b"),
                column: "Instructions",
                value: "For a delicious twist, grill your slice of bread to your preference. Then, simply smash the avocado with a fork and spread it generously over the bread. Top it off with sliced cherry tomatoes, sprinkle with Himalayan salt and hemp seeds, and finally squeeze a little bit of lemon juice on top. Now, savor the flavors and enjoy your delightful avocado toast!");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("25c6718c-b53b-4092-9454-d6999355f12d"),
                column: "Instructions",
                value: "Boil 2l of water. Add the chicken meat and some salt. Boil until ready or at leas for half an hour. Remove the chicken and portion it into small pieces. Take the remaining chicken broth back and again bring to a boil. Cut the vegetables into small pieces. First add the carrots and the onions to the boiling broth. After 5 minutes add the cut into small pieces potatoes. 5 minutes later also add the noodles. Finally add the portioned chicken to the soup. After boiling for another 5 minutes, add some finely cut celery. ");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("27664df3-cb8d-4ff6-a2cf-da0745a17531"),
                column: "Instructions",
                value: "Prepare the Beans: Soak the beans overnight or for at least 6-8 hours. Rinse them thoroughly and place them in a pressure cooker without covering. As soon as the beans begin to foam, rinse them with cold water in the sink, then add fresh water to the pot and bring it to a boil.\r\n\r\nPrepare the Vegetables: While the beans are cooking, chop the onions, carrots, and peppers into appropriate pieces. Place them in the pot with 2-3 tablespoons of sunflower oil. Add paprika and other desired spices, except for salt, at this stage. Do not add salt until later.\r\n\r\nPressure Cook: Close the pressure cooker and cook everything for about 40 minutes. Once done, remove from heat.\r\n\r\nCheck the Beans: When it's safe to open the pressure cooker, check if the beans are fully cooked. If they are, add the grated or blended tomatoes and salt.\r\n\r\nFinal Cooking: Boil for an additional 10 minutes, then reduce heat to low and simmer until ready. For enhanced flavor, allow the stew to sit with the lid on for at least a few hours.\r\n\r\nServe: When ready to serve, sprinkle finely chopped fresh parsley on top for a burst of freshness. Enjoy your delicious bean stew!");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("294c6abe-0072-427e-a1e8-355ba414fa5b"),
                column: "Instructions",
                value: "To cook Thai pumpkin soup, start by sautéing aromatics like onions, (garlic, optional), ginger, and (lemongrass, optional) in a pot until fragrant. Add diced pumpkin (or canned pumpkin puree), coconut milk, vegetable broth, and Thai curry paste. Simmer until the pumpkin is tender. Then, blend the soup until smooth using an immersion blender or countertop blender. Adjust seasoning with salt, pepper, and lime juice to taste. Serve hot, garnished with fresh cilantro, a swirl of coconut milk, and a sprinkle of chili flakes for extra heat, if desired.");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("4a37318d-86fc-4411-a686-b01ae7e007c8"),
                column: "Instructions",
                value: "Add the onion, black pepper (beans), parsley, sunflower oil, salt and the beef to a pressure cooker. Fill with clean water to a level of 2 fingers above the products. Cook under pressure for about 40 minutes. Open the pressure cooker and strain the broth from the onion and black pepper beans. Portion the meat and remove the meat zip. Take back to a boil the portioned meat, the bone broth and add the largely cut into pieces carrots, celery root and potatoes. Bring the pressure cooker to a boil again and cook for another 20 minutes.\r\n");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("9dbc2359-a2c2-49c8-ae84-cd6d6aad9bcb"),
                column: "Instructions",
                value: "1. Finely-chop the onion and carrots. Add to a pre-heated 3-4 tbsp of sunflower oil. Bake for a few minutes. 2. Add the minced meat while constantly mixing 3. Add the tomatoes and leave for the liquid to evaporate. Finally add the rice and the red pepper. Bake for another minute and remove from the stove 4. Add spices according to your taste - at least salt and black pepper (may add also allspice, cumin, etc.) 5. Stuff the peppers and put them in the oven with a little bit of salty water. Bake for 45mins on 180 degrees.");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("cd9be7fb-c016-4246-ac36-411f6c3ece14"),
                column: "Instructions",
                value: "Mix the banana and the milk of your choice in a high-speed blender and blend until smooth. Divide the rest of ingridients (half a cup rolled-oats, 1tbsp chia seeds and 2tsp sunflower seeds) and place in 2 bowls. Mix well and pour half of the blended milk with banana on top of each bowl. Store in a fridge during the night. The morning after top with granolla and fruits of your choice.");
        }
    }
}
