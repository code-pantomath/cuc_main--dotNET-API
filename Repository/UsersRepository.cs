using CheapUdemy.Contracts;
using CheapUdemy.Data;
using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.ChangeTracking;

//using BCrypt.Net;


namespace CheapUdemy.Repository
{
    public class UsersRepository : GenericRepository<User>, IUsersRepository
    {
        private readonly MyAppDbContext _ctx;

        public UsersRepository(MyAppDbContext context) : base(context)
        {
            this._ctx = context;
        }



        //private User? GetLastUser()
        //{
        //    return _ctx.Set<User>().ToList()?.LastOrDefault();
        //}


        public override async Task<User> AddAsync(User user)
        {

            int theUserId = _ctx.Set<User>().ToList().FirstOrDefault() is null ? 0 : _ctx.Set<User>().ToList().LastOrDefault().Id;
            int currUserWalletId = theUserId + 1;

            Wallet UserWallet = new Wallet
            {
                OwnerId = currUserWalletId,
                Currency = "USD",
                Type = "Primary",
                Value = 0,
                History = new List<WalletHistoryOperationObj>(),
            };

            ////************////::
            //user.Wallet = UserWallet;

            string userPswd_Ptr = user.Password;
            user.Password = BCrypt.Net.BCrypt.HashPassword(userPswd_Ptr);

            await _ctx.AddAsync(user);

            await _ctx.Set<Wallet>().AddAsync(UserWallet);


            await _ctx.SaveChangesAsync();


            user.Password = userPswd_Ptr;
            return user;
        }


        public Task<User?> GetDetails(int? id)
        {
            throw new NotImplementedException();
        }


        public async Task<User?> GetByEmailAsync(string? email)
        {
            User? user = (await _ctx.Set<User>().ToListAsync()).Find(u => u.Email.Equals(email));
            return user;
        }


        public async Task<User?> CheckUserLoginDataAsync(UserLogin userData)
        {
            //bool isUserExist = (await GetAllAsync()).Any(u => u.Email.ToLower().Equals(userData?.Email?.ToLower()) && BCrypt.Net.BCrypt.Verify(userData.Password, u.Password));

            User? user = (await GetAllAsync()).Find(u => u.Email.Equals(userData?.Email?.ToLower()) && BCrypt.Net.BCrypt.Verify(userData.Password, u.Password));

            if (user is null) return null;

            return user;
        }




        public async Task<Wallet?> GetWalletAsync(int id)
        {
            Wallet? wallet = _ctx.Set<Wallet>().ToList().Find(w => w.OwnerId == id);

            List<WalletHistoryOperationObj>? whoos = await _ctx.Set<WalletHistoryOperationObj>().ToListAsync();

            if (wallet is null || whoos is null) return null;

            wallet.History = whoos.FindAll(whoo => whoo.OwnerId == id).Reverse<WalletHistoryOperationObj>().ToList();

            return wallet;
        }


        public async Task<Wallet?> AddWalletCreditsAsync(int userId, byte amount, string? paymentData="")
        {
            if (amount == 0) return null;

            User? user = (await GetAllAsync()).Find(u => u.Id == userId);

            if (user is null) return null;

            Wallet? wallet = _ctx.Set<Wallet>().ToList().Find(w => w.OwnerId == user.Id);

            if (wallet is null) return null;


            wallet.Value += amount;


            var WHOO = new WalletHistoryOperationObj
            {
                OwnerId = userId,
                Type = '+',
                Value = amount,
            };

            await _ctx.Set<WalletHistoryOperationObj>().AddAsync(WHOO);


            wallet.History = _ctx.Set<WalletHistoryOperationObj>().ToList().FindAll(whoo => whoo.OwnerId == userId).Concat<WalletHistoryOperationObj>(new List<WalletHistoryOperationObj>() { WHOO }).Reverse().ToList();


            await _ctx.Set<Payment>().AddAsync(new Payment
            {
                Data = paymentData
            });


            await _ctx.SaveChangesAsync();

            return _ctx.Set<Wallet>().ToList().Find(uw => uw.Id == user.Id);
        }


        public async Task<Wallet?> TransferWalletCreditsAsync(int userId, string? receiverEmail, ushort amount)
        {
            User? senderUser = _ctx.Set<User>().ToList().Find(u => u.Id == userId);
            User? receiverUser = _ctx.Set<User>().ToList().Find(u => u.Email.Equals(receiverEmail?.ToLower()));

            if (senderUser is null || receiverUser is null || receiverEmail is null) return null;
            if (receiverEmail.ToLower().Equals(senderUser.Email)) return null;

            Wallet? senderWallet = await GetWalletAsync(userId) ?? _ctx.Set<Wallet>().ToList().Find(uw => uw.OwnerId == senderUser?.Id);
            Wallet? receiverWallet = await GetWalletAsync(receiverUser.Id);

            if (senderWallet is null || receiverWallet is null) return null;
            if (senderWallet.Value < amount) return null;



            senderWallet.History.Add(new WalletHistoryOperationObj
            {
                OwnerId = userId,
                Type = '-',
                Value = amount,
                CreatedAt = DateTime.UtcNow,
            });
            senderWallet.Value -= amount;

            amount--; // Cut off 1Credit I.E. 1$ from the transfered amount.

            receiverWallet.History.Add(new WalletHistoryOperationObj
            {
                OwnerId = receiverUser.Id,
                Type = '+',
                Value = amount,
                CreatedAt = DateTime.UtcNow,
            });
            receiverWallet.Value += amount;


            await _ctx.SaveChangesAsync();

            Wallet? userWallet_updated = _ctx.Set<Wallet>().ToListAsync().Result.Find(uw => uw.OwnerId == userId);
            //userWallet_updated.History = userWallet_updated.History.Reverse<WalletHistoryOperationObj>().ToList();
            return userWallet_updated;
        }




