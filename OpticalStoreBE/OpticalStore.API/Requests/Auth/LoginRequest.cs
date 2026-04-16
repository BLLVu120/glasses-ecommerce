using System.ComponentModel.DataAnnotations;

namespace OpticalStore.API.Requests.Auth
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(255, MinimumLength = 6)]
        public string Password { get; set; } = null!;
    }
}
