namespace CookTheWeek.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;

    using static Common.EntityValidationConstants.ApplicationUser;

    /// <summary>
    /// Custom user class that works with the default ASP.NET Core Identity
    /// </summary>
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid();   
            this.FavoriteRecipes = new HashSet<FavouriteRecipe>();
        }
        public ICollection<FavouriteRecipe> FavoriteRecipes { get; set; }
    }
}
