﻿namespace CookTheWeek.Services.Data.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CookTheWeek.Data.Models;
    using CookTheWeek.Web.ViewModels.Meal;

    public interface IMealService
    {  

        /// <summary>
        /// Transforms a collection of MealAddFormModel into a collection of Meals and returns it
        /// </summary>
        /// <param name="meals"></param>
        /// <returns>A collection of Meal</returns>
        ICollection<Meal> CreateMealsAsync(ICollection<MealAddFormModel> meals);

       
        /// <summary>
        /// Gets the Meal (including all its ingredients) by a given id or throws an Exception
        /// </summary>
        /// <param name="mealId"></param>
        /// <returns>Meal</returns>
        /// <exception cref="RecordNotFoundException"></exception>
        Task<Meal> GetByIdAsync(int mealId);


        /// <summary>
        /// Returns the total count of all meals, cooked by a given recipeId
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns>int or 0</returns>
        Task<int?> GetAllMealsCountByRecipeIdAsync(Guid recipeId);
        
        /// <summary>
        /// Soft deletes all meals by a given recipe ID by settings their IsDeleted flag to true
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        Task SoftDeleteAllByRecipeIdAsync(Guid recipeId);

        /// <summary>
        /// Marks a meal as cooked
        /// </summary>
        /// <param name="mealId"></param>
        /// <returns></returns>
        Task TryMarkAsCooked(int mealId);
    }
}
