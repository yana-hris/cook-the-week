# App Purpose

CookTheWeek is an intuitive recipe application that helps you plan your meals for the entire week. With a predefined collection of Recipes and Ingredients, CookTheWeek lets you search and filter by Category, Ingredient, or just Cooking Time. It allows you to add your favorite recipes, and include them in your Meal Plans. The app will remember your intended recipes between browsing sessions and devices and will also give you access and renewal options for your past fulfilled Meal Plans.

## App Limitations
- **User Added Ingredients**: Ordinary Users cannot add their Ingredients in case they want to add a Recipe, and an Ingredient is missing. The Ingredients are predefined and can be added, edited, or deleted solely by an Admin User.  
- **Recipe Ingredient Measures and Specifics**: Ordinary Users cannot add or edit the pre-defined Recipe-Ingredient Measures and Specifications. They are initially seeded in the database.  
- **User Likes**: Users cannot like their User Added Recipes, they can only like the App`s Seeded (Admin Added) Recipes or other Users' Added Recipes. If a Recipe is deleted by its owner, it will not appear in the user's collection of liked Recipes.  
- **Deleting Recipes**: Recipes, included in current Meal Plans cannot be deleted. Users can delete only their own Recipes in case they are not currently included in a Meal Plan.  
- **Meal Plan**: Users can only have one unsaved/still-in-progress Meal Plan (stored in the Local Storage). Upon Building the Meal Plan, it will be available for Review in the tab "Meal Plans".  

## App Features

- **User Profiles**: Create personalized profiles to save preferences, add your recipes, and explore the full functionality. 
- **Recipe Collection**: Access a diverse range of recipes for different types of meals (breakfast, salad, soup, main course, or dessert).
- **Add Recipes**: Add your favorite family recipes and include them in your future Meal Plans:
<img src="https://github.com/yana-hris/cook-the-week/assets/8995553/23cdc1e1-3e83-459a-af29-04a91080b41b" width="900">  

- **Save Favorites**: Bookmark your favorite recipes for quick access later and access them in your "Mine" collection:
  
<img src="https://github.com/yana-hris/cook-the-week/assets/8995553/9d1bfd81-8312-4526-81a3-78c183e16e30" width="900">  

- **Suggestive Search for Ingredients Addition**: Start typing the ingredient name and the suggestions will start popping after the third character:  

<img src="https://github.com/yana-hris/cook-the-week/assets/8995553/999e2d3f-238e-4553-b21f-f4db8ed15aa4" width="900">    


- **Customization**: Filter and order recipes based on category, ingredients, date created, or cooking time. 
  
  
- **Add Recipes to Meal Plan** by clicking the "+" button and save them for the future Meal Plan Build

<img src="https://github.com/yana-hris/cook-the-week/assets/8995553/9710cc0c-7eb7-4ba6-9982-029bfc0cfcbb" width="900">  



- **Change Theme** by clicking the button in the right upper corner:  

<img src="https://github.com/yana-hris/cook-the-week/assets/8995553/b39816bf-9de9-401a-a13b-f9d8652ff2f7" width="900">

<img src="https://github.com/yana-hris/cook-the-week/assets/8995553/26be14c6-18df-40e5-8ac9-cad24c639094" width="900">

<img src="https://github.com/yana-hris/cook-the-week/assets/8995553/83b80015-716b-41dd-b000-6afe730ccc16" width="900">  

### App User features:
- Can Sign up or Log in to his/her User Account.    
- Browse the Recipe collection, use filters and sorting to find Recipes by Name, Ingredient, Cooking Time, etc.     
- Add his/her Recipes /only for registered and logged users/. The add ingredient tab allows suggestive search and makes it easy and fast /to explore this functionality, both **CookTheWeek.Web** and **CookTheWeek.WebApi** must be started as *Multiple Startup Projects*).    
- Like and unlike recipes by other users.    
- View his/her Recipes in the "Mine" tab - both user-added and user-liked.  
- FUNCTIONALITY IN DEVELOPMENT: **Meal Planner**: Build a Meal Plan by adding Recipes, selecting Serving Sizes, and Cooking Dates (optional).
- FUNCTIONALITY IN PROGRESS: **Customize Meals**: According to your needs by adjusting the required serving of every Meal in your Meal Plan. The Recipe Ingredients will be automatically adjusted to the required needs.
- FUNCTIONALITY IN PROGRESS: **Plan Days for Cooking**: By selecting them from the drop-down menu in your Meal Plan.
- FUNCTIONALITY IN DEVELOPMENT: **Ingredient Shopping List**: Generate a shopping list based on your chosen recipes and servings, and save it as a file or view it in-app.



### Admin Area features:  
- Has custom navigation and custom pages.  
- Can Add, Edit, Delete, and View All:
   - Recipes  
   - Ingredients  
   - Categories - both Recipe and Ingredient Categories    
   - View Users and Statistics  
   - FUNCTIONALITY IN DEVELOPMENT - view all users` saved Meal Plans   

<img src="https://github.com/yana-hris/cook-the-week/assets/8995553/f855d808-2c42-4c48-9782-c73cd23fa190" width="900">

<img src="https://github.com/yana-hris/cook-the-week/assets/8995553/c20c7353-032a-4074-b519-7c3b9b123151" width="900">   

<img src="https://github.com/yana-hris/cook-the-week/assets/8995553/bf44d8f3-ff66-4929-ba44-fa40a9a17bbf" width="900">  

<img src="https://github.com/yana-hris/cook-the-week/assets/8995553/4e772a09-b4c3-4198-a899-ff2796ebd775" width="900">  

<img src="https://github.com/yana-hris/cook-the-week/assets/8995553/7df0c93b-7fc0-4492-bea4-76957e0cd431" width="900">   

---



## Installation

1. **Clone** the repository:  
```bash
git clone https://github.com/yana-hris/cook-the-week.git
```

2. **Navigate** to the project directory  

3. **Install** dependencies

4. **Set up the database**: update the connection strings in **Program.cs** of both ***CookTheWeek.Web*** & ***CookTheWeek.WebApi***

5. **Run migrations** to create the database schema:  
```bash
dotnet ef database update
```
6. **Build** the project

7. **Run** the project - Configure multiple StartUp Projects and *run both* **CookTheWeek.Web** & **CookTheWeek.WebApi**. The application should now be running locally. Two browsers must open - one for Swagger and one for the Web app.

## Usage

The Database is pre-seeded with 2 users. They can be used to log in and explore the app:

### Admin User  
> Username:
```bash
adminUser
```
> Password:
```bash
admin1
```

### AppUser  
> Username:
```bash
appUser
```
> Password:
```bash
123456  
```
Apart from that, you can register and log in with your account. The app does not require e-mail confirmation and works with a custom account, no external logins have been integrated so far.

 ### Recipes
 The database is pre-seeded with several Recipes. You can try and add, edit and delete your own Recipes.

 ### Ingredients
 Admin User can add Ingredients, edit and delete them.

 ### Categories
 Admin User can Add, Remove and Edit Recipe Categroies and Ingredient Categories.

 ### Users
 Admin User can see all Users and also their statistics.



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

