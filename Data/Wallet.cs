using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CheapUdemy.Data
{

    //[Owned]
    public class Wallet
    {
        public int Id { get; set; }

        public int OwnerId { get; set; }
        public string? Type { get; set; } = "Primary";
        public string? Currency { get; set; } = "USD";

        [Range(0, ushort.MaxValue)]
        public ushort Value { get; set; } = 0;


        public List<WalletHistoryOperationObj> History { get; set; } = new List<WalletHistoryOperationObj>();

    }


    //[Owned]
    //[Keyless]
    public class WalletHistoryOperationObj
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }

        public char? Type { get; set; }

        [Range(0, ushort.MaxValue)]
        public ushort Value { get; set; }

        public DateTime? CreatedAt { get; internal set; } = DateTime.UtcNow;

    }

}