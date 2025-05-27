using LoginQRCode.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LoginQRCode
{
    public static class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager)
        {
            if (!await userManager.Users.AnyAsync())
            {
                var users = new List<AppUser> {
                        new AppUser {
                            UserName = "hoainam10th",
                            DisplayName = "Nguyễn Hoài Nam",
                        },
                        new AppUser{ UserName="ubuntu", DisplayName = "Ubuntu Nguyễn"},
                    };

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "123456");
                }
            }           
        }
    }
}
