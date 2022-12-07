using System.ComponentModel.DataAnnotations;

namespace CheapUdemy.Models.Udem
{
    public class CreateUdemAccountDto
    {
        [Required]
        public string? Name { get; set; }
        [EmailAddress]
        public string? Email { get; set; }

    }
}
