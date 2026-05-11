namespace bookStore.BusinessLogic.Configuration
{
    public class JwtOptions
    {
        public string Key { get; set; } = "DevOnly_ChangeMe_Use_Long_Secret_Key_At_Least_32_Chars!";
        public string Issuer { get; set; } = "bookStore";
        public string Audience { get; set; } = "bookStoreClients";
        public int ExpiresMinutes { get; set; } = 120;

        public static JwtOptions Current { get; private set; } = new();

        public static void Configure(JwtOptions options)
        {
            Current = options;
        }
    }
}
