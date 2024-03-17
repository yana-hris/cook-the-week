﻿namespace CookTheWeek.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Data.Models.Recipe;
    using Web.ViewModels.Recipe;

    public interface IRecipeService
    {        
        Task<ICollection<RecipeAllViewModel>> AllUnsortedUnfilteredAsync();

        Task<AllRecipesFilteredAndPagedServiceModel> AllAsync(AllRecipesQueryModel queryModel);
        Task AddRecipeAsync(RecipeFormViewModel model);
    }
}
