using System.ComponentModel.DataAnnotations;


namespace CheapUdemy.Data
{
    public class User
    {
        public int Id { get; set; }


        [MinLength(3)]
        [MaxLength(15)]
        public string FirstName { get; set; }

        [MinLength(3)]
        [MaxLength(15)]
        public string LastName { get; set; }

        [MinLength(5)]
        [MaxLength(45)]
        public string Email { get; set; }

        //[MinLength(8)]
        //[MaxLength(32)]
        public string Password { get; set; }

        public string? IP { get; set; }


        //[Newtonsoft.Json.JsonIgnore]
        ////************////::
        //public virtual Wallet? Wallet { get; set; }

        public int UdemAccountsAmount { get; set; } = 0;

        [MaxLength(14)]
        public string? UniqueCode { get; set; } = Guid.NewGuid().ToString().ToUpper().Split('-').LastOrDefault();
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    }

}
