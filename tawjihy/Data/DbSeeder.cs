using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace tawjihy.Data
{
    public static class DbSeeder
    {
        public static async Task SeedDatabaseAsync(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                try
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                    await SeedDefaultUserAsync(userManager);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }



        private static async Task SeedDefaultUserAsync(UserManager<IdentityUser> userManager)
        {
            if (await userManager.Users.AnyAsync())
            {
                return;
            }

            var user = new IdentityUser
            {
                UserName = "dev@gmail.com",
                Email = "dev@gmail.com",
                EmailConfirmed = true,
            };

            await userManager.CreateAsync(user, "@Admin@1234@");
        }

    }
}
