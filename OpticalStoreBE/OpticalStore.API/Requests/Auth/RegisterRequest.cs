using System;
using System.ComponentModel.DataAnnotations;

namespace OpticalStore.API.Requests.Auth
{
    public class RegisterRequest
    {
        [DataType(DataType.Date)]
        public DateTime? Dob { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Username { get; set; } = null!;

        [Required]
        [StringLength(255, MinimumLength = 6)]
        public string Password { get; set; } = null!;

        [StringLength(20)]
        public string? Phone { get; set; }
    }
}
