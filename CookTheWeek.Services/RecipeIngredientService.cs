﻿namespace CookTheWeek.Services.Data
{    
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data;
    using CookTheWeek.Data.Models;
    using Interfaces;
    using Web.ViewModels.RecipeIngredient;

    public class RecipeIngredientService : IRecipeIngredientService
    {
        private readonly CookTheWeekDbContext dbContext;

        public RecipeIngredientService(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ICollection<RecipeIngredientSelectMeasureViewModel>> GetRecipeIngredientMeasuresAsync()
        {
            ICollection<RecipeIngredientSelectMeasureViewModel> allMeasures = await this.dbContext
                .Measures
                .AsNoTracking()
                .Select(m => new RecipeIngredientSelectMeasureViewModel()
                {
                    Id = m.Id,
                    Name = m.Name,

                }).ToListAsync();

            return allMeasures;
        }

        public async Task<ICollection<RecipeIngredientSelectSpecificationViewModel>> GetRecipeIngredientSpecificationsAsync()
        {
            ICollection<RecipeIngredientSelectSpecificationViewModel> allSpecs = await this.dbContext
                .Specifications
                .AsNoTracking()
                .Select(sp => new RecipeIngredientSelectSpecificationViewModel()
                {
                    Id = sp.Id,
                    Description = sp.Description
                }).ToListAsync();

            return allSpecs;
        }

        public async Task<int> AddAsync(RecipeIngredientFormModel model, string recipeId)
        {
            int ingredientId = await this.dbContext.Ingredients
                .AsNoTracking()
                .Where(i => i.Name.ToLower() == model.Name.ToLower())
                .Select(i => i.Id)
                .FirstOrDefaultAsync();

         
            RecipeIngredient recipeIngredient = new RecipeIngredient()
            {
                IngredientId = ingredientId,
                RecipeId = Guid.Parse(recipeId),
                MeasureId = model.MeasureId!.Value,
                Qty = model.Qty.GetDecimalQtyValue(),
                SpecificationId = model.SpecificationId!.Value
            };

            await this.dbContext.RecipesIngredients
                .AddAsync(recipeIngredient);
            await this.dbContext.SaveChangesAsync();

            return recipeIngredient.IngredientId;
        }

        public async Task<bool> IsAlreadyAddedAsync(string ingredientName, string recipeId)
        {
            return await this.dbContext.RecipesIngredients
                .AsNoTracking()
                .AnyAsync(ri => ri.RecipeId.ToString() == recipeId && ri.Ingredient.Name == ingredientName);
        }

        public async Task<bool> IngredientMeasureExistsAsync(int measureId)
        {
            return await this.dbContext.Measures
                .AsNoTracking()
                .AnyAsync(m => m.Id == measureId);
        }

        public async Task<bool> IngredientSpecificationExistsAsync(int specificationId)
        {
            return await this.dbContext.Specifications
                .AsNoTracking()
                .AnyAsync(sp => sp.Id == specificationId);
        }

        public async Task RemoveAsync(int ingredientId, string recipeId)
        {
            RecipeIngredient? recipeIngredient = await this.dbContext.RecipesIngredients
                .FirstOrDefaultAsync(ri => ri.IngredientId == ingredientId && ri.RecipeId.ToString() == recipeId);

            if(recipeIngredient == null)
            {
                throw new InvalidOperationException("No such ingredient exists for this recipe!");
            }

            this.dbContext.RecipesIngredients.Remove(recipeIngredient);
            await this.dbContext.SaveChangesAsync();
        }
    }
}
