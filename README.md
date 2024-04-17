# CookTheWeek App

CookTheWeek is an intuitive recipe application that helps you plan your meals for the entire week. With a vast collection of recipes and powerful features, CookTheWeek makes meal planning and cooking an enjoyable experience.

## Features

- **User Profiles**: Create personalized profiles to save preferences, add your recipes, and explore the full functionality.
- **Recipe Collection**: Access a diverse range of recipes for different types of meals (breakfast, salad, soup, main course, or dessert).
- **Customization**: Filter and order recipes based on category, ingredients, date created, or cooking time.
- **Cooking Instructions**: Detailed step-by-step instructions to guide you through the cooking process.
- **Add Recipes**: Add your favorite family recipes and include them in your Meal Plans.
- **Save Favorites**: Bookmark your favorite recipes for quick access later.
- **Meal Planner**: Plan your meals for the entire week by clicking a few buttons. Simply add recipes to your meal plan - this functionality is still in progress.
- **Ingredient Shopping List**: Automatically generate a shopping list based on your chosen recipes and servings - this functionality is still in progress.

## Installation

1. Clone the repository:
git clone https://github.com/yana-hris/cook-the-week.git

2. Navigate to the project directory

3. Install dependencies

4. Set up the database: update the connection strings in Program.cs of both CookTheWeek.Web & CookTheWeek.WebApi

5. Run migrations to create the database schema:
dotnet ef database update

6. Build the project

7. Run the project - Configure multiple StartUp Projects and run both CookTheWeek.Web & CookTheWeek.WebApi. The application should now be running locally. Two browsers must open - one for Swagger and one for the Web app.

## Usage

The Database is pre-seeded with 2 users. They can be used to log in and explore the app:

### Admin User
Username: adminUser
Password: admin1

### AppUser
Username: appUser
Password: 123456

Apart from that, you can register and log in with your account. The app does not require e-mail confirmation and works with a custom account, no external logins have been integrated so far.

APP USER
1. Sign up or log in to some of the pre-defined accounts.
2. Explore the recipe collection or use filters to find recipes that suit your preferences.
3. Add your recipes if you are a registered user. The ingredient tab allows suggestive search and makes it easy and fast (works only if the project is started together with the web API).
4. Like and unlike recipes that are not user-owned.
5. View your recipes, liked and added by you in the "Mine" tab.
6. NOT FINISHED YET: Build your meal plan by adding recipes, selecting serving sizes and cooking dates.
7. NOT FINISHED YET: Generate a shopping list and save it as a file or view it in-app.

ADMIN USER
1. The Admin User has custom navigation and custom pages
2. Can Add, Edit, Delete and View All:
   2.1 Recipes
   2.2 Ingredients
   2.3 Categories - both Recipe and Ingredient Category
   2.4 Can View Users
   2.5 NOT FINISHED - will be able to view all saved Meal Plans (finished and not-finished)

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


## Acknowledgements

- Fontawesome - Icons used in the application.
- Pexels.com - Images used in the application.

## Contact

If you have any questions, suggestions, or feedback, please don't hesitate to reach out:

- Email: yana.hristova.work@gmail.com

