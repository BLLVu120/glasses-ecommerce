using System.ComponentModel.DataAnnotations;

namespace OpticalStore.API.Requests.Auth
{
    public class RefreshTokenRequest
    {
        [Required]
        [MinLength(1)]
        public string RefreshToken { get; set; } = null!;
    }
}
