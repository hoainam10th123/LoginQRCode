using System.ComponentModel.DataAnnotations;

namespace LoginQRCode.Dtos
{
    public class LoginDto
    {
        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string Username { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
