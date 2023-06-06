using System.ComponentModel.DataAnnotations;

namespace FunMediaStore.Model.Transfer
{
    public class AuthRequest
    {
        [Required]
        [EmailAddress]

        public string? Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The password must be at least 3 characters long.")]
        public string? Password { get; set; }
    }
}
