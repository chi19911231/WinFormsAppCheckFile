using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsAppBase.Settings
{
    public  class AppFileSetting
    {


        /// <summary>
        /// 執行頻率(秒)
        /// </summary>
        public int FrequencySeconds { get; set; }

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
    }


}
