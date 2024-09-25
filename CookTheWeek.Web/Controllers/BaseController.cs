﻿namespace CookTheWeek.Web.Controllers
{
    using CookTheWeek.Services.Data.Models.Validation;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;


    [Authorize]
    public abstract class BaseController : Controller
    {
        
        public BaseController()
        {
            
        }

        // Helper method for adding validation errors to ModelState
        protected void AddValidationErrorsToModelState(ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }
        }

    }
}
