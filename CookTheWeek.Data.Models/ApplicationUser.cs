namespace CookTheWeek.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Custom user class that works with the default ASP.NET Core Identity
    /// </summary>
    [Comment("The Application User")]
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid();   
            this.Recipes = new HashSet<Recipe>();   
            this.FavoriteRecipes = new HashSet<FavouriteRecipe>();
            this.MealPlans = new HashSet<MealPlan>();
        }

        [Comment("A collection of user`s owned recipes")]
        public ICollection<Recipe> Recipes { get; set; }

        [Comment("A collection of the user`s favourite recipes")]
        public ICollection<FavouriteRecipe> FavoriteRecipes { get; set; }

        [Comment("A set of Meal Plans built by the User")]
        public ICollection<MealPlan> MealPlans { get; set; }
    }
}
