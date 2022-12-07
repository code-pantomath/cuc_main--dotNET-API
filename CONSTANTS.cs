namespace CheapUdemy
{
    public abstract class CONSTANTS
    {

        public static byte SERVICE_ACCOUNT_PRICE { get; } = 6;
        public static byte SERVICE_GIFT_PRICE { get; } = 7;
        /////
        ///

        public static byte[] WALLET_CHARGE_RANGES { get; } = new byte[5] { 6, 7, 12, 14, 28 };
        ///


        //public static Dictionary<byte, int> PAYMENTGATEWAYSIDS { get; } = new Dictionary<byte, int>()
        //{
        //    [6] = 3435776,
        //    [7] = 3435808,
        //    [12] = 3435816,
        //    [14] = 3435819,
        //    [28] = 3435823,
        //};

    }
}
