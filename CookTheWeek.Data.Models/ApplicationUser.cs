namespace CookTheWeek.Data.Models
{
    using CookTheWeek.Data.Models.RecipeEntities;
    using Microsoft.AspNetCore.Identity;
    
    /// <summary>
    /// Customized Identity User
    /// </summary>
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid();   
            this.Recipes = new HashSet<Recipe>();
        }
        public ICollection<Recipe> Recipes { get; set; }
    }
}
