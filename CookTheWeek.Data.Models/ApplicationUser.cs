namespace CookTheWeek.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using static Common.EntityValidationConstants.ApplicationUser;

    /// <summary>
    /// Custom user class that works with the default ASP.NET Core Identity
    /// </summary>
    [Comment("The Application User")]
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid();   
            this.FavoriteRecipes = new HashSet<FavouriteRecipe>();
        }
        [Comment("A collection of the user`s favourite recipes")]
        public ICollection<FavouriteRecipe> FavoriteRecipes { get; set; }
    }
}
