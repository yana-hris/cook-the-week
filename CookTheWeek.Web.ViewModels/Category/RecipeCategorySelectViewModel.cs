﻿namespace CookTheWeek.Web.ViewModels.Category
{
    using System.ComponentModel.DataAnnotations;

    using CookTheWeek.Web.ViewModels.Interfaces;

    public class RecipeCategorySelectViewModel : ISelectViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;
    }
}
