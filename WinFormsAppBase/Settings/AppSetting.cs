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
        /// 程式名稱
        /// </summary>
        public int TimerDate { get; set; }

        
        /// <summary>
        /// 檢查本機路徑啟用
        /// </summary>
        public bool LocalFilePathEnable { get; set; }


        /// <summary>
        /// 本機檔案路徑
        /// </summary>
        public string? LocalFilePath { get; set; }


        /// <summary>
        /// 檢查網路路徑啟用
        /// </summary>
        public bool NetworkDriveFilePathEnable { get; set; }


        /// <summary>
        /// 網路磁碟上傳路徑
        /// </summary>
        public string? NetworkDriveFilePath { get; set; }

        /// <summary>
        /// 執行頻率(秒)
        /// </summary>
        public int FrequencySeconds { get; set; }


        /// <summary>
        /// EnableSsl
        /// </summary>
        public bool EnableSsl { get; set; }

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
