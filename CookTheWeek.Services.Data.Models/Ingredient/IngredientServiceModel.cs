﻿namespace CookTheWeek.Services.Data.Models.Ingredient
{
    
    public class IngredientServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int CategoryId { get; set; }
    }
}
