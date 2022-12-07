namespace CheapUdemy.Data
{
    public class Payment
    {
        public int Id { get; set; }
        //public int UserId { get; set; }

        //public byte Amount { get; set; }

        public string? Data { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
