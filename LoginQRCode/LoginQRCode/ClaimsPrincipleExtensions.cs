using System.Security.Claims;

namespace LoginQRCode
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            try {
                return user.FindFirst(ClaimTypes.Name).Value;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }            
        }
    }
}
