using CheapUdemy.Data;

namespace CheapUdemy.Contracts
{
    public interface IUsersRepository : IGenericRepository<User>
    {

        Task<User?> GetByEmailAsync(string? email);

        Task<User?> GetDetails(int? id);

        Task<User?> CheckUserLoginDataAsync(UserLogin user);


        Task<Wallet?> AddWalletCreditsAsync(int userId, byte amount, string? paymentData);

        Task<Wallet?> GetWalletAsync(int id);

        Task<Wallet?> TransferWalletCreditsAsync(int userId, string? receiverEmail, ushort amount);


        Task<List<UdemAccount>?> AddUdemyAccountAsync(UdemAccount acc, int userId);
        Task<List<UdemAccount>?> DeleteUdemyAccountAsync(int udemAccountId, int userId);
        Task<List<UdemAccount>?> GetUdemyAccountsAsync (int id);


        Task<UserServicePurchase?> PurchaseServiceAsync(UserServicePurchase purchase);
        Task<UserServicePurchase?>? GetServicePurchase(int userId, int purchaseId, string purchaseType, string? history);

    }

}
