namespace KadaiMVCApp
{
    public static class AppSettings
    {
        public static string dbConnectString;
        public static void Initialize(IConfiguration configuration)
        {
            dbConnectString = configuration["ConnectionString"];//Program.csのconfigから指定
        }
    }
}
