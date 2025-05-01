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
        /// 信箱Host
        /// </summary>
        public string? MailHost { get; set; }


        /// <summary>
        /// 信箱Port
        /// </summary>
        public int MailPort { get; set; }

        /// <summary>
        /// 寄件者
        /// </summary>
        public string? SendMail { get; set; }
        /// <summary>
        /// 寄件密碼
        /// </summary>
        public string? SendPassword { get; set; }

      

        /// <summary>
        /// 收件者清單
        /// </summary>
        public string? ReceiveMailList { get; set; }

        /// <summary>
        /// CC收件者清單
        /// </summary>
        public string? ReceiveMailCcList { get; set; }

                
        /// <summary>
        /// 主旨
        /// </summary>
        public string? MailSubject { get; set; }
        /// <summary>
        /// 內容
        /// </summary>
        public string? MailBody { get; set; }


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
