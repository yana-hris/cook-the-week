﻿namespace CookTheWeek.Data.Repositories
{
    using CookTheWeek.Data.Models;

    public interface IRecipeIngredientRepository
    {
        /// <summary>
        /// Gets a queriable collection of tracked recipe ingredients 
        /// which can be changed and persisted in the database when calling SaveChangesAsync() method
        /// </summary>
        /// <returns></returns>
        IQueryable<RecipeIngredient> GetAllTrackedQuery();

        
        /// <summary>
        /// Persists changes in the database when using non-async Recipe Ingredient methods beforehand or if changing any tracked entities
        /// </summary>
        /// <returns></returns>
        Task SaveChangesAsync();


        // FOR NESTED ENTITIES IN RECIPE INGREDIETS:
        // MEASURES
        /// <summary>
        /// Returns a queriable of all measures
        /// </summary>
        /// <returns>A qieryable collection of Measure</returns>
        IQueryable<Measure> GetAllMeasuresQuery();

        /// <summary>
        /// Checks if a given recipe ingredient Measure exists by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true or false</returns>
        Task<bool> MeasureExistsByIdAsync(int id);

        /// <summary>
        /// Checks if a given recipe ingredient Measure exists by name
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true or false</returns>
        Task<bool> MeasureExistsByNameAsync(string name);

        /// <summary>
        /// Adds a Measure to the database
        /// </summary>
        /// <param name="measure"></param>
        /// <returns></returns>
        Task AddMeasureAsync(Measure measure); 

        /// <summary>
        /// Updates an existing Measure
        /// </summary>
        /// <param name="measure"></param>
        /// <returns></returns>
        Task UpdateMeasureAsync(Measure measure);

        /// <summary>
        /// Deletes an existing Measure from the database
        /// </summary>
        /// <param name="measure"></param>
        /// <returns></returns>
        Task DeleteMeasureAsync(Measure measure);

        // SPECIFICATIONS:
        /// <summary>
        /// Returns a queriable of all specifications
        /// </summary>
        /// <returns>A qieryable collection of Specification</returns>
        IQueryable<Specification> GetAllSpecsQuery();

        /// <summary>
        /// Checks if a given recipe ingredient Specification exists by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true or false</returns>
        Task<bool> SpecificationExistsByIdAsync(int id);

        /// <summary>
        /// Checks if a given recipe ingredient Specification exists by name
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true or false</returns>
        Task<bool> SpecificationExistsByNameAsync(string name);

        /// <summary>
        /// Adds a Specification to the database
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        Task AddSpecAsync(Specification spec);

        /// <summary>
        /// Updates an existing Specification
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        Task UpdateSpecAsync(Specification spec);

        /// <summary>
        /// Deletes an existing Specification from the database
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        Task DeleteSpecAsync(Specification mespecasure);

        
    }
}
