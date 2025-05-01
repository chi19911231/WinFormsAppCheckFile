namespace WinFormsAppBase.Settings
{
    /// <summary>
    /// 
    /// </summary>
    public class AppSetting
    {
        /// <summary>
        /// 程式名稱
        /// </summary>
        public string? AppName { get; set; }


        /// <summary>
        /// 本機檔案路徑
        /// </summary>
        public string? LocalFilePath { get; set; }

        /// <summary>
        /// 執行頻率(秒)
        /// </summary>
        public int FrequencySeconds { get; set; }




        /// <summary>
        /// 
        /// </summary>
        public string ApiUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int RetryCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool FeatureEnabled { get; set; }
    }
}
