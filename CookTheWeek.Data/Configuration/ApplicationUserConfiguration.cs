namespace CookTheWeek.Data.Configuration
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using CookTheWeek.Data.Models;

    using static Common.GeneralApplicationConstants;

    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder
                .HasData(SeedUsers());
        }

        internal ICollection<ApplicationUser> SeedUsers()
        {
            ICollection<ApplicationUser> seededUsers = new HashSet<ApplicationUser>();
            
            PasswordHasher<ApplicationUser> hasher = new PasswordHasher<ApplicationUser>();

            ApplicationUser appUser = new ApplicationUser()
            {
                Id = Guid.Parse(AppUserId),
                UserName = AppUserUsername,
                NormalizedUserName = AppUserUsername.ToUpper(),
                Email = AppUserEmail,
                NormalizedEmail = AppUserEmail.ToUpper(),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };

            appUser.PasswordHash = hasher.HashPassword(appUser, AppUserPassword);
            seededUsers.Add(appUser);

            ApplicationUser adminUser = new ApplicationUser()
            {
                Id = Guid.Parse(AdminUserId),
                UserName = AdminUserUsername,
                NormalizedUserName = AdminUserUsername.ToUpper(),
                Email = AdminUserEmail,
                NormalizedEmail = AdminUserEmail.ToUpper(),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };

            adminUser.PasswordHash = hasher.HashPassword(appUser, AdminUserPassword);
            seededUsers.Add(adminUser);

            return seededUsers;
        }
    }
}
