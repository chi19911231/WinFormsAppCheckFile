namespace WinFormsAppBase.Settings
{
    /// <summary>
    /// 
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// 程式名稱
        /// </summary>
        public string AppName { get; set; }

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
