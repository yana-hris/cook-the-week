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
            this.Recipes = new HashSet<Recipe>();
        }
        public ICollection<Recipe> Recipes { get; set; }
    }
}