        public async Task<List<UdemAccount>?> AddUdemyAccountAsync(UdemAccount acc, int userId)
        {
            acc.OwnerId = userId;

            List<User> users = await GetAllAsync();
            User? user = users.Find(u => u.Id == userId);


            if (user is not null && user.UdemAccountsAmount >= 5) return null;

            List<UdemAccount> udemAccounts = _ctx.Set<UdemAccount>().ToList();

            if (udemAccounts.Any(ua => (ua.Email.Equals(acc.Email) || (ua.Name.Equals(acc.Name) && ua.OwnerId == userId)))) return null;


            if (user is null) return null;

            await _ctx.Set<UdemAccount>().AddAsync(acc);
            user.UdemAccountsAmount++;
            await _ctx.SaveChangesAsync();

            List<UdemAccount>? accounts = _ctx.Set<UdemAccount>()?.ToList()?.FindAll(ua => ua.OwnerId == userId);

            return accounts;
            
        }

        public async Task<List<UdemAccount>?> DeleteUdemyAccountAsync(int udemAccountId, int userId)
        {
            List<User> users = await GetAllAsync();
            List<UdemAccount> udemAccounts = _ctx.Set<UdemAccount>().ToList();

            User? user = users.Find(u => u.Id == userId);

            if (user is null) return null;

            UdemAccount? udemAccount = udemAccounts.Find(ua => ua.Id == udemAccountId && ua.OwnerId == userId);


            if (udemAccount is null) return null;


            _ctx.Set<UdemAccount>().Remove(udemAccount);
            user.UdemAccountsAmount--;
            await _ctx.SaveChangesAsync();


            List<UdemAccount>? accounts = _ctx.Set<UdemAccount>()?.ToList()?.FindAll(ua => ua.OwnerId == userId);

            return accounts;

        }

        public async Task<List<UdemAccount>?> GetUdemyAccountsAsync(int id)
        {
            await Task.CompletedTask;
            return _ctx.Set<UdemAccount>().ToList().FindAll(u => u.OwnerId == id);
        }



        public async Task<UserServicePurchase?> PurchaseServiceAsync(UserServicePurchase purchase)
        {
            if (purchase is null) return null;

            UdemAccount? udemAcc = _ctx.Set<UdemAccount>().ToList().Find(ua => ua.Email.ToLower().Equals(purchase.UdemEmail.ToLower()) && ua.OwnerId == purchase.OwnerId);
            //if (udemAcc is null) udemAcc = _ctx.Set<UdemAccount>().ToList().Find(ua => ua.OwnerId == purchase.Id);
            Wallet? wallet = _ctx.Set<Wallet>().ToListAsync().Result.Find(uw => uw.OwnerId == purchase.OwnerId);

            Console.WriteLine($"\n\n val:{wallet?.Value} || price:{purchase.Price} \n\n");

            if ((purchase.Type.ToLower().Equals("gift") && udemAcc is null) || wallet is null) return null;
            if (wallet.Value < purchase.Price && wallet.Value != purchase.Price) return null;

            await _ctx.Set<UserServicePurchase>().AddAsync(purchase);

            await _ctx.Set<WalletHistoryOperationObj>().AddAsync(new WalletHistoryOperationObj
            {
                OwnerId = purchase.OwnerId,
                Type = '-',
                Value = purchase.Price,
            });

            wallet.Value -= purchase.Price;

            await _ctx.SaveChangesAsync();

            return purchase;
        }

        public Task<UserServicePurchase?>? GetServicePurchase(int userId, int purchaseId, string purchaseType, string? history)
        {
            //await Task.CompletedTask;

            Console.WriteLine(history);

            Console.Write(string.Format("{0:yyyy-MM-ddTHH:mm:ss.FFFZ}", _ctx.Set<UserServicePurchase>().ToListAsync().Result.Find(usp => usp.OwnerId == userId)?.CreatedAt).Replace(".", String.Empty)?.Replace(":", String.Empty).Replace("-", String.Empty));

            UserServicePurchase? purchase = _ctx.Set<UserServicePurchase>().ToListAsync().Result.Find(usp =>
                usp.OwnerId == userId &&
                usp.Id == purchaseId &&
                usp.Type == purchaseType &&
                ((string.Format("{0:yyyy-MM-ddTHH:mm:ss.FFFZ}", usp.CreatedAt)?.Replace("-", string.Empty)?.Replace(":", string.Empty)?.Replace(".", string.Empty))?.Split('T')?[0]).Equals(history?.Split('T')?[0])
            );

            Console.Write(purchase is null);


            if (purchase is null) return null;
            else return Task.FromResult(purchase);
        }


    }

}
