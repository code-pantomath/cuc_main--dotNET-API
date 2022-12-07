namespace CheapUdemy.Data
{
    public class UserServicePurchase
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }

        public string UdemEmail { get; set; }
        public string Type { get; set; }
        public string Order { get; set; }
        public byte Price { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
