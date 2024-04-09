namespace CookTheWeek.Services.Tests
{
    using Microsoft.AspNetCore.Identity;

    using CookTheWeek.Data;
    using CookTheWeek.Data.Models;

    using static Common.GeneralApplicationConstants;

    public static class DbSeeder
    {
        public static ApplicationUser AdminUser;
        public static ApplicationUser AppUser;

        public static void SeedDatabase(CookTheWeekDbContext dbContext)
        {
            PasswordHasher<ApplicationUser> hasher = new PasswordHasher<ApplicationUser>();

            AdminUser = new ApplicationUser()
            {
                Id = Guid.Parse(AdminUserId),
                UserName = AdminUserUsername,
                NormalizedUserName = AdminUserUsername.ToUpper(),
                Email = AdminUserEmail,
                NormalizedEmail = AdminUserEmail.ToUpper(),
                ConcurrencyStamp = "caf271d7-0ba7-4ab1-8d8d-6d0e3711c27d",
                SecurityStamp = "ca32c787-626e-4234-a4e4-8c94d85a2b1c",
                EmailConfirmed = true,
            };

            AdminUser.PasswordHash = hasher.HashPassword(AdminUser, AdminUserPassword);

            AppUser = new ApplicationUser()
            {
                Id = Guid.Parse("65fc0e0d-6572-4ec6-a853-c633d9f28c9e"),
                UserName = "testUser",
                NormalizedUserName = "TESTUSER",
                Email = "user@user.bg",
                NormalizedEmail = "USER@USER.BG",
                ConcurrencyStamp = "8b51706e-f6e8-4dae-b240-54f856fb3004",
                SecurityStamp = "f6af46f5-74ba-43dc-927b-ad83497d0387",
                EmailConfirmed = true,
            };

            AppUser.PasswordHash = hasher.HashPassword(AppUser, "123123");

            dbContext.Users.Add(AdminUser);
            dbContext.Users.Add(AppUser);

            dbContext.SaveChanges();
        }

    }
}
