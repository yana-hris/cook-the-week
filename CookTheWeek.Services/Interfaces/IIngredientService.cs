namespace CookTheWeek.Services.Interfaces
{
    using CookTheWeek.Web.ViewModels.Ingredient;
    using System.Threading.Tasks;

    public interface IIngredientService
    {
        Task<bool> existsByNameAsync(string name);
        Task<int> AddIngredientAsync(IngredientFormViewModel model);
    }
}
