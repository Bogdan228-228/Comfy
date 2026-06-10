using DOMAIN;
using Microsoft.AspNetCore.Identity;

namespace DLL
{
    public static class DbSeeder
    {
        public static async Task SeedAdminAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));

            var adminUser = await userManager.FindByEmailAsync("admin@comfy.com");
            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = "admin@comfy.com",
                    Email = "admin@comfy.com",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
