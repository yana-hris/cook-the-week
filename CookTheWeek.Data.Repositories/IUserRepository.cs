﻿namespace CookTheWeek.Data.Repositories
{
    using CookTheWeek.Data.Models;
    using Microsoft.AspNetCore.Identity;

    public interface IUserRepository
    {
        /// <summary>
        /// Checks if a user exists by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> ExistsByIdAsync(string id);

        /// <summary>
        /// Gets the Application user if exists by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Application User or null</returns>
        Task<ApplicationUser?> FindByEmailAsync(string email);

        /// <summary>
        /// Gets the Application user if exists by name
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>Application user or null</returns>
        Task<ApplicationUser?> FindByNameAsync(string userName);

        /// <summary>
        /// Returns the user, if existing or throws an exception
        /// </summary>
        /// <param name="id"></param>
        /// <returns>User or null</returns>
        /// <exception cref="RecordNotFoundException">If a user is not found, throws an exception</exception>
        Task<ApplicationUser> GetUserByIdAsync(string id);

        /// <summary>
        /// Signs in the user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task SignInUserAsync(ApplicationUser user);


        /// <summary>
        /// Creates the gicen user with the given password in the database
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns>Indetity Result</returns>
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);

        /// <summary>
        /// Returns true or false, indicating if the user has confirmed his email address
        /// </summary>
        /// <param name=""></param>
        /// <returns>true or false</returns>
        Task<bool> IsUserEmailConfirmedAsync(ApplicationUser user);

        /// <summary>
        /// Generates a email confirmation token for the specified user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>token</returns>
        Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user);

        /// <summary>
        /// Validates the user`s token and returns the validation result if it matches
        /// </summary>
        /// <param name="user"></param>
        /// <param name="code"></param>
        /// <returns>IdentityResult</returns>
        /// <exception cref="ArgumentNullException">If the token is null, throws an exception</exception>
        /// <exception cref="RecordNotFoundException">If the user is null, throws and exception</exception>
        Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string code);

        /// <summary>
        /// Generates a password reset token for the specified user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>token</returns>
        Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user);

        /// <summary>
        /// Returns a boolean, indicating if the current user has a password or not
        /// </summary>
        /// <param name="user"></param>
        /// <returns>true or false</returns>
        Task<bool> HasPasswordAsync(ApplicationUser user);

        /// <summary>
        /// Adds a password to an existing user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns>The Identity Result</returns>
        Task<IdentityResult> AddPasswordAsync(ApplicationUser user, string password);

        /// <summary>
        /// Returns is the current user`s password is true or false
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns>true or false</returns>
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);

        /// <summary>
        /// Changes the user`s current password with a new one
        /// </summary>
        /// <param name="user"></param>
        /// <param name="currentPass"></param>
        /// <param name="newPass"></param>
        /// <returns>The Identity Result</returns>
        Task<IdentityResult> ChangePassAsync(ApplicationUser user, string currentPass, string newPass);

        /// <summary>
        /// Resets the User`s password if the token is verified
        /// </summary>
        /// <param name="user"></param>
        /// <param name="token"></param>
        /// <param name="newPassword"></param>
        /// <returns>Identity Result</returns>
        Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword);

        /// <summary>
        /// Deletes the user from the database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task DeleteAsync(ApplicationUser user);

        /// <summary>
        /// Gets the total count of all registered users in the database
        /// </summary>
        /// <returns>int</returns>
        Task<int?> AllCountAsync();

        /// <summary>
        /// Returns an IQueryable collection of all users in the database, that can be filtered and sorted using LINQ
        /// </summary>
        /// <returns>IQueryable collection of ApplicationUser</returns>
        IQueryable<ApplicationUser> GetAllQuery();
    }
}
