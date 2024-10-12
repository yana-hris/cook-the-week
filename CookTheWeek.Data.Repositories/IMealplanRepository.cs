namespace CookTheWeek.Data.Repositories
{
    using System.Threading;

    using CookTheWeek.Data.Models;

    public interface IMealplanRepository
    {
        /// <summary>
        /// Retrieves an <see cref="IQueryable{MealPlan}"/> representing the queryable 
        /// collection of all meal plans from the database. 
        /// The query is deferred, allowing further filtering and querying operations 
        /// before execution.
        /// </summary>
        /// <returns>
        /// An <see cref="IQueryable{MealPlan}"/> that represents the queryable collection 
        /// of meal plans.
        /// </returns>       
        IQueryable<MealPlan> GetAllQuery();

        /// <summary>
        /// Gets a Mealplan by id as queryable
        /// </summary>
        /// <param name="id"></param>
        /// <returns>MealPlan as queryable</returns>
        IQueryable<MealPlan> GetByIdQuery(Guid id);

        /// <summary>
        /// Creates a new Mealplan in the database
        /// </summary>
        /// <param name="mealPlan"></param>
        /// <returns>the newly created MealPlan Id as string</returns>
        Task<string> AddAsync(MealPlan mealPlan);

        /// <summary>
        /// Updates an existing meal plan
        /// </summary>
        /// <param name="mealPlan"></param>
        /// <returns></returns>
        Task UpdateAsync(MealPlan mealPlan);

        /// <summary>
        /// Deletes a single meal plan
        /// </summary>
        /// <param name="mealPlan"></param>
        /// <returns></returns>
        Task RemoveAsync(MealPlan mealPlan);

        /// <summary>
        /// Deletes a collection of meal plans
        /// </summary>
        /// <param name="mealPlans"></param>
        /// <returns></returns>
        Task RemoveRangeAsync(ICollection<MealPlan> mealPlans);

        /// <summary>
        /// Asynchronously saves all tracked changes in the DbContext to the database, 
        /// including any related entities that have been modified.
        /// </summary>
        /// <param name="cancellationToken">
        /// A token that can be used to cancel the asynchronous operation.
        /// If cancellation is requested, the save operation will be canceled, 
        /// and an <see cref="OperationCanceledException"/> will be thrown.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous save operation.
        /// </returns>
        Task SaveAsync(CancellationToken cancellationToken);
        
    }
}
