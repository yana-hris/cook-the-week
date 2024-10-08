namespace CookTheWeek.Services.Data.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CookTheWeek.Data.Models;
    using CookTheWeek.Web.ViewModels.Meal;

    public interface IMealService
    {  

        /// <summary>
        /// Transforms a collection of MealAddFormModel into a collection of Meals and adds them to the database
        /// </summary>
        /// <param name="meals"></param>
        /// <returns></returns>
        Task AddAllAsync(ICollection<MealAddFormModel> meals);

       
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
        Task<int?> GetAllMealsCountByRecipeIdAsync(string recipeId);

        
        /// <summary>
        /// Deletes all meals, included in a specific meal plan (by id)
        /// </summary>
        /// <param name="id">Meal Plan Id</param>
        /// <returns></returns>
        Task HardDeleteAllByMealPlanIdAsync(string mealplanId);


        /// <summary>
        /// Soft deletes all meals by a given recipe ID by settings their IsDeleted flag to true
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        Task SoftDeleteAllByRecipeIdAsync(string recipeId);


        /// <summary>
        /// Deletes all meals, cooked by a specific recipe Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task HardDeleteAllByRecipeIdAsync(string recipeId);

    }
}
