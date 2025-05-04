using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsAppBase.Settings
{
    public class AppMailSetting
    {

        /// <summary>
        /// SslEnable
        /// </summary>
        public bool SslEnable { get; set; }

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
    }


}
