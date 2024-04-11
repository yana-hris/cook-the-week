﻿namespace CookTheWeek.Services.Data.Interfaces
{
    using CookTheWeek.Web.ViewModels.User;

    public interface IUserService
    {
        Task<ICollection<UserAllViewModel>> AllAsync();
        Task<int> AllCountAsync();
        Task<bool> ExistsByIdAsync(string id);
        Task<bool> IsOwnerByRecipeId(string recipeId, string userId);
    }
}
