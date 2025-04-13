using Microsoft.Extensions.Configuration;

namespace WinFormsAppBase.Settings
{

    public static class AppConfig
    {
        public static IConfiguration Configuration { get; }
        public static AppSettings Settings { get; private set; }

        static AppConfig()
        {
            // 建立 IConfiguration 物件並讀取 appsettings.json
            Configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // 使用 Bind 方法將設定綁定到強型別
            Settings = new AppSettings();
            Configuration.GetSection("AppSettings").Bind(Settings);
        }
    }
}
