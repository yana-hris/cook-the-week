﻿namespace CookTheWeek.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Web.ViewModels.MealPlan;
    using Web.ViewModels.Admin.MealPlanAdmin;

    public interface IMealPlanService
    {
        Task AddAsync(string userId, MealPlanAddFormModel model);
        Task<ICollection<MealPlanAllAdminViewModel>> AllActiveAsync();
        Task<ICollection<MealPlanAllAdminViewModel>> AllFinishedAsync();
        Task<int> AllActiveCountAsync();
        Task<ICollection<MealPlanAllViewModel>> MineAsync(string userId);
        Task<MealPlanAddFormModel> GetForEditByIdAsync(string id);
        Task<bool> ExistsByIdAsync(string id);
        Task<MealPlanViewModel> GetByIdAsync(string id);
        Task<int> GetIngredientsCount(string id);
        Task<int> GetTotalMinutes(string id);
    }
}
