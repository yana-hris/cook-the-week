namespace CookTheWeek.Services.Interfaces
{
    using CookTheWeek.Web.ViewModels.Ingredient;
    using System.Threading.Tasks;

    public interface IIngredientService
    {
        Task AddIngredientAsync(IngredientFormViewModel model);
        Task<bool> existsByNameAsync(string name);
    }
}
