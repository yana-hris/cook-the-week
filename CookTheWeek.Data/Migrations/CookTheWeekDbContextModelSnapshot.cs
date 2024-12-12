﻿// <auto-generated />
using System;
using CookTheWeek.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CookTheWeek.Data.Migrations
{
    [DbContext(typeof(CookTheWeekDbContext))]
    partial class CookTheWeekDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CookTheWeek.Data.Models.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", null, t =>
                        {
                            t.HasComment("The Application User");
                        });
                });

            modelBuilder.Entity("CookTheWeek.Data.Models.FavouriteRecipe", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("User Key Identifier");

                    b.Property<Guid>("RecipeId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Recipe Key Identifier");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false)
                        .HasComment("Soft Delete the Recipe Like when the Recipe is deleted");

                    b.HasKey("UserId", "RecipeId");

                    b.HasIndex("RecipeId");

                    b.ToTable("FavoriteRecipes", t =>
                        {
                            t.HasComment("Users` Favourite Recipes (likes)");
                        });
                });

            modelBuilder.Entity("CookTheWeek.Data.Models.Ingredient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Key Identifier");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int")
                        .HasComment("Ingredient Category Key Identifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasComment("Ingredient Name");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Ingredients", t =>
                        {
                            t.HasComment("Ingredient");
                        });
                });

            modelBuilder.Entity("CookTheWeek.Data.Models.IngredientCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Key identifier");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasComment("Ingredient Category Name");

                    b.HasKey("Id");

                    b.ToTable("IngredientCategories", t =>
                        {
                            t.HasComment("Ingredient Category");
                        });
                });

            modelBuilder.Entity("CookTheWeek.Data.Models.Meal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Meal Key Identifier");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CookDate")
                        .HasColumnType("datetime2")
                        .HasComment("Meal Cook Date");

                    b.Property<bool>("IsCooked")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false)
                        .HasComment("Meal completion Identifier");

                    b.Property<Guid>("MealPlanId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Meal Plan Key Identifier");

                    b.Property<Guid>("RecipeId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Recipe Key Identifier");

                    b.Property<int>("ServingSize")
                        .HasColumnType("int")
                        .HasComment("Meal Serving Size");

                    b.HasKey("Id");

                    b.HasIndex("MealPlanId");

                    b.HasIndex("RecipeId");

                    b.ToTable("Meals", t =>
                        {
                            t.HasComment("Meal");
                        });
                });

            modelBuilder.Entity("CookTheWeek.Data.Models.MealPlan", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Meal Plan Key Identifier");

                    b.Property<bool>("IsFinished")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false)
                        .HasComment("Meal Plan Completion Identifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasComment("Meal Plan Name");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Meal Plan Owner Key Identifier");

                    b.Property<DateTime>("StartDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()")
                        .HasComment("Meal Plan Start Date");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("MealPlans", t =>
                        {
                            t.HasComment("Meal Plan");
                        });
                });

            modelBuilder.Entity("CookTheWeek.Data.Models.Measure", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Key Identifier");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasComment("Measure Name");

                    b.HasKey("Id");

                    b.ToTable("Measures", t =>
                        {
                            t.HasComment("Measure of Recipe-Ingredient");
                        });
                });

            modelBuilder.Entity("CookTheWeek.Data.Models.Recipe", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Key Indetifier");

                    b.Property<double>("AverageRating")
                        .HasColumnType("float")
                        .HasComment("Recipe Average Rating calculated");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int")
                        .HasComment("Recipe Category Key Identifier");

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()")
                        .HasComment("Date and Time of a Recipe Creation");

                    b.Property<string>("Description")
                        .HasMaxLength(1500)
                        .HasColumnType("nvarchar(1500)")
                        .HasComment("Recipe Description");

                    b.Property<int?>("DifficultyLevel")
                        .HasColumnType("int")
                        .HasComment("Level of difficulty for the Recipe");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)")
                        .HasComment("Recipe Image Link");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false)
                        .HasComment("Soft Delete the Recipe");

                    b.Property<bool>("IsSiteRecipe")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false)
                        .HasComment("Indicator for Recipe Ownership");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Recipe Creator");

                    b.Property<int>("Servings")
                        .HasColumnType("int")
                        .HasComment("Recipe Serving Size");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasComment("Recipe Title");

                    b.Property<int>("TotalTimeMinutes")
                        .HasColumnType("int")
                        .HasComment("Recipe Cooking Time");

                    b.Property<int>("Views")
                        .HasColumnType("int")
                        .HasComment("Recipe Total Views");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Recipes", t =>
                        {
                            t.HasComment("Recipe");
                        });
                });

            modelBuilder.Entity("CookTheWeek.Data.Models.RecipeCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Key Identifier");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasComment("Recipe Category Name");

                    b.HasKey("Id");

                    b.ToTable("RecipeCategories", t =>
                        {
                            t.HasComment("Recipes Category");
                        });
                });

            modelBuilder.Entity("CookTheWeek.Data.Models.RecipeIngredient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Unique Recipe Ingredient Key identifier");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("IngredientId")
                        .HasColumnType("int")
                        .HasComment("Key Identifier for Ingredient");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false)
                        .HasComment("Soft Delete the RecipeIngredient when the Recipe is deleted");

                    b.Property<int>("MeasureId")
                        .HasColumnType("int")
                        .HasComment("Measure Key Identifier");

                    b.Property<decimal>("Qty")
                        .HasPrecision(18, 3)
                        .HasColumnType("decimal(10,3)")
                        .HasComment("Quantity of Ingredient in Recipe");

                    b.Property<Guid>("RecipeId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Key Identifier for Recipe");

                    b.Property<int?>("SpecificationId")
                        .HasColumnType("int")
                        .HasComment("Key identifier for Specification");

                    b.HasKey("Id");

                    b.HasIndex("IngredientId");

                    b.HasIndex("MeasureId");

                    b.HasIndex("SpecificationId");

                    b.HasIndex("RecipeId", "IngredientId", "MeasureId")
                        .IsUnique()
                        .HasFilter("[SpecificationId] IS NULL");

                    b.HasIndex("RecipeId", "IngredientId", "MeasureId", "SpecificationId")
                        .IsUnique()
                        .HasFilter("[SpecificationId] IS NOT NULL");

                    b.ToTable("RecipesIngredients", t =>
                        {
                            t.HasComment("Recipe Ingredient");
                        });
                });

            modelBuilder.Entity("CookTheWeek.Data.Models.RecipeRating", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("User Key Identifier");

                    b.Property<Guid>("RecipeId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Recipe Key Identifier");

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()")
                        .HasComment("Time of Rating Creation");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false)
                        .HasComment("Soft Delete the Rating when the Recipe is deleted");

                    b.Property<string>("RatingText")
                        .HasColumnType("nvarchar(max)")
                        .HasComment("User Comment for a given Recipe explaining Rativng Value");

                    b.Property<int>("RatingValue")
                        .HasColumnType("int")
                        .HasComment("User Rating for a given Recipe from 1 to 5");

                    b.HasKey("UserId", "RecipeId");

                    b.HasIndex("RecipeId");

                    b.ToTable("Ratings", t =>
                        {
                            t.HasComment("Recipe Rating by a given User");
                        });
                });

            modelBuilder.Entity("CookTheWeek.Data.Models.RecipeTag", b =>
                {
                    b.Property<Guid>("RecipeId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Recipe Key Identifier");

                    b.Property<int>("TagId")
                        .HasColumnType("int")
                        .HasComment("Tag Key Identifier");

                    b.HasKey("RecipeId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("RecipeTags", t =>
                        {
                            t.HasComment("Recipe`s Tags");
                        });
                });

            modelBuilder.Entity("CookTheWeek.Data.Models.Specification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Key Identifier");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasComment("Specification Description");

                    b.HasKey("Id");

                    b.ToTable("Specifications", t =>
                        {
                            t.HasComment("Specification of Recipe-Ingredient");
                        });
                });

            modelBuilder.Entity("CookTheWeek.Data.Models.Step", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Step Key Identifier");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)")
                        .HasComment("Cooking Step Instructions");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false)
                        .HasComment("Soft Delete the Step upon Recipe Deletion");

                    b.Property<Guid>("RecipeId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Recipe Key Identifier");

                    b.HasKey("Id");

                    b.HasIndex("RecipeId");

                    b.ToTable("Steps", t =>
                        {
                            t.HasComment("Cooking Step");
                        });
                });

            modelBuilder.Entity("CookTheWeek.Data.Models.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Key identifier");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)")
                        .HasComment("Meal Plan Name");

                    b.HasKey("Id");

                    b.ToTable("Tags", t =>
                        {
                            t.HasComment("Recipe Tag");
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("CookTheWeek.Data.Models.FavouriteRecipe", b =>
                {
                    b.HasOne("CookTheWeek.Data.Models.Recipe", "Recipe")
                        .WithMany("FavouriteRecipes")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CookTheWeek.Data.Models.ApplicationUser", "User")
                        .WithMany("FavoriteRecipes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recipe");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CookTheWeek.Data.Models.Ingredient", b =>
                {
                    b.HasOne("CookTheWeek.Data.Models.IngredientCategory", "Category")
                        .WithMany("Ingredients")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("CookTheWeek.Data.Models.Meal", b =>
                {
                    b.HasOne("CookTheWeek.Data.Models.MealPlan", "MealPlan")
                        .WithMany("Meals")
                        .HasForeignKey("MealPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CookTheWeek.Data.Models.Recipe", "Recipe")
                        .WithMany("Meals")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("MealPlan");

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("CookTheWeek.Data.Models.MealPlan", b =>
                {
                    b.HasOne("CookTheWeek.Data.Models.ApplicationUser", "Owner")
                        .WithMany("MealPlans")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("CookTheWeek.Data.Models.Recipe", b =>
                {
                    b.HasOne("CookTheWeek.Data.Models.RecipeCategory", "Category")
                        .WithMany("Recipes")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CookTheWeek.Data.Models.ApplicationUser", "Owner")
                        .WithMany("Recipes")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("CookTheWeek.Data.Models.RecipeIngredient", b =>
                {
                    b.HasOne("CookTheWeek.Data.Models.Ingredient", "Ingredient")
                        .WithMany("RecipesIngredients")
                        .HasForeignKey("IngredientId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CookTheWeek.Data.Models.Measure", "Measure")
                        .WithMany("RecipesIngredients")
                        .HasForeignKey("MeasureId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CookTheWeek.Data.Models.Recipe", "Recipe")
                        .WithMany("RecipesIngredients")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CookTheWeek.Data.Models.Specification", "Specification")
                        .WithMany("RecipesIngredients")
                        .HasForeignKey("SpecificationId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Ingredient");

                    b.Navigation("Measure");

                    b.Navigation("Recipe");

                    b.Navigation("Specification");
                });

            modelBuilder.Entity("CookTheWeek.Data.Models.RecipeRating", b =>
                {
                    b.HasOne("CookTheWeek.Data.Models.Recipe", "Recipe")
                        .WithMany("Ratings")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CookTheWeek.Data.Models.ApplicationUser", "User")
                        .WithMany("Ratings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Recipe");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CookTheWeek.Data.Models.RecipeTag", b =>
                {
                    b.HasOne("CookTheWeek.Data.Models.Recipe", "Recipe")
                        .WithMany("RecipeTags")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CookTheWeek.Data.Models.Tag", "Tag")
                        .WithMany("RecipeTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recipe");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("CookTheWeek.Data.Models.Step", b =>
                {
                    b.HasOne("CookTheWeek.Data.Models.Recipe", "Recipe")
                        .WithMany("Steps")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("CookTheWeek.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("CookTheWeek.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CookTheWeek.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("CookTheWeek.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CookTheWeek.Data.Models.ApplicationUser", b =>
                {
                    b.Navigation("FavoriteRecipes");

                    b.Navigation("MealPlans");

                    b.Navigation("Ratings");

                    b.Navigation("Recipes");
                });

            modelBuilder.Entity("CookTheWeek.Data.Models.Ingredient", b =>
                {
                    b.Navigation("RecipesIngredients");
                });

            modelBuilder.Entity("CookTheWeek.Data.Models.IngredientCategory", b =>
                {
                    b.Navigation("Ingredients");
                });

            modelBuilder.Entity("CookTheWeek.Data.Models.MealPlan", b =>
                {
                    b.Navigation("Meals");
                });

            modelBuilder.Entity("CookTheWeek.Data.Models.Measure", b =>
                {
                    b.Navigation("RecipesIngredients");
                });

            modelBuilder.Entity("CookTheWeek.Data.Models.Recipe", b =>
                {
                    b.Navigation("FavouriteRecipes");

                    b.Navigation("Meals");

                    b.Navigation("Ratings");

                    b.Navigation("RecipeTags");

                    b.Navigation("RecipesIngredients");

                    b.Navigation("Steps");
                });

            modelBuilder.Entity("CookTheWeek.Data.Models.RecipeCategory", b =>
                {
                    b.Navigation("Recipes");
                });

            modelBuilder.Entity("CookTheWeek.Data.Models.Specification", b =>
                {
                    b.Navigation("RecipesIngredients");
                });

            modelBuilder.Entity("CookTheWeek.Data.Models.Tag", b =>
                {
                    b.Navigation("RecipeTags");
                });
#pragma warning restore 612, 618
        }
    }
}
