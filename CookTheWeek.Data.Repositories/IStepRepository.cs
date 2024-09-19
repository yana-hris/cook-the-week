namespace CookTheWeek.Data.Repositories
{
    using CookTheWeek.Data.Models;

    public interface IStepRepository
    {
        Task AddAllAsync(ICollection<Step> steps);
        Task UpdateAllAsync(string recipeId, ICollection<Step> steps);
        Task DeleteAllAsync(string recipeId);
    }
}
