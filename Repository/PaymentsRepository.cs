//using CheapUdemy.Contracts;
//using CheapUdemy.Data;

//namespace CheapUdemy.Repository
//{
//    public class PaymentsRepository : GenericRepository<Payment>, IPaymentsRepository
//    {
//        private readonly MyAppDbContext _ctx;

//        public PaymentsRepository(MyAppDbContext context) : base(context)
//        {
//            this._ctx = context;
//        }



//        public async Task<Payment?> Pay(Payment paymentParam)
//        {
//            Payment payment = await AddAsync(paymentParam);

//            if (payment is null) return null;

//            User? user = _ctx.Set<User>().ToList().Find(u => u.Id == paymentParam.UserId);
//            Wallet? userWallet = _ctx.Set<Wallet>().ToList().Find(w => w.OwnerId == paymentParam.UserId);

//            if (user is null || userWallet is null) return null;
//            if (userWallet.Value < paymentParam.Amount) return null;

//            userWallet.Value -= paymentParam.Amount;

//            await _ctx.Set<WalletHistoryOperationObj>().AddAsync(new WalletHistoryOperationObj
//            {
//                OwnerId = paymentParam.UserId,
//                Type = '-',
//                Value = paymentParam.Amount,
//            });

//            await _ctx.SaveChangesAsync();

//            return payment;
//        }


//        async Task<Payment?> SavePaymentRecordAsync(Payment payment)
//        {
//            await _ctx.Set<Payment>().AddAsync(payment);
//            await _ctx.SaveChangesAsync();

//            return payment;
//        }



//    }

//}
