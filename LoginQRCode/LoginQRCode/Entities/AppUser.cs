using Microsoft.AspNetCore.Identity;


namespace LoginQRCode.Entities
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; } = null!;
    }
}
