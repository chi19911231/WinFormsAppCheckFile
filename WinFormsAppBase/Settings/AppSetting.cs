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
        /// 執行頻率
        /// </summary>
        public int TimerDate { get; set; }

        /// <summary>
        /// 重試執行次數
        /// </summary>
        public int RetryMax { get; set; }
        /// <summary>
        /// 停止秒數
        /// </summary>
        public int StopSeconds { get; set; }     

   

        /// <summary>
        /// 視窗(高)
        /// </summary>
        public int WindowWidth { get; set; }
        /// <summary>
        /// 視窗(寬)
        /// </summary>
        public int WindowHeight { get; set; }
        /// <summary>
        /// 視窗是否隱藏
        /// </summary>
        public bool HideWindowEnable { get; set; }
        /// <summary>
        /// 視窗是否隱藏到右下方
        /// </summary>
        public bool NotifyIconEnable { get; set; }
        /// <summary>
        /// 視窗關閉
        /// </summary>
        public bool WindowCloseEnable { get; set; }


    }
}
