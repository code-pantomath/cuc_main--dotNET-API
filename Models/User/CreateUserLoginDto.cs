using System.ComponentModel.DataAnnotations;

namespace CheapUdemy.Models.User
{
    public class CreateUserLoginDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }

    }

}
