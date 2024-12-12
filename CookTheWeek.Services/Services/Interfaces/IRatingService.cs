namespace CookTheWeek.Services.Data.Services.Interfaces
{
    using System;
    using System.Threading.Tasks;

    public interface IRatingService
    {
        Task SoftDeleteAllByRecipeIdAsync(Guid recipeId);
    }
}
