using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CheapUdemy.Data
{
    public class MyAppDbContext : DbContext
    {

        public MyAppDbContext(DbContextOptions options) : base(options)
        {

            

        }


        public DbSet<User>? Users { get; set; }
        public DbSet<Wallet>? Wallets { get; set; }
        public DbSet<WalletHistoryOperationObj>? WalletHistoryOperations { get; set; }
        public DbSet<UdemAccount>? UdemAccounts { get; set; }
        public DbSet<UserServicePurchase>? UserServicePurchases { get; set; }

        public DbSet<Payment>? Payments { get; set; }




        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);


        //    //modelBuilder.Entity<Wallet>().Property(w => w.History).HasColumnName("Id");


        //    /////


        //    //modelBuilder.Entity<User>().OwnsOne(u => u.Wallet).OwnsOne(uw => uw.History);

        //    //modelBuilder.Entity<User>().OwnsOne(u => u.Udem).OwnsOne(ud => ud.UdemyAccounts);

        //    //modelBuilder.Entity<User>().Property(u => u.UdemAccounts)
        //    //    .HasConversion(
        //    //        v => JsonSerializer.Serialize(v, null),
        //    //        v => JsonSerializer.Deserialize<List<UdemyAccount>>(v, null));

        //    //modelBuilder.Entity<User>().Property(u => u.UdemAccounts);

        //    //modelBuilder.Entity<UdemyAccount>().Navigation(u => u.Name).IsRequired();
        //    //modelBuilder.Entity<UdemyAccount>().Navigation(u => u.Email).IsRequired();



        //    //modelBuilder.Entity<User>()
        //    //    .HasOne(u => u.UserWallet)
        //    //    .WithOne(uw => uw.User)
        //    //    .HasForeignKey<UserWallet>(uw => uw.WalletId)
        //    //;


        //}


        // This is how to create default data when the database-server first runs...
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{

        //    base.OnModelCreating(modelBuilder);
        //    modelBuilder.Entity<User>().HasData(new User[] // This methods receives an array of the objects you want to save as default objects that exists before any other ones...
        //    {

        //        new User
        //        {
        //            UserId = 1,
        //            FirstName = "Rashad",
        //            LastName = "Rashad",
        //            Email = "fedevalverde777777777@gmail.com",
        //            Password = "this is a pswd",
        //            IP = null,
        //        },

        //        new User
        //        {
        //            UserId = 2,
        //            FirstName = "Mo",
        //            LastName = "Ad",
        //            Email = "tortorfatal123321@gmail.com",
        //            Password = "this is a pswd",
        //            IP = null,
        //        }

        //    });
        //}
        ///


    }
}
