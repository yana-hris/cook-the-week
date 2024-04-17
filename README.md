# App Purpose

CookTheWeek is an intuitive recipe application that helps you plan your meals for the entire week. With a predefined collection of recipes and ingredients, CookTheWeek lets you search and filter by Category, ingredient, or just Cooking Time. It allows you to add your favorite recipes, and include them in your Meal Plans. The app will remember your intended recipes between browsing sessions and will also give you access and renewal options for your past fulfilled Meal Plans.

## App Limitations
- **User Added Ingredients**: Ordinary Users cannot add their ingredients in case they want to add a Recipe, and an ingredient is missing. The ingredients are predefined and can be added, edited, or deleted solely by an admin user.  
- **Recipe Ingredient Measures and Specifics**: Ordinary Users cannot add or edit the pre-defined recipe-ingredient measures and specifications. They are initially seeded in the database.  
- **User Likes**: Users cannot like their User Added Recipes, they can only like the App`s Seeded (or Admin Added) Recipes or other Users' Added Recipes. If a Recipe is deleted, it will not appear in the user's collection of liked Recipes.  
- **Deleting Recipes**: Recipes, included in current Meal Plans cannot be deleted. Users can delete only their own Recipes in case they are not currently included in a Meal Plan.  
- **Meal Plan**: Users can only have one unsaved/still-in-progress Meal Plan (stored in the Local Storage). Upon Building the Meal Plan, it will be available for Review in the tab "Meal Plans".  

## App Features

- **User Profiles**: Create personalized profiles to save preferences, add your recipes, and explore the full functionality. 
- **Recipe Collection**: Access a diverse range of recipes for different types of meals (breakfast, salad, soup, main course, or dessert).
- **Customization**: Filter and order recipes based on category, ingredients, date created, or cooking time. 
- **Suggestive Search for Ingredients Addition**: Start typing the ingredient name and the suggestions will start popping after the third character:  
  ![image](https://github.com/yana-hris/cook-the-week/assets/8995553/999e2d3f-238e-4553-b21f-f4db8ed15aa4)


  
  
- **Add Recipes**: Add your favorite family recipes and include them in your future Meal Plans:
![image](https://github.com/yana-hris/cook-the-week/assets/8995553/23cdc1e1-3e83-459a-af29-04a91080b41b)




- **Save Favorites**: Bookmark your favorite recipes for quick access later and access them in your "Mine" collection:
  ![image](https://github.com/yana-hris/cook-the-week/assets/8995553/9d1bfd81-8312-4526-81a3-78c183e16e30)


  
  
- **Add Recipes to Meal Plan** by clicking the "+" button and save them for the future Meal Plan Build
![image](https://github.com/yana-hris/cook-the-week/assets/8995553/9710cc0c-7eb7-4ba6-9982-029bfc0cfcbb)

- **Change Theme** by clicking the button in the right upper corner:
  
  ![image](https://github.com/yana-hris/cook-the-week/assets/8995553/b39816bf-9de9-401a-a13b-f9d8652ff2f7)

  ![image](https://github.com/yana-hris/cook-the-week/assets/8995553/26be14c6-18df-40e5-8ac9-cad24c639094)

  ![image](https://github.com/yana-hris/cook-the-week/assets/8995553/83b80015-716b-41dd-b000-6afe730ccc16)



- FUNCTIONALITY IN PROGRESS: **Customize Meals** according to your needs by adjusting the required serving of every Meal in your Meal Plan. The Recipe Ingredients will be automatically adjusted to the required needs.
- FUNCTIONALITY IN PROGRESS: **Plan Days for Cooking** by selecting them from the drop-down menu in your Meal plan.
- FUNCTIONALITY IN PROGRESS: **Meal Planner**: Plan your meals for the entire week by clicking a few buttons.
- FUNCTIONALITY IN PROGRESS: **Ingredient Shopping List**: Automatically generate a shopping list based on your chosen recipes and servings.
- **Admin Area**: The Admin User can access the admin panel where he can monitor, add, and edit the main app entities - Ingredient, Recipe Category, Ingredient Category. He can also edit all Recipes and see the User statistics:
- 
![image](https://github.com/yana-hris/cook-the-week/assets/8995553/f855d808-2c42-4c48-9782-c73cd23fa190)

![image](https://github.com/yana-hris/cook-the-week/assets/8995553/c20c7353-032a-4074-b519-7c3b9b123151)  

![image](https://github.com/yana-hris/cook-the-week/assets/8995553/bf44d8f3-ff66-4929-ba44-fa40a9a17bbf)  

![image](https://github.com/yana-hris/cook-the-week/assets/8995553/4e772a09-b4c3-4198-a899-ff2796ebd775)  

![image](https://github.com/yana-hris/cook-the-week/assets/8995553/7df0c93b-7fc0-4492-bea4-76957e0cd431)  


---



## Installation

1. **Clone** the repository:  
> git clone https://github.com/yana-hris/cook-the-week.git

2. **Navigate** to the project directory  

3. **Install** ***dependencies*** 

4. **Set up the database**: ***update*** the ***connection strings*** in ***Program.cs*** of both **CookTheWeek.Web** & **CookTheWeek.WebApi** 

5. **Run migrations** to create the database schema:  
> dotnet ef database update

6. **Build** the project

7. **Run** the project - Configure multiple StartUp Projects and *run both* **CookTheWeek.Web** & **CookTheWeek.WebApi**. The application should now be running locally. Two browsers must open - one for Swagger and one for the Web app.

## Usage

The Database is pre-seeded with 2 users. They can be used to log in and explore the app:

### Admin User  
> Username: adminUser  
> Password: admin1  

### AppUser  
> Username: appUser  
> Password: 123456  

Apart from that, you can register and log in with your account. The app does not require e-mail confirmation and works with a custom account, no external logins have been integrated so far.

APP USER  
1. Sign up or log in to some of the pre-defined accounts.  
2. Explore the recipe collection or use filters to find recipes that suit your preferences.  
3. Add your recipes if you are a registered user. The ingredient tab allows suggestive search and makes it easy and fast (works only if the project is started together with the web API).  
4. Like and unlike recipes that are not user-owned.  
5. View your recipes, liked and added by you in the "Mine" tab.  
6. NOT FINISHED YET: Build your meal plan by adding recipes, and selecting serving sizes and cooking dates.  
7. NOT FINISHED YET: Generate a shopping list and save it as a file or view it in-app.  

ADMIN USER  
- The Admin User has custom navigation and custom pages
- Can Add, Edit, Delete and View All:
   - Recipes
   - Ingredients
   - Categories - both Recipe and Ingredient Category
   - Can View Users and statistics
   - IN PROGRESS - view all users` saved Meal Plans

## Database Diagram:
![image](https://github.com/yana-hris/cook-the-week/assets/8995553/7aba1633-3c52-4146-8b0b-84108ae54c2a)

## Used technologies
ASP.NET Core MVC
ASP.NET Web API
Entity Framework Core (EF Core)
SQL Server
jQuery
AJAX
JavaScript
Bootstrap
SCSS (SASS)
Local Storage

## Acknowledgements

- Fontawesome - Icons used in the application.
- Pexels.com - Images used in the application.

## Contact

If you have any questions, suggestions, or feedback, please don't hesitate to reach out:

- Email: yana.hristova.work@gmail.com

