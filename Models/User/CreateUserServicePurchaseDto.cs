using System.ComponentModel.DataAnnotations;

namespace CheapUdemy.Models.User
{
    public class CreateUserServicePurchaseDto
    {
        [Required]
        [EmailAddress]
        public string? UdemEmail { get; set; }

        [Required]
        public string? Type { get; set; }

        [Required]
        public string? Order { get; set; }

    }

}
