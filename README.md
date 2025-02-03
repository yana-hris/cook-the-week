# 🍽️ Cook The Week

Plan Your Weekly Meals with Ease!

## 📖 About The Project

Cook The Week is a **.NET 8-based web application** designed to help users **plan, organize, and manage weekly meals**. The application features **recipe management, meal planning, and automatic shopping list generation**.

This app runs on **ASP.NET Core**, uses **Entity Framework Core for database management**, and utilizes **Hangfire for background job scheduling**. The database is **automatically created and seeded on first launch**—no manual migrations needed!

---

## 🛠️ Features

- ✅ **User-Added & Site Recipes** – Browse, search, and manage recipes.
- ✅ **Secure Authentication** – Users can sign up manually or use external logins (Google or Facebook).
- ✅ **Editable & Reusable Meal Plans** – Users can modify and reuse meal plans as needed.
- ✅ **Shopping Lists** – Easily generate downloadable or email-ready shopping lists with a single click.
- ✅ **Admin Dashboard** – Manage all recipes, categories, and user contributions.
- ✅ **Background Jobs (Hangfire)** – Automated meal plan claims updates and cleanup.

---

## 📸 Screenshots

### 🏠 Home Page – Welcome message and call-to-action buttons.

![Home Page](image.png)

### ℹ️ How It Works – A step-by-step guide on using the app effectively.

![How It Works](image-8.png)

### 🔎 Browse Page – Featured recipes, filters, and pagination.

![Browse Recipes Page](image-2.png)

### 📜 Recipe Details Page – Ingredients, instructions, and description.

![Recipe Details](image-3.png)

### 🗓️ Add Meal Plan Page – Create, edit, and reuse meal plans from selected recipes.

![Add MealPlan Page](image-4.png)

### 🛒 Shopping List – Download or email your grocery list in one click.

![Shopping List Page](image-5.png)

### 🔧 Admin Panel – Manage and oversee recipes, categories, and user contributions. Add/Edit/Delete Ingredients, Categories, etc.

![Admin Panel](image-6.png)

### 👤 User Dashboard – Track saved recipes and meal plans.

![User Profile](image-7.png)

---

## 🛠️ Tech Stack

### 🖥️ Frontend

- **SASS (SCSS)** – Advanced styling with variables, mixins, and modular CSS.
- **Bootstrap** – Responsive and mobile-friendly design.
- **jQuery & JavaScript** – Interactive UI components and dynamic UI bindings.
- **KnockoutJS** – Client-side validation and dynamic UI bindings.

### ⚙️ Backend

- **ASP.NET Core (.NET 8)** – The main backend framework.
- **Entity Framework Core** – Database management with SQL Server.
- **Hangfire** – Background job scheduling and task automation.

### 📂 Database & Storage

- **SQL Server** – Relational database for storing recipes, users, and meal plans.
- **Cloudinary** – Image processing and storage for recipe images.

### 📧 Email & Reports

- **SendGrid** – Emailing service for sending shopping lists and notifications.
- **Rotativa** – PDF generation for downloading meal plans and shopping lists.

### 🔑 Authentication & External Logins

- **ASP.NET Identity** – Secure user authentication and account management.
- **External Logins** – Support for Google, Facebook, and more.

---

## 🚀 Getting Started

### 🔧 Prerequisites

Before running the application, ensure you have the following installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Local or Azure SQL)
- [Azure Account (Optional)](https://azure.microsoft.com/en-us/free/)

---

### 💻 Installation & Setup

#### 1️⃣ Clone the Repository

```bash
git clone https://github.com/yana-hris/cook-the-week.git
cd cook-the-week
```

#### 🔧 Configure the Database Connection

Open `appsettings.json` and update the `ConnectionStrings` section with your database details:
apsettings.json
----
````json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your_server;Database=your_db;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

#### 3️⃣ Run the Application
```bash
dotnet run
```

## 4️⃣ Access the App in Your Browser

```bash
http://localhost:7170
```

### 👤 Admin User Credentials

> Username:

```bash
adminUser
```

> Password:

```bash
admin1
```

### 👤 App User Credentials

> Username:

```bash
appUser
```

> Password:

```bash
123456
```

## Database Diagram:

![image](image-9.png)

## Acknowledgements

- Fontawesome - Icons used in the application.
- Pexels.com - Images used in the application.

## Contact

For any feedback, don't hesitate to reach out:

- Email: yana.hristova.work@gmail.com






