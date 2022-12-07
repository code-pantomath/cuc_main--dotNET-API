using System.ComponentModel.DataAnnotations;

namespace CheapUdemy.Models.User
{
    public class CreateUserDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(15)]
        public string? FirstName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(15)]
        public string? LastName { get; set; }

        [Required]
        [EmailAddress]
        [MinLength(5)]
        [MaxLength(45)]
        public string? Email { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(32)]
        public string? Password { get; set; }

        public string? IP { get; set; }


    }

}
